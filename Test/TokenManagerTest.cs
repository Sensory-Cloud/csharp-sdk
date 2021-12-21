using System;
using NUnit.Framework;
using Sensory.Api.V1.Management;
using SensoryCloud.Src.Services;

namespace Test
{
    [TestFixture]
    public class TokenManagerTest
    {
        [Test]
        public void TestGetToken()
        {
            var OauthToken = new OauthToken("my-token", DateTime.UtcNow);
            var OAuthService = new TestOAuthService { OauthToken = OauthToken };

            var tokenManager = new SensoryCloud.Src.TokenManager.TokenManager(OAuthService);

            var token = tokenManager.GetToken();
            Assert.AreEqual(token, OAuthService.OauthToken.Token, "the token returned should be correct");
            Assert.AreEqual(OAuthService.GetTokenWasCalled, 1, "OAuth service should be called if no token is populated");

            OAuthService.OauthToken = new OauthToken("another-token", DateTime.UtcNow);

            token = tokenManager.GetToken();
            Assert.AreEqual(token, OAuthService.OauthToken.Token, "the token returned should be correct");
            Assert.AreEqual(OAuthService.GetTokenWasCalled, 2, "OAuth service should not be called");

            OAuthService.OauthToken = new OauthToken("a-final-token", DateTime.UtcNow.AddDays(1));

            token = tokenManager.GetToken();
            Assert.AreEqual(token, OAuthService.OauthToken.Token);
            Assert.AreEqual(OAuthService.GetTokenWasCalled, 3, "OAuth service should be called if token is expired");

            token = tokenManager.GetToken();
            Assert.AreEqual(token, OAuthService.OauthToken.Token);
            Assert.AreEqual(OAuthService.GetTokenWasCalled, 3, "OAuth service should be called if token is expired");
        }

        [Test]
        public void TestGetAuthorizationMetadata()
        {
            var oAuthToken = new OauthToken("my-token", DateTime.UtcNow);
            var oAuthService = new TestOAuthService { OauthToken = oAuthToken };

            var tokenManager = new SensoryCloud.Src.TokenManager.TokenManager(oAuthService);

            tokenManager.GetAuthorizationMetadata();
            Assert.AreEqual(oAuthService.GetTokenWasCalled, 1, "OAuth service should be called if no token is populated");
        }
    }

    public class TestOAuthService : IOauthService
    {
        public OauthToken OauthToken { get; set; }
        public int GetTokenWasCalled { get; private set; }

        public OauthClient GenerateCredentials()
        {
            throw new NotImplementedException();
        }

        public OauthToken GetToken()
        {
            this.GetTokenWasCalled++;
            return this.OauthToken;
        }

        public DeviceResponse GetWhoAmI()
        {
            throw new NotImplementedException();
        }

        public DeviceResponse Register(string deviceName, string credential)
        {
            throw new NotImplementedException();
        }
    }
}
