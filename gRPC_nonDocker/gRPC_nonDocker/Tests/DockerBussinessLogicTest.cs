using Docker.DotNet;
using gRPC_nonDocker.BussinessLogic;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace gRPC_nonDocker.Tests
{
    public class DockerBussinessLogicTest
    {
        private DockerClient _dockerClient;

        private IOptions<DockerConfig> _dockerConfig;

        private object _lock = new object();

        private readonly Mock<ILogger<DockerBussinessLogic>> _mockLogger;

        DockerBussinessLogic dockerBussinessLogic;
        public DockerBussinessLogicTest()
        {
            _mockLogger = new Mock<ILogger<DockerBussinessLogic>>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            _dockerConfig = Options.Create(configuration.GetSection("DockerConfig").Get<DockerConfig>());
            dockerBussinessLogic = new DockerBussinessLogic(_mockLogger.Object, _dockerConfig);
        }

        [Fact]
        public void GetDockerClientTest()
        {
            var client1 = dockerBussinessLogic.GetDockerClient();
            var client2 = dockerBussinessLogic.GetDockerClient();
            Assert.Equal(client1, client2);

        }
    }
}
