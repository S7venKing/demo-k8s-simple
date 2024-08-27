using Docker.DotNet;
using Docker.DotNet.X509;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace gRPC_nonDocker.BussinessLogic
{
    public class DockerBussinessLogic
    {
        private DockerClient _dockerClient;

        private DockerConfig _dockerConfigValue;

        private object _lock = new object();

        private readonly ILogger<DockerBussinessLogic> _logger;
        public DockerBussinessLogic(ILogger<DockerBussinessLogic> logger, IOptions<DockerConfig> dockerconfig)
        {
            _logger = logger;
            _dockerConfigValue = dockerconfig.Value;
        }

        public DockerClient? GetDockerClient()
        {
            // Use double-checked locking to ensure thread safety
            if (_dockerClient == null)
            {
                try
                {
                    lock (_lock)
                    {
                        if (_dockerClient == null)
                        {
                            if (_dockerConfigValue.DockerHost.IsNullOrEmpty()
                                || _dockerConfigValue.DockerCA.IsNullOrEmpty()
                                 || _dockerConfigValue.DockerClient.IsNullOrEmpty()
                                    || _dockerConfigValue.DockerKey.IsNullOrEmpty())
                            {
                                // Initialize the Docker client if it doesn't already exist
                                var config = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"));

                                // For Windows, use:
                                // var config = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
                                _dockerClient = config.CreateClient();
                            }
                            else
                            {
                                _logger.LogDebug("Connecting to docker daemon with config");
                                var credentials = new CertificateCredentials(
                                    new X509Certificate2(_dockerConfigValue.DockerClient, _dockerConfigValue.DockerKey));
                                var config = new DockerClientConfiguration(new Uri(_dockerConfigValue.DockerHost), credentials);


                                _dockerClient = config.CreateClient();
                            }

                            _dockerClient.System.PingAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug("Could not connect to docker daemon:" + ex.Message);
                }

            }

            return _dockerClient;
        }
    }
}
