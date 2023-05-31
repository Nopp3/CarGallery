using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace CarGalleryAPI.Data
{
    public static class Hash
    {
        public static string Encrypt(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
