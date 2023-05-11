using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InitialProject.WPF.Views.Guest2Views;
using System.Windows;
using InitialProject.WPF.ViewModels.Guest2ViewModels;

namespace InitialProject.WPF.Validations.Guest2Validations
{
    public class EndDateValidation : ValidationRule
    {
        public DateTime SelectedStartDate { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime endDate = (DateTime)value;
            CreateOrdinaryTourRequestView view = Application.Current.Windows.OfType<CreateOrdinaryTourRequestView>().FirstOrDefault();
            CreateOrdinaryTourRequestViewModel viewModel = (CreateOrdinaryTourRequestViewModel)view.DataContext;
            SelectedStartDate = viewModel.StartDate;

            if (endDate.Date >= SelectedStartDate.Date)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid end date");
            }
        }
    }
}
