using System;
using Grpc.Core;

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

        Metadata GetAuthorizationMetadata();
    }

    /// <summary>
    /// 
    /// </summary>
    public class TokenManager: ITokenManager
    {
        public TokenManager()
        {
        }
    }
}
