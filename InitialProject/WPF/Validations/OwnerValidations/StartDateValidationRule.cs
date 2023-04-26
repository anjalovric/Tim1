using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class StartDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime startDate = (DateTime)value;

            if (startDate > DateTime.Now)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Renovation can't be scheduled for today");
            }
        }
    }

    
}
