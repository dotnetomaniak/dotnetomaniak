namespace Kigg
{
    using System;

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}