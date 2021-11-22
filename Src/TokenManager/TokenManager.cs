using System;
using System.Threading;
using Grpc.Core;
using SensoryCloud.Src.Services;

namespace SensoryCloud.Src.TokenManager
{
    /// <summary>
    /// Interface to handle the management of OAuth Tokens
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Get a valid OAuth token.
        /// </summary>
        /// <returns>A JWT as a string</returns>
        string GetToken();

        /// <summary>
        /// Get a token wrapped in grpc metadata.
        /// </summary>
        /// <returns>A JWT wripped in grpc metadata</returns>
        Metadata GetAuthorizationMetadata();
    }

    /// <summary>
    ///  Manages the rotation and injection of OAuth JWTs into grpc requests
    /// </summary>
    public class TokenManager: ITokenManager
    {
        private readonly int ExpiresBufferSeconds = 60 * 60; // 1 hour
        private readonly IOauthService OauthService;
        private static Mutex TokenMutex = new Mutex();
        private string Token = null;
        private DateTime? Expires = null;


        public TokenManager(IOauthService oauthService) {
            this.OauthService = oauthService;
        }

        /// <summary>
        /// Gets a cached local token if the token exists and is not expired. Otherwise, requests a new token from Sensory Cloud.
        /// </summary>
        /// <returns>The JWT as a string</returns>
        public string GetToken()
        {
            TokenMutex.WaitOne();
            if (!String.IsNullOrEmpty(this.Token) && this.Expires != null && DateTime.UtcNow < this.Expires)
            {
                TokenMutex.ReleaseMutex();
                return this.Token;
            }

            try
            {
                OAuthToken oAuthToken = this.OauthService.GetToken();
                this.SetToken(oAuthToken);
                TokenMutex.ReleaseMutex();
                return oAuthToken.Token;
            } catch(Exception ex)
            {
                TokenMutex.ReleaseMutex();
                throw ex;
            }

            
        }

        /// <summary>
        /// A helper function for grpc calls. Returns the getToken() call packed into grpc Metadata.
        /// </summary>
        /// <returns>The JWT as grpc Metadata</returns>
        public Metadata GetAuthorizationMetadata()
        {
            string token = this.GetToken();
            return new Metadata
            {
                {"Authorization", "Bearer " + token }
            };
        }

        private void SetToken(OAuthToken oAuthToken)
        {
            this.Token = oAuthToken.Token;
            this.Expires = oAuthToken.Expires.AddSeconds(-1 * this.ExpiresBufferSeconds);
        }
    }
}
