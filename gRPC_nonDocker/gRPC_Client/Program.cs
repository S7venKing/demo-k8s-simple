// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using gRPC_nonDocker;

var channel = GrpcChannel.ForAddress("http://localhost:5137");
var client = new Customer.CustomerClient(channel);

var k8sClient = new K8s.K8sClient(channel);

var k8sCall = k8sClient.CreateDockerContainerPod(new CreateDockerContainerPodRequest() { Name = "test"});
Console.WriteLine(k8sCall.Message);

Console.WriteLine();

Console.WriteLine("List of customers: ");

using (var call = client.GetNewCustomers(new NewCustomerRequest()))
{
    int i = 1;
    while (await call.ResponseStream.MoveNext())
    {
        var currentCust = call.ResponseStream.Current as CustomerModel;
        Console.WriteLine($"{i++}) {currentCust.FirstName} {currentCust.LastName} - {currentCust.Email} - Age: {currentCust.Age} - Status: {(currentCust.IsAlive ? "ALIVE" : "DEAD")}");
    }
}

Console.ReadLine();