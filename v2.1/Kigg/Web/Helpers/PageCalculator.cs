namespace Kigg.Web
{
    using System;

    public static class PageCalculator
    {
        public static int TotalPage(int total, int rowPerPage)
        {
            if ((total == 0) || (rowPerPage == 0))
            {
                return 1;
            }

            if ((total % rowPerPage) == 0)
            {
                return total / rowPerPage;
            }

            double result = Convert.ToDouble(total / rowPerPage);

            result = Math.Ceiling(result);

            return Convert.ToInt32(result) + 1;
        }

        public static int StartIndex(int? page, int rowPerPage)
        {
            return (page.HasValue && (page.Value > 1)) ? ((page.Value - 1) * rowPerPage) : 0;
        }
    }
}