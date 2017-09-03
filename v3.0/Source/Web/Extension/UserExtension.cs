namespace Kigg.Web
{
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    using DomainObjects;

    public static class UserExtension
    {
        public static string GravatarUrl(this IUser user, int size)
        {
            return GenerateGravatarUrl(((user == null) ? string.Empty : user.Email), size);
        }

        private static string GenerateGravatarUrl(string email, int size)
        {
            const string GravatarUrlFormat = "https://www.gravatar.com/avatar/{0}?r=G&s={1}&d=identicon";

            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(email))
            {
                byte[] hash;

                using (MD5 md5 = MD5.Create())
                {
                    byte[] data = Encoding.Default.GetBytes(email.ToLowerInvariant());

                    hash = md5.ComputeHash(data);
                }

                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
            }

            return GravatarUrlFormat.FormatWith(result.ToString(), size);
        }
    }
}