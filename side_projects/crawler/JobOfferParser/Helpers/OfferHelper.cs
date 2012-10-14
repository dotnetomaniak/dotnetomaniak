using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JobOfferParser.Data;

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


        public static IEnumerable<Keyword> ScanTextForKeywords(string text, IEnumerable<Keyword> allKeywords)
        {
            return from keyword in allKeywords where text.ToLower().Contains(keyword.Name.ToLower()) select new Keyword()
                                                                                            {
                                                                                                Id = keyword.Id,
                                                                                                Name = keyword.Name
                                                                                            };
        }
    }
}