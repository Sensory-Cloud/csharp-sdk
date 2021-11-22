using System;
using Grpc.Core;
using Sensory.Api.Common;
using Sensory.Api.Oauth;

namespace SensoryCloud.Src.Services
{
    public struct OAuthToken
    {
        string Token { get; }
        DateTime Expires { get; }

        public OAuthToken(string token, DateTime expires)
        {
            this.Token = token;
            this.Expires = expires;
        }
    }

    public interface ISecureCredentialStore
    {
        string GetClientId();
        string GetClientSecret();
    }

    public class OauthService
    {
        private readonly Config Config;
        private readonly ISecureCredentialStore SecureCredentialStore;
        private readonly Sensory.Api.Oauth.OauthService.OauthServiceClient OauthClient;

        public OauthService(Config config, ISecureCredentialStore secureCredentialStore)
        {
            this.Config = config;
            this.SecureCredentialStore = secureCredentialStore;
            this.OauthClient = new Sensory.Api.Oauth.OauthService.OauthServiceClient(this.Config.GetChannel());
        }

        public OAuthToken GetToken()
        {
            string clientId = this.SecureCredentialStore.GetClientId();
            if (String.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("null clientId was returned from the secure credential store");
            }

            string clientSecret = this.SecureCredentialStore.GetClientSecret();
            if (String.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("null clientSecret was returned from the secure credential store");
            }

            DateTime now = DateTime.UtcNow;

            TokenRequest body = new TokenRequest();
            body.ClientId = clientId;
            body.Secret = clientSecret;

            TokenResponse tokenResponse = this.OauthClient.GetToken(body);

            return new OAuthToken(tokenResponse.AccessToken, now.AddSeconds(tokenResponse.ExpiresIn));
        }
    }
}
