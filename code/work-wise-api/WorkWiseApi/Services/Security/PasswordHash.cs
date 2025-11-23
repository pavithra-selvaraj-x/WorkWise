using System.Security.Cryptography;

namespace Services.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class PasswordHash
    {
        private const int SaltByteSize = 32;
        private const int HashByteSize = 32;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetSalt()
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] b_salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(b_salt);
            return Convert.ToBase64String(b_salt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static string GetPasswordHash(string password, int iterations)
        {
            string salt = GetSalt();

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] derived;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                iterations,
                HashAlgorithmName.SHA512))
            {
                derived = pbkdf2.GetBytes(HashByteSize);
            }

            return string.Format("{0}:{1}", Convert.ToBase64String(derived), Convert.ToBase64String(saltBytes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static bool VerifyPasswordHash(string password, string hash, int iterations)
        {
            string[] parts = hash.Split(new char[] { ':' });

            byte[] saltBytes = Convert.FromBase64String(parts[1]);
            byte[] derived;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                iterations,
                HashAlgorithmName.SHA512))
            {
                derived = pbkdf2.GetBytes(HashByteSize);
            }

            string new_hash = string.Format("{0}:{1}", Convert.ToBase64String(derived), Convert.ToBase64String(saltBytes));

            return hash == new_hash;
        }
    }
}