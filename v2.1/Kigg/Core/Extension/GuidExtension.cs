namespace Kigg
{
    using System;

    public static class GuidExtension
    {
        public static string Shrink(this Guid target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
    }
}