namespace Kigg
{
    using System;
    using System.Globalization;

    public static class Constants
    {
        public static readonly DateTime ProductionDate = new DateTime(2009, 1, 11);

        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }
    }
}