using System;
using System.Security.Cryptography;
using System.Text;

namespace JobOfferParser.Helpers
{
    public class OfferHelper
    {
        public static string GenerateSha1(string text)
        {
            var bytes = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            Array.ForEach(bytes, b => sb.AppendFormat("{0:X2}", b));
            return sb.ToString();
        }
    }
}