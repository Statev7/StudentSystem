namespace StudentSystem.Web.Infrastructure.Filters
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using static StudentSystem.Web.Common.NotificationsConstants;

    public class DateLessThanTodayAttribute : ValidationAttribute
    {
        private readonly bool canBeToday;

        public DateLessThanTodayAttribute(bool canBeToday = false)
        {
            this.canBeToday = canBeToday;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTimeValue = (DateTime)value;

            if (!this.canBeToday && startTimeValue.Date <= DateTime.UtcNow.Date)
            {
                var dateAsString = DateTime.UtcNow.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var errorMessage = string.Format(START_DATE_MESSAGE, dateAsString);

                return new ValidationResult(errorMessage);
            }

            if (this.canBeToday && startTimeValue.ToUniversalTime() < DateTime.UtcNow)
            {
                var dateAsString = DateTime.UtcNow.ToString("g", CultureInfo.InvariantCulture);
                var errorMessage = string.Format(START_DATE_MESSAGE, dateAsString);

                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
