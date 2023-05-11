using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.Validations.Guest2Validations
{
    public class StartDateValidation : ValidationRule
    {
        public DateTime SelectedEndDate { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime startDate = (DateTime)value;
            CreateOrdinaryTourRequestView view = Application.Current.Windows.OfType<CreateOrdinaryTourRequestView>().FirstOrDefault();
            CreateOrdinaryTourRequestViewModel viewModel = (CreateOrdinaryTourRequestViewModel)view.DataContext;
            SelectedEndDate = viewModel.EndDate;
            if (startDate.Day<=DateTime.Now.Day+2 && startDate.Month==DateTime.Now.Month && startDate.Year==DateTime.Now.Year)
            {
                return new ValidationResult(false, "Invalid date");
            }
            else if(startDate <= SelectedEndDate)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid date");
            }
        }
    }
}
