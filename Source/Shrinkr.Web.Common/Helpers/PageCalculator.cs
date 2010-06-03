namespace Shrinkr.Web
{
    using System;
    using System.Diagnostics;

    public static class PageCalculator
    {
        [DebuggerStepThrough]
        public static int TotalPage(int total, int itemPerPage)
        {
            if ((total == 0) || (itemPerPage == 0))
            {
                return 1;
            }

            if ((total % itemPerPage) == 0)
            {
                return total / itemPerPage;
            }

            double result = Convert.ToDouble(total / itemPerPage);

            result = Math.Ceiling(result);

            return Convert.ToInt32(result) + 1;
        }

        [DebuggerStepThrough]
        public static int StartIndex(int? page, int itemPerPage)
        {
            return (page.HasValue && (page.Value > 1)) ? ((page.Value - 1) * itemPerPage) : 0;
        }
    }
}