using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.Guest2Validations
{
    public class StringTourNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "This field is required");
            }
        }
    }
}
