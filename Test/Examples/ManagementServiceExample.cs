using System;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;
using SensoryCloud.Src.TokenManager;

namespace Test.Examples
{
    public static class ManagementServiceExample
    {
        public static ManagementService GetManagementService()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
            string deviceId = "a-hardware-identifier-unique-to-your-device";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId, deviceId);

            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            IOauthService oauthService = new OauthService(config, credentialStore);
            ITokenManager tokenManager = new TokenManager(oauthService);

            return new ManagementService(config, tokenManager);
        }
    }
}
