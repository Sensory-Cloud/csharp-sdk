using System;
using SensoryCloud.Src.FileParser;
using SensoryCloud.Src.Services;

namespace SensoryCloud.Src.Initializer
{
    // IJWTSigner see the readme section labeled "Registering OAuth Credentials"
    public interface IJWTSigner
    {
        string SignJWT(string enrollmentKey, string deviceName, string tenantID, string clientID);
    }

    public class Initializer
    {
        private ISecureCredentialStore SecureCredentialStore;
        private IJWTSigner JWTSigner;
        private SDKInitConfig Config;

        // jWTSigner only needs to be defined if you have an enrollment type of JWT
        public Initializer(ISecureCredentialStore secureCredentialStore, IJWTSigner jWTSigner)
        {
            this.SecureCredentialStore = secureCredentialStore;
            this.JWTSigner = jWTSigner;
        }

        public Initializer(ISecureCredentialStore secureCredentialStore)
        {
            this.SecureCredentialStore = secureCredentialStore;
        }

        public void InitializeFromFile(string filepath)
        {
            IniFile iniFile = new IniFile(filepath);
            SDKInitConfig config = iniFile.LoadConfig();
            this.Initialize(config);
        }

        public Config Initialize(SDKInitConfig config)
        {
            this.Config = config;

            // Configuration specific to your tenant
            Config configuration = new Config(this.Config).Connect();

            // Check if we are already registered
            if (this.SecureCredentialStore.IsConfigured())
            {
                // Do nothing
                return configuration;
            }

            // Create new OauthService
            OauthService oauthService = new OauthService(configuration, this.SecureCredentialStore);

            // Generate cryptographically random credentials
            OauthClient oAuthClient = oauthService.GenerateCredentials();

            // Store credentials
            this.SecureCredentialStore.SaveCredentials(oAuthClient);

            string credential = "";
            if (config.EnrollmentType.Equals(EnrollmentType.JWT))
            {
                credential = this.JWTSigner.SignJWT(this.Config.Credential, this.Config.DeviceName, this.Config.TenantId, oAuthClient.ClientId);
            }
            else
            {
                credential = this.Config.Credential;
            }

            try
            {
                oauthService.Register(this.Config.DeviceName, credential);
            }
            catch (Exception ex)
            {
                this.SecureCredentialStore.ClearCredentials();
                throw ex;
            }
            
            return configuration;
        }
    }
}
