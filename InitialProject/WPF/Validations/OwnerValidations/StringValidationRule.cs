using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class StringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value as string;
            if (string.IsNullOrEmpty(text))
            {
                return new ValidationResult(false, "This field is required");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
