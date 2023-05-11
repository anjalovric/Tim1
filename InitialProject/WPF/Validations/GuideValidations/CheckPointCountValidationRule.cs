using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class CheckPointCountValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            if (Convert.ToInt32(value)>1)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                var app = (App)Application.Current;
                string Message = "";
                if (app.Lang.Equals("en-US"))
                    Message = "At least 2 points are required";
                else
                    Message = "Obavezno uneti bar 2 tačke";
                return new ValidationResult(false, Message);
            }
        }
    }

}
