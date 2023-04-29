using NPOI.XSSF.Streaming.Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return new ValidationResult(false, "At least 1 image is required");
            }
        }


    }
}
