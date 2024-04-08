using System.ComponentModel.DataAnnotations;
namespace GniApi.Dtos.RequestDto
{
    public class LoanRequestActionDto
    {
        public required long requestId {  get; set; }


        [ValueIn("Offer-approved", "Offer-rejected")]
        public required string status { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValueInAttribute : ValidationAttribute
    {
        private readonly string[] _allowedValues;

        public ValueInAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && Array.IndexOf(_allowedValues, value.ToString()) == -1)
            {
                return new ValidationResult($"The field {validationContext.DisplayName} must be one of the following values: {string.Join(", ", _allowedValues)}.");
            }

            return ValidationResult.Success;
        }
    }
}
