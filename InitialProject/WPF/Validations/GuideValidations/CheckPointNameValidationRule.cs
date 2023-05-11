using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class CheckPointNameValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                var app = (App)Application.Current;
                string Message = "";
                if (app.Lang.Equals("en-US"))
                    Message = "Enter point name";
                else
                    Message = "Unesi naziv tačke";
                return new ValidationResult(false, Message);
            }
        }
    }
}
