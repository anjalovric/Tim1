using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using NPOI.HSSF.Record.PivotTable;

namespace InitialProject.WPF.Validations.OwnerValidations
{
    public class EndDateValidationRule : ValidationRule
    {
        public DateTime SelectedStartDate { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime endDate = (DateTime)value;
            ScheduleRenovationView view = (ScheduleRenovationView)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            ScheduleRenovationViewModel viewModel = (ScheduleRenovationViewModel)view.DataContext;
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
