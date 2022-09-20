namespace StudentSystem.Web.Infrastructure.Helpers
{
    using System;

    public static class MyValidator
    {
        public static bool CompareDates(DateTime firstDate, DateTime secondDate)
            => secondDate > firstDate;
    }
}
