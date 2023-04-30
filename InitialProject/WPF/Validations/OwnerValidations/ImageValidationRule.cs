using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class ImageValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string imageUrl = value as string;
            if (string.IsNullOrEmpty(imageUrl))
                return new ValidationResult(false, "/Resources/Images/noImage.png");
            return ValidationResult.ValidResult;
        }
    }
}
