using NUnit.Framework;
using Moq;
using Sensory.Api.Common;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using System.Threading;

namespace Test.Services
{
    [TestFixture]
    public class HealthServiceTest
    {
        
        [Test]
        public void TestGetHealth()
        {
            var healthClient = new Mock<Sensory.Api.Health.HealthService.HealthServiceClient>();
            var response = new ServerHealthResponse
            {
                IsHealthy = true,
                ServerVersion = "1.2.3",
            };

            response.Services.Add(new ServiceHealth { IsHealthy = true, Message = "ok", Name = "test" });

            healthClient.Setup(client => client.GetHealth(It.IsAny<Sensory.Api.Health.HealthRequest>(), null, null, CancellationToken.None)).Returns(response);

            var healthService = new MockHealthService(new Config("doesnt-matter", "doesnt-matter"), healthClient.Object);

            var healthResponse = healthService.GetHealth();
            Assert.AreSame(healthResponse, response);
        }
    }

    public class MockHealthService: HealthService
    {
        public MockHealthService(Config config, Sensory.Api.Health.HealthService.HealthServiceClient healthClient) : base(config, healthClient)
        {
        }
    }
}
