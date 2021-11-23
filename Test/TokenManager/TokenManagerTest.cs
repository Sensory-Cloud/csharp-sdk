using System;
using Grpc.Core;
using NUnit.Framework;

using SensoryCloud.Src.TokenManager;

namespace Test.TokenManager
{
    [TestFixture]
    public class TokenManagerTest
    {
      
    }

    public class MockTokenManager : ITokenManager
    {
        public string Token { get; }

        public Metadata GetAuthorizationMetadata()
        {
            string token = this.GetToken();
            return new Metadata
            {
                {"Authorization", "Bearer " + token }
            };
        }

        public string GetToken()
        {
            return this.Token;
        }
    }
}
