using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class DescriptionStringValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string stringValue = value as string;
                if (stringValue.Length == 0)
                {
                    return new ValidationResult(false, "This field can't be empty");
                }


                return new ValidationResult(true, "");
            }
            catch
            {
                return new ValidationResult(false, "Invalid");
            }
        }
    }
}
