using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.Guest2Validations
{
    public class StringToDoubleValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string stringValue = value as string;
            var isNumeric = double.TryParse(stringValue, out double n);
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "This field is required");
            }else if (!isNumeric)
            {
                return new ValidationResult(false, "This field must be number");
            }
            else if (Convert.ToDouble(stringValue) < 0)
            {
                return new ValidationResult(false, "Number must be positive and real");
            }
            else if (Convert.ToDouble(stringValue) == 0)
            {
                return new ValidationResult(false, "This field is requried");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
