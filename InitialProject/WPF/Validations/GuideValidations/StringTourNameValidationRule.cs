using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class StringTourNameValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string stringValue = value as string;
                Regex r = new Regex(@"^[A-Za-z]+(\s[A-Za-z]+)*$");

                if (stringValue.Length == 0)
                {
                    return new ValidationResult(false, "This field can't be empty");
                }

                if (r.IsMatch(stringValue))
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Enter name");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
}
