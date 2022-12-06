namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime TrimToSeconds(this DateTime value)
            => value - TimeSpan.FromTicks(value.Ticks % 10_000_000);
    }
}
