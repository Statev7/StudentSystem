namespace StudentSystem.Web.Infrastructure.Attributes
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;

	[AttributeUsage(AttributeTargets.Property)]
    public class IsBeforeUtcNowAttribute : ValidationAttribute
    {
		public override bool IsValid(object value)
		{
			DateTime valuesAsDateTime = (DateTime)value;

			return valuesAsDateTime >= DateTime.UtcNow;
        }

		public override string FormatErrorMessage(string name)
		{
			var dateAsString = DateTime.UtcNow.Date.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
			var errorMessage = $"The '{name}' cannot be earlier than {dateAsString}!";

			return errorMessage;
		}
	}
}
