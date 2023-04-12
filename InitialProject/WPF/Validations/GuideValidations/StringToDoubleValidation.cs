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
            try
            {
                string stringValue = value as string;
                Regex r = new Regex(@"^([0]//.[1-9]+)|([1-9][0-9]*)|([1-9]//.[0-9])$");

                if (stringValue.Length == 0)
                {
                    return new ValidationResult(false, "Can't be empty");
                }

                if (r.IsMatch(stringValue))
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Enter capacity!");
            }
            catch
            {
                return new ValidationResult(false, "Enter valid number");
            }
        }
    }
}
