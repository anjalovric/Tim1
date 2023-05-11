using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class ComboBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string comboBoxValue = value as string;
            if(string.IsNullOrEmpty(comboBoxValue))
            {
                return new ValidationResult(false, "Please select city");
            }
            return ValidationResult.ValidResult;
        }
    }
}
