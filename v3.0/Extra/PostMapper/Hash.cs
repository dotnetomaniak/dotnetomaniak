using System;
using System.Security.Cryptography;
using System.Text;

namespace PostMaping
{
    public class Hash
    {
        public static string ComputeHash(string target)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }
    }
}