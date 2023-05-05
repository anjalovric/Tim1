using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.Guest2Validations
{
    public class StringToIntValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value as string;
            var isNumeric = int.TryParse(stringValue, out int n);
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "This field is required");
            }else if (!isNumeric)
            {
                return new ValidationResult(false, "This field must be integer number");
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
                return ValidationResult.ValidResult;
            }
        }
    }
}
