using System.Security.Cryptography;

namespace SensoryCloud.Src.Services
{
    /// <summary>
    /// Handles cryptographic operations
    /// </summary>
    public static class CryptoService
    {
        private static readonly string AllowableCharacters = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Generates a cryptographically-random string
        /// </summary>
        /// <param name="length">length of the string</param>
        /// <returns>a cryptographically-random string of the specified length</returns>
        public static string GetSecureRandomString(int length)
        {
            // Generate random data
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);

            // Generate the output string
            var allowable = AllowableCharacters.ToCharArray();
            var l = allowable.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = allowable[rnd[i] % l];

            return new string(chars);
        }


    }
}
