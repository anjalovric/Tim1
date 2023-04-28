using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace InitialProject.WPF.Validations.Guest1Validations
{
    public class NumberOfGuestsAndDaysValidationRule : ValidationRule
    {
        public enum ValidationResultType
        {
            Valid,
            Invalid,
            ValidWithWarning
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string content = value as string;
            var regex = "^([1-9][0-9]*)$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (!match.Success && content != "")
                return new ValidationResult(false, "Enter an integer greater than 0.");
            else
                return ValidationResult.ValidResult;
        }
          
    }
}
