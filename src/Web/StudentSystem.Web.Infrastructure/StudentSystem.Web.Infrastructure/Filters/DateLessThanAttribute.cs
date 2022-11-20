namespace StudentSystem.Web.Infrastructure.Filters
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using static StudentSystem.Web.Common.NotificationsConstants;

    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            this.comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTimeValue = (DateTime)value;

            var endTimeAsProperty = validationContext.ObjectType.GetProperty(this.comparisonProperty);
            if (endTimeAsProperty == null)
            {
                throw new ArgumentNullException("Property with this name not found");
            }

            var endTimeValue = (DateTime)endTimeAsProperty.GetValue(validationContext.ObjectInstance);

            if (startTimeValue > endTimeValue)
            {
                var endTimeDisplayAttribute = endTimeAsProperty
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .SingleOrDefault();

                var endTimeDisplayName = endTimeDisplayAttribute == null 
                        ? endTimeAsProperty.Name 
                        : endTimeDisplayAttribute.Name;

                 var errorMessage = 
                    string.Format(THE_START_TIME_CANNOT_BE_LATER_THAN_THE_END_TIME, 
                    validationContext.DisplayName, 
                    endTimeDisplayName.ToLower());

                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
