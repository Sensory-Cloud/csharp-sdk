using System;
using SensoryCloud.Src;
using SensoryCloud.Src.Initializer;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    public class InitializationExample
    {
        public InitializationExample()
        {
        }

        public static Config InitializeExample()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
            string deviceId = "a-hardware-identifier-unique-to-your-device";

            // Create a secure crendetial manager and OAuth service
            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();

            var initializer = new Initializer(credentialStore);
            return initializer.Initialize(new SDKInitConfig
            {
                FullyQualifiedDomainName = "https://your-inference-server.com",
                IsConnectionSecure = true,
                TenantId = sensoryTenantId,
                EnrollmentType = EnrollmentType.SharedSecret,
                Credential = "credential-provided-by-sensory",
                DeviceId = deviceId,
                DeviceName = "a friendly device name"
            });
        }
    }
}
