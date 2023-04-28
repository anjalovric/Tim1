﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.Validations.GuideValidations
{
    public class DatesCountValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            if (Convert.ToInt32(value) >0)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "At least 1 date is required");
            }
        }

    }
}
