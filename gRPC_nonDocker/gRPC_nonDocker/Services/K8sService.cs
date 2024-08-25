using Grpc.Core;
using gRPC_nonDocker.Protos;
using IdentityModel.OidcClient;
using k8s;
using k8s.Models;

namespace gRPC_nonDocker.Services
{
    public class K8sService : K8s.K8sBase
    {
        private readonly ILogger<K8sService> _logger;
        private KubernetesClientConfiguration _k8sconfig;
        public K8sService(ILogger<K8sService> logger, KubernetesClientConfiguration k8sconfig)
        {
            _logger = logger;
            _k8sconfig = k8sconfig;
        }

        public override Task<CreateDockerContainerPodResponse> CreateDockerContainerPod(CreateDockerContainerPodRequest request, ServerCallContext context)
        {

           try
            {
                IKubernetes client = new Kubernetes(_k8sconfig);
                // Define the pod specification - demo but will use infor from request
                var pod = new V1Pod
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = request.Name, // Name of the pod
                        Labels = new Dictionary<string, string> { { "app", "example" } }
                    },
                    Spec = new V1PodSpec
                    {
                        Containers = new List<V1Container>
                {
                    new V1Container
                    {
                        Name = "example-container",
                        Image = "nginx", // Docker image to run
                        Ports = new List<V1ContainerPort>
                        {
                            new V1ContainerPort(containerPort: 80)
                        }
                    }
                }
                    }
                };
                // Create the pod in the default namespace
                var result =  client.CoreV1.CreateNamespacedPodAsync(pod, namespaceParameter: "default").Result;
                Console.WriteLine($"Pod created with name: {result.Metadata.Name}");
                return Task.FromResult(new CreateDockerContainerPodResponse { Mess = $"{result.Metadata.Name}"});
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when creating Pod: {ex.Message}");
                return Task.FromResult(new CreateDockerContainerPodResponse { Mess = $"{ex.Message}" });
            }
        }
    }
    
}
