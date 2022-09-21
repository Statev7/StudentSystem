namespace StudentSystem.Web.Infrastructure.Helpers
{
    using System;

    using StudentSystem.Web.Common;

    public static class MyValidator
    {
        public static (bool isValid, string errorMessage) ValidateDates(
            DateTime firstDate, 
            DateTime secondDate, 
            string nameOfFirst, 
            string nameOfSecond,
            bool canBeInSameDay = false)
        {
            if (canBeInSameDay)
            {
                if (firstDate.Date.Day < DateTime.UtcNow.Date.Day) return GetResult();
            }
            else
            {
                if (firstDate.Date.Day <= DateTime.UtcNow.Date.Day) return GetResult(1);
            }

            if(firstDate > secondDate)
            {
                var errorMessage = 
                    string.Format(NotificationsConstants.SECOND_DATE_CANNOT_BE_EARLIER_MESSAGE, nameOfFirst, nameOfSecond);

                return (false, errorMessage);
            }

            return (true, String.Empty);
        }

        private static (bool isValid, string errorMessage) GetResult(int daysToAdd = 0)
        {
            var dateAsString = DateTime.UtcNow.AddDays(daysToAdd).ToString("dd/MM/yyyy");
            var errorMessage = string.Format(NotificationsConstants.START_DATE_MESSAGE, dateAsString);

            return (false, errorMessage);
        }
    }
}
