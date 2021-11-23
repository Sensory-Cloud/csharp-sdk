using System;
using NUnit.Framework;
using Sensory.Api.V1.Management;
using SensoryCloud.Src.Services;

namespace Test.TokenManager
{
    [TestFixture]
    public class TokenManagerTest
    {
        [Test]
        public void TestGetToken()
        {
            var oauthToken = new OAuthToken("my-token", DateTime.UtcNow);
            var oauthService = new MockOauthService { OAuthToken = oauthToken };

            var tokenManager = new SensoryCloud.Src.TokenManager.TokenManager(oauthService);

            var token = tokenManager.GetToken();
            Assert.AreEqual(token, oauthService.OAuthToken.Token, "the token returned should be correct");
            Assert.AreEqual(oauthService.GetTokenWasCalled, 1, "Oauth service should be called if no token is populated");

            oauthService.OAuthToken = new OAuthToken("another-token", DateTime.UtcNow);

            token = tokenManager.GetToken();
            Assert.AreEqual(token, oauthService.OAuthToken.Token, "the token returned should be correct");
            Assert.AreEqual(oauthService.GetTokenWasCalled, 2, "Oauth service should not be called");

            oauthService.OAuthToken = new OAuthToken("a-final-token", DateTime.UtcNow.AddDays(1));

            token = tokenManager.GetToken();
            Assert.AreEqual(token, oauthService.OAuthToken.Token);
            Assert.AreEqual(oauthService.GetTokenWasCalled, 3, "Oauth service should be called if token is expired");

            token = tokenManager.GetToken();
            Assert.AreEqual(token, oauthService.OAuthToken.Token);
            Assert.AreEqual(oauthService.GetTokenWasCalled, 3, "Oauth service should be called if token is expired");
        }

        [Test]
        public void TestGetAuthorizationMetadata()
        {
            var oauthToken = new OAuthToken("my-token", DateTime.UtcNow);
            var oauthService = new MockOauthService { OAuthToken = oauthToken };

            var tokenManager = new SensoryCloud.Src.TokenManager.TokenManager(oauthService);

            tokenManager.GetAuthorizationMetadata();
            Assert.AreEqual(oauthService.GetTokenWasCalled, 1, "Oauth service should be called if no token is populated");
        }
    }

    public class MockOauthService : IOauthService
    {
        public OAuthToken OAuthToken { get; set; }
        public int GetTokenWasCalled { get; private set; }

        public OAuthClient GenerateCredentials()
        {
            throw new NotImplementedException();
        }

        public OAuthToken GetToken()
        {
            this.GetTokenWasCalled++;
            return this.OAuthToken;
        }

        public DeviceResponse Register(string deviceId, string deviceName, string credential)
        {
            throw new NotImplementedException();
        }
    }
}
