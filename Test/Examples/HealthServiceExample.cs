using System;
using Sensory.Api.Common;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    public static class HealthServiceExample
    {
        public static void GetHealth()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId);

            HealthService healthService = new HealthService(config);

            ServerHealthResponse serverHealth = healthService.GetHealth();
        }
    }
}
