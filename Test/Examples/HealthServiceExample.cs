using System;
using Sensory.Api.Common;
using SensoryCloud.Src;
using SensoryCloud.Src.Initializer;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    public static class HealthServiceExample
    {
        public static void GetHealth()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
            string deviceId = "a-hardware-identifier-unique-to-your-device";

            // Configuration specific to your tenant
            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            var initializer = new Initializer(credentialStore);
            Config config = initializer.Initialize(new SDKInitConfig
            {
                FullyQualifiedDomainName = "https://your-inference-server.com",
                IsConnectionSecure = true,
                TenantId = sensoryTenantId,
                EnrollmentType = EnrollmentType.SharedSecret,
                Credential = "credential-provided-by-sensory",
                DeviceId = deviceId,
                DeviceName = "a friendly device name"
            });

            HealthService healthService = new HealthService(config);

            ServerHealthResponse serverHealth = healthService.GetHealth();
        }
    }
}
