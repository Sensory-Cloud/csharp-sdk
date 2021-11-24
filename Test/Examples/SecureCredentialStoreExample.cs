using System;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    public class SecureCredentialStoreExample : ISecureCredentialStore
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
