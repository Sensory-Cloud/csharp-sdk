using Sensory.Api.Common;
using Sensory.Api.Health;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Package used to obtain health status of Sensory Cload
    /// </summary>
    public class HealthService
    {
        private readonly Config Config;
        private readonly Sensory.Api.Health.HealthService.HealthServiceClient HealthClient;

        public HealthService(Config config)
        {
            this.Config = config;
            this.HealthClient = new Sensory.Api.Health.HealthService.HealthServiceClient(config.GetChannel());
        }

        protected HealthService(Config config, Sensory.Api.Health.HealthService.HealthServiceClient healthClient)
        {
            this.Config = config;
            this.HealthClient = healthClient;
        }

        /// <summary>
        /// Get the health status of the cloud endpoint defined in Config
        /// </summary>
        /// <returns>the health of the server</returns>
        public ServerHealthResponse GetHealth()
        {
            return this.HealthClient.GetHealth(new HealthRequest());
        }
    }
}
