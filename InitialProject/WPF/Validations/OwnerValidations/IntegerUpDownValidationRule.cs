using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class IntegerUpDownValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string integerValue = value as string;
            if (string.IsNullOrEmpty(integerValue))
                return new ValidationResult(false, "This field is required");
            try
            {
                if (Convert.ToInt32(integerValue) <= 0)
                    return new ValidationResult(false, "This field should be at least 1");
                else
                    return ValidationResult.ValidResult;
            }
            catch
            {
                return new ValidationResult(false, "Please enter number");
            }
        }
    }
}
