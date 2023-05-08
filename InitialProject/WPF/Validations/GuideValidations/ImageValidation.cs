using NPOI.XSSF.Streaming.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class ImageValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            BitmapImage image = value as BitmapImage;

            if (image!=null)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                var app = (App)Application.Current;
                string Message = "";
                if (app.Lang.Equals("en-US"))
                    Message = "At least 1 image is required";
                else
                    Message = "Obavezno uneti bar 1 sliku";
                return new ValidationResult(false, Message);
            }
        }


    }
}
