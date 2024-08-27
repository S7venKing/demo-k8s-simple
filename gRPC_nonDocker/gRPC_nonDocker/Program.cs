using gRPC_nonDocker.BussinessLogic;
using gRPC_nonDocker.Services;
using k8s;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<KubernetesClientConfiguration>(_=> KubernetesClientConfiguration.BuildConfigFromConfigFile());

// Bind DockerConfig section
builder.Services.Configure<DockerConfig>(builder.Configuration.GetSection("DockerConfig"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CustomerService>();
app.MapGrpcService<K8sService>();
app.MapGrpcService<DockerBussinessLogic>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
