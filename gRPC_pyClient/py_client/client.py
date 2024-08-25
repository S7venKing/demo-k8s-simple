from flask import Flask, jsonify
import grpc
import k8s_pb2
import k8s_pb2_grpc

app = Flask(__name__)

@app.route('/api/k8s/createpod', methods=['POST'])
def get_customer():
    try:
        # Make gRPC call to the K8sService
        with grpc.insecure_channel('localhost:5137') as channel:
            stub = k8s_pb2_grpc.K8sStub(channel)
            #infor and pod resource will be described in body of request
            #now only demo
            grpc_request = k8s_pb2.CreateDockerContainerPodRequest(name = "test-pod")
            grpc_response = stub.CreateDockerContainerPod(grpc_request)

        return grpc_response
    except Exception as e:
        return jsonify({'message': f"Error: {e}"})

if __name__ == '__main__':
    app.run(debug=True)