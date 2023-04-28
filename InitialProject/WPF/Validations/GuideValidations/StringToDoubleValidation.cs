using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class StringToDoubleValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                return ValidationResult.ValidResult;
            }
            else if(string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "This field is required");
            }
            else if (Convert.ToDouble(stringValue) == 0)
            {
                return new ValidationResult(false, "This field is required");
            }
            else if (Convert.ToDouble(stringValue) < 1)
            {
                return new ValidationResult(false, "Number must be positive and real");
            }
            else
            {
                return new ValidationResult(false, "This field is required");
            }
        }
    }
}
