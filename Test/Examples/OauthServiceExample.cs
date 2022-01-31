using System;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using ScottBrady.IdentityModel.Crypto;
using ScottBrady.IdentityModel.Tokens;
using SensoryCloud.Src;
using SensoryCloud.Src.Services;

namespace Test.Examples
{
    public static class OAuthServiceExample
    {
        public static void RegisterDevice()
        {
            // Tenant ID granted by Sensory Inc.
            string sensoryTenantId = "f6580f3b-dcaf-465b-867e-59fbbb0ab3fc";
            string deviceId = "a-hardware-identifier-unique-to-your-device";

            // Configuration specific to your tenant
            Config config = new Config("https://your-inference-server.com", sensoryTenantId, deviceId);

            ISecureCredentialStore credentialStore = new SecureCredentialStoreExample();
            OauthService oauthService = new OauthService(config, credentialStore);

            // Generate cryptographically random credentials
            OauthClient oAuthClient = oauthService.GenerateCredentials();

            // Register credentials with Sensory Cloud
            var friendlyDeviceName = "Server 1";

            // OAuth registration can take one of two paths, the unsecure path that uses a shared secret between this device and your instance of Sensory Cloud
            // or asymmetric public / private keypair registration.

            // Path 1 --------
            // Unsecure authorization credential as configured on your instance of Sensory Cloud
            var unsecureSharedSecret = "password";
            oauthService.Register(friendlyDeviceName, unsecureSharedSecret);

            // Path 2 --------
            // Secure Public / private keypair registration using Portable.BouncyCastle and ScottBrady.IdentityModel
            var keyPairGenerator = new Ed25519KeyPairGenerator();
            keyPairGenerator.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));
            var keyPair = keyPairGenerator.GenerateKeyPair();

            var privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;

            // Public key is persisted in Sensory Cloud via the Management interface
            var publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

            var handler = new JsonWebTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "sensory-sdk",
                Audience = "sensory-cloud",
                Subject = new ClaimsIdentity(new[] { new Claim("clientId", oAuthClient.ClientId) }),

                // using JOSE algorithm "EdDSA"
                SigningCredentials = new SigningCredentials(
                new EdDsaSecurityKey(privateKey), ExtendedSecurityAlgorithms.EdDsa)
            });

            oauthService.Register(friendlyDeviceName, token);

            oauthService.RenewDeviceCredential(token);
        }
    }
}
