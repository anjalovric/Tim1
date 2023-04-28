using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class StringToIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
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
            else if (Convert.ToInt32(stringValue) == 0)
            {
                return new ValidationResult(false, "This field is required");
            }
            else if (Convert.ToInt32(stringValue) < 1)
            { 
                return new ValidationResult(false, "Number must be positive and whole");
            }
                else
            {
                return new ValidationResult(false, "This field is required");
            }
        }
    }
}
