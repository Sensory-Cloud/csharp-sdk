﻿using System;
using System.Security.Cryptography;
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
        OAuthClient GenerateCredentials();

        /// <summary>
        /// Obtains an OAuth token used for API authentication
        /// </summary>
        /// <returns></returns>
        OAuthToken GetToken();

        /// <summary>
        /// Register credentials provided by the attached SecureCredentialStore to Sensory Cloud. This function should only be called
        /// once per unique credential pair.An error will be thrown if registration fails.
        /// </summary>
        /// <param name="deviceId">The unique hardware identifier of the registering device</param>
        /// <param name="deviceName">The friendly name of the registering device</param>
        /// <param name="credential">The credential configured on the Sensory Cloud server</param>
        void Register(string deviceId, string deviceName, string credential);
    }

    /// <summary>
    /// Holds OAuth token and expiration
    /// </summary>
    public struct OAuthToken
    {
        public string Token { get; }
        public DateTime Expires { get; }

        public OAuthToken(string token, DateTime expires)
        {
            this.Token = token;
            this.Expires = expires;
        }
    }

    /// <summary>
    /// Holds OAuth clientId and secret
    /// </summary>
    public struct OAuthClient
    {
        string ClientId { get; }
        string ClientSecret { get; }

        public OAuthClient(string clientId, string clientSecret)
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

        /// <summary>
        /// Can be called to generate secure and guaranteed unique credentials.
        /// Should be used the first time the SDK registers and OAuth token with the cloud.
        /// </summary>
        /// <returns>An OAuth Client</returns>
        public OAuthClient GenerateCredentials()
        {
            return new OAuthClient(Guid.NewGuid().ToString(), CryptoService.GetSecureRandomString(16));
        }

        /// <summary>
        /// Obtains an Oauth JWT from Sensory Cloud.
        /// </summary>
        /// <returns>OAuth JWT and expiration</returns>
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
            TokenRequest request = new TokenRequest{ClientId = clientId, Secret = clientSecret};
            TokenResponse tokenResponse = this.OauthClient.GetToken(request);

            return new OAuthToken(tokenResponse.AccessToken, now.AddSeconds(tokenResponse.ExpiresIn));
        }

        /// <summary>
        /// Register credentials provided by the attached SecureCredentialStore to Sensory Cloud. This function should only be called
        /// once per unique credential pair. An error will be thrown if registration fails.
        /// </summary>
        /// <param name="deviceId">The unique hardware identifier of the registering device</param>
        /// <param name="deviceName">The friendly name of the registering device</param>
        /// <param name="credential">The credential configured on the Sensory Cloud server</param>
        public void Register(string deviceId, string deviceName, string credential)
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

            GenericClient client = new GenericClient { ClientId = clientId, Secret = clientSecret };
            EnrollDeviceRequest request = new EnrollDeviceRequest
            {
                Client = client,
                DeviceId = deviceId,
                Name = deviceName,
                Credential = credential,
                TenantId = Config.TenantId
            };

            this.DeviceClient.EnrollDevice(request);
        }
    }
}