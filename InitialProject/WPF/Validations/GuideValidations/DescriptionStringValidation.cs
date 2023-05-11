using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class DescriptionStringValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
           
                string stringValue = value as string;
                if (string.IsNullOrEmpty(stringValue))
                {
                var app = (App)Application.Current;
                string Message = "";
                if (app.Lang.Equals("en-US"))
                    Message = "This field is required";
                else
                    Message = "Ovo polje je obavezno";
                return new ValidationResult(false, Message);
            }

                else
                    return new ValidationResult(true, "");
        }
    }
}
