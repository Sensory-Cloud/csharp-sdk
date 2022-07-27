using System;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    /// <summary>
    /// This example should not be used in production. It does not persist data
    /// to disk or in any permanent manner and it is not secure.
    /// </summary>
    public class SecureCredentialStoreExample : ISecureCredentialStore
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
