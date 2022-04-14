using System;
using Grpc.Core;
using Sensory.Api.Common;
using Sensory.Api.Oauth;
using Sensory.Api.V1.Management;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Manages OAuth interactions with Sensory Cloud
    /// </summary>
    public interface IOauthService
    {

        /// <summary>
        /// Creates a new cryptographically random OAuth client
        /// </summary>
        /// <returns></returns>
        OauthClient GenerateCredentials();

        /// <summary>
        /// Obtains an OAuth token used for API authentication
        /// </summary>
        /// <returns></returns>
        OauthToken GetToken();

        /// <summary>
        /// Register credentials provided by the attached SecureCredentialStore to Sensory Cloud. This function should only be called
        /// once per unique credential pair.An error will be thrown if registration fails.
        /// </summary>
        /// <param name="deviceName">The friendly name of the registering device</param>
        /// <param name="credential">The credential configured on the Sensory Cloud server</param>
        DeviceResponse Register(string deviceName, string credential);


        /// <summary>
        /// Renew a device credential if the device credential has expired or is near expiration.
        /// </summary>
        /// <param name="credential">The credential configured on the Sensory Cloud server</param>
        DeviceResponse RenewDeviceCredential(string credential);

        /// <summary>
        /// Get information about the current registered device as inferred by the OAuth credentials supplied by the credential manager.
        /// A new token is request every time this call is made, so use sparingly.
        /// </summary>
        /// <returns>information about the current device</returns>
        DeviceResponse GetWhoAmI();
    }

    /// <summary>
    /// Holds OAuth token and expiration
    /// </summary>
    public struct OauthToken
    {
        public string Token { get; }
        public DateTime Expires { get; }

        public OauthToken(string token, DateTime expires)
        {
            this.Token = token;
            this.Expires = expires;
        }
    }

    /// <summary>
    /// Holds OAuth clientId and secret
    /// </summary>
    public struct OauthClient
    {
        public string ClientId { get; }
        public string ClientSecret { get; }

        public OauthClient(string clientId, string clientSecret)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }
    }

    /// <summary>
    /// Serves credentials from secure storage location such as a database
    /// </summary>
    public interface ISecureCredentialStore
    {
        string GetClientId();
        string GetClientSecret();
    }

    /// <summary>
    /// Service to manage all OAuth-related functions.
    /// </summary>
    public class OauthService: IOauthService
    {
        private readonly Config Config;
        private readonly ISecureCredentialStore SecureCredentialStore;
        private readonly Sensory.Api.Oauth.OauthService.OauthServiceClient OauthClient;
        private readonly DeviceService.DeviceServiceClient DeviceClient;

        public OauthService(Config config, ISecureCredentialStore secureCredentialStore)
        {
            this.Config = config;
            this.SecureCredentialStore = secureCredentialStore;
            this.OauthClient = new Sensory.Api.Oauth.OauthService.OauthServiceClient(config.GetChannel());
            this.DeviceClient = new DeviceService.DeviceServiceClient(config.GetChannel());
        }

        protected OauthService(
            Config config,
            ISecureCredentialStore secureCredentialStore,
            Sensory.Api.Oauth.OauthService.OauthServiceClient oauthClient,
            DeviceService.DeviceServiceClient deviceClient)
        {
            this.Config = config;
            this.SecureCredentialStore = secureCredentialStore;
            this.OauthClient = oauthClient;
            this.DeviceClient = deviceClient;
        }

        /// <summary>
        /// Can be called to generate secure and guaranteed unique credentials.
        /// Should be used the first time the SDK registers and OAuth token with the cloud.
        /// </summary>
        /// <returns>An OAuth Client</returns>
        public OauthClient GenerateCredentials()
        {
            return new OauthClient(Guid.NewGuid().ToString(), CryptoService.GetSecureRandomString(24));
        }

        /// <summary>
        /// Get information about the current registered device as inferred by the OAuth credentials supplied by the credential manager.
        /// A new token is request every time this call is made, so use sparingly.
        /// </summary>
        /// <returns>information about the current device</returns>
        public DeviceResponse GetWhoAmI()
        {
            OauthToken token = this.GetToken();
            Metadata metadata = new Metadata { { "Authorization", "Bearer " + token.Token } };
            return this.DeviceClient.GetWhoAmI(new DeviceGetWhoAmIRequest(), metadata);
        }

        /// <summary>
        /// Obtains an Oauth JWT from Sensory Cloud.
        /// </summary>
        /// <returns>OAuth JWT and expiration</returns>
        public OauthToken GetToken()
        {
            string clientId = this.SecureCredentialStore.GetClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("null clientId was returned from the secure credential store");
            }

            string clientSecret = this.SecureCredentialStore.GetClientSecret();
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("null clientSecret was returned from the secure credential store");
            }

            DateTime now = DateTime.UtcNow;
            TokenRequest request = new TokenRequest{ClientId = clientId, Secret = clientSecret};
            TokenResponse tokenResponse = this.OauthClient.GetToken(request);

            return new OauthToken(tokenResponse.AccessToken, now.AddSeconds(tokenResponse.ExpiresIn));
        }

        /// <summary>
        /// Register credentials provided by the attached SecureCredentialStore to Sensory Cloud. This function should only be called
        /// once per unique credential pair. An error will be thrown if registration fails.
        /// </summary>
        /// <param name="deviceName">The friendly name of the registering device</param>
        /// <param name="credential">The credential configured on the Sensory Cloud server</param>
        /// <returns>device response indicating the device was successfully registered</returns>
        public DeviceResponse Register(string deviceName, string credential)
        {
            string clientId = this.SecureCredentialStore.GetClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("null clientId was returned from the secure credential store");
            }

            string clientSecret = this.SecureCredentialStore.GetClientSecret();
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("null clientSecret was returned from the secure credential store");
            }

            GenericClient client = new GenericClient { ClientId = clientId, Secret = clientSecret };
            EnrollDeviceRequest request = new EnrollDeviceRequest
            {
                Client = client,
                DeviceId = this.Config.DeviceId,
                Name = deviceName,
                Credential = credential,
                TenantId = Config.TenantId
            };

            return this.DeviceClient.EnrollDevice(request);
        }

        public DeviceResponse RenewDeviceCredential(string credential)
        {
            string clientId = this.SecureCredentialStore.GetClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("null clientId was returned from the secure credential store");
            }

            RenewDeviceCredentialRequest request = new RenewDeviceCredentialRequest
            {
                ClientId = clientId,
                Credential = credential,
                DeviceId = Config.DeviceId,
                TenantId = Config.TenantId
            };

            return this.DeviceClient.RenewDeviceCredential(request);
        }
    }
}
