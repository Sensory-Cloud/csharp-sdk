using NUnit.Framework;
using Moq;
using Sensory.Api.Common;
using Sensory.Api.V1.Management;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using System;
using System.Threading;

namespace Test.Services
{
    [TestFixture]
    public class OauthServiceTest
    {

        [Test]
        public void TestGenerateCredentials()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();

            OauthService oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var credentials = oauthService.GenerateCredentials();

            Assert.IsTrue(Guid.TryParse(credentials.ClientId, out _), "the generated clientId should be a valid GUID");
            Assert.AreEqual(credentials.ClientSecret.Length, 16, "the generated secret should be exactly 16 characters");
        }

        [Test]
        public void TestGetTokenNullCredentials()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();

            OauthService oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = null, ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.GetToken();
            }, "oauthService should throw an error if no clientId is found in secure storage");

            oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.GetToken();
            }, "oauthService should throw an error if no clientId is found in secure storage");
        }

        [Test]
        public void TestGetToken()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var tokenResponse = new TokenResponse { AccessToken = "fake-token", ExpiresIn = 0 };

            oauthClient.Setup(client => client.GetToken(It.IsAny<Sensory.Api.Oauth.TokenRequest>(), null, null, CancellationToken.None)).Returns(tokenResponse);

            OauthService oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var oauthToken = oauthService.GetToken();
            Assert.AreEqual(oauthToken.Token, tokenResponse.AccessToken, "returned access token should be the same");
            Assert.IsTrue(oauthToken.Expires.CompareTo(DateTime.UtcNow) <= 0, "token expiration should be earlier than or equal to now");
        }

        [Test]
        public void TestRegisterNullCredentials()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var deviceId = "doesnt";
            var deviceName = "matter";
            var credential = "at-all";

            OauthService oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = null, ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.Register(deviceId, deviceName, credential);
            }, "oauthService should throw an error if no clientId is found in secure storage");

            oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.Register(deviceId, deviceName, credential);
            }, "oauthService should throw an error if no clientId is found in secure storage");
        }

        [Test]
        public void TestRegister()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var deviceId = "doesnt";
            var deviceName = "matter";
            var credential = "at-all";
            var response = new DeviceResponse { DeviceId = deviceId, Name = deviceName };

            deviceClient.Setup(client => client.EnrollDevice(It.IsAny<EnrollDeviceRequest>(), null, null, CancellationToken.None)).Returns(response);

            OauthService oauthService = new MockOauthService(
                new Config("doesnt-matter", "doesnt-matter"),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var deviceResponse = oauthService.Register(deviceId, deviceName, credential);
            Assert.AreEqual(deviceResponse.Name, deviceName);
            Assert.AreEqual(deviceResponse.DeviceId, deviceId);
        }
    }

    public class MockOauthService : OauthService
    {
        public MockOauthService(
            Config config,
            ISecureCredentialStore secureCredentialStore,
            Sensory.Api.Oauth.OauthService.OauthServiceClient oauthClient,
            DeviceService.DeviceServiceClient deviceClient): base(config, secureCredentialStore, oauthClient, deviceClient) { }
    }

    public class MockSecureCredentialsStore : ISecureCredentialStore
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string GetClientId()
        {
            return ClientId;
        }

        public string GetClientSecret()
        {
            return ClientSecret;
        }
    }
}
