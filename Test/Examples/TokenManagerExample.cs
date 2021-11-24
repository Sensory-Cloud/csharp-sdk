using System;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test.Examples
{
    public static class TokenManagerExample
    {
        public static ITokenManager GetTokenManager()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId);

            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            IOAuthService oAuthService = new OAuthService(config, credentialStore);
            ITokenManager tokenManager = new TokenManager(OAuthService);

            // Obtain OAuth token from Sensory Cloud
            string token = tokenManager.GetToken();

            return tokenManager;
        }
    }
}
