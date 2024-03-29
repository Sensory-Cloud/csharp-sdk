﻿using NUnit.Framework;
using Moq;
using Sensory.Api.Common;
using Sensory.Api.V1.Management;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using System;
using System.Threading;
using Grpc.Core;

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
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var credentials = oauthService.GenerateCredentials();

            Assert.IsTrue(Guid.TryParse(credentials.ClientId, out _), "the generated clientId should be a valid GUID");
            Assert.AreEqual(credentials.ClientSecret.Length, 16, "the generated secret should be exactly 16 characters");
        }

        [Test]
        public void TestGetWhoAmI()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var tokenResponse = new TokenResponse { AccessToken = "fake-token", ExpiresIn = 0 };
            var response = new DeviceResponse { DeviceId = "12345", Name = "Doesn't Matter" };

            oauthClient.Setup(client => client.GetToken(It.IsAny<Sensory.Api.Oauth.TokenRequest>(), null, null, CancellationToken.None)).Returns(tokenResponse);
            deviceClient.Setup(client => client.GetWhoAmI(It.IsAny<DeviceGetWhoAmIRequest>(), It.IsAny<Metadata>(), null, CancellationToken.None)).Returns(response);

            OauthService oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var deviceResponse = oauthService.GetWhoAmI();

            Assert.AreSame(deviceResponse, response, "the who am I response should be correct");
        }

        [Test]
        public void TestGetTokenNullCredentials()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();

            OauthService oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = null, ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.GetToken();
            }, "oauthService should throw an error if no clientId is found in secure storage");

            oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
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
                new Config(new SDKInitConfig { }),
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
            var deviceName = "matter";
            var credential = "at-all";

            OauthService oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = null, ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.Register(deviceName, credential);
            }, "oauthService should throw an error if no clientId is found in secure storage");

            oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "" },
                oauthClient.Object,
                deviceClient.Object);

            Assert.Throws<ArgumentNullException>(delegate {
                oauthService.Register(deviceName, credential);
            }, "oauthService should throw an error if no clientId is found in secure storage");
        }

        [Test]
        public void TestRegister()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var deviceName = "matter";
            var credential = "at-all";
            var response = new DeviceResponse { DeviceId = "doesnt", Name = deviceName };

            deviceClient.Setup(client => client.EnrollDevice(It.IsAny<EnrollDeviceRequest>(), null, null, CancellationToken.None)).Returns(response);

            OauthService oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var deviceResponse = oauthService.Register(deviceName, credential);
            Assert.AreEqual(deviceResponse.Name, deviceName);
            Assert.AreEqual(deviceResponse.DeviceId, response.DeviceId);
        }


        [Test]
        public void TestRenewCredential()
        {
            var oauthClient = new Mock<Sensory.Api.Oauth.OauthService.OauthServiceClient>();
            var deviceClient = new Mock<DeviceService.DeviceServiceClient>();
            var deviceName = "matter";
            var credential = "at-all";
            var response = new DeviceResponse { DeviceId = "doesnt", Name = deviceName };

            deviceClient.Setup(client => client.RenewDeviceCredential(It.IsAny<RenewDeviceCredentialRequest>(), null, null, CancellationToken.None)).Returns(response);

            OauthService oauthService = new MockOauthService(
                new Config(new SDKInitConfig { }),
                new MockSecureCredentialsStore { ClientId = "doesnt-matter", ClientSecret = "doesnt-matter" },
                oauthClient.Object,
                deviceClient.Object);

            var deviceResponse = oauthService.RenewDeviceCredential("doest-matter");
            Assert.AreEqual(deviceResponse.Name, deviceName);
            Assert.AreEqual(deviceResponse.DeviceId, response.DeviceId);
        }
    }

    public class MockOauthService : OauthService
    {
        public MockOauthService(
            Config config,
            ISecureCredentialStore secureCredentialStore,
            Sensory.Api.Oauth.OauthService.OauthServiceClient oauthClient,
            DeviceService.DeviceServiceClient deviceClient) : base(config, secureCredentialStore, oauthClient, deviceClient) { }
    }

    public class MockSecureCredentialsStore : ISecureCredentialStore
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public void ClearCredentials()
        {
            this.ClientId = null;
            this.ClientSecret = null;
        }

        public string GetClientId()
        {
            return ClientId;
        }

        public string GetClientSecret()
        {
            return ClientSecret;
        }

        public bool IsConfigured()
        {
            return this.ClientId != null;
        }

        public void SaveCredentials(OauthClient credentials)
        {
            this.ClientId = credentials.ClientId;
            this.ClientSecret = credentials.ClientSecret;
        }
    }
}
