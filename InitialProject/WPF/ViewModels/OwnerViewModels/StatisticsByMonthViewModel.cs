using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByMonthViewModel : INotifyPropertyChanged
    {
        public Accommodation Accommodation { get; set; }
        private string busiestMonth;
        private OwnerMonthStatisticsService monthStatisticsService;
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<OwnerOneMonthStatisticViewModel> StatisticsByMonth { get; set; }

        public int Year { get; set; }
        public StatisticsByMonthViewModel(Accommodation accommodation, int year)
        {
            Accommodation = accommodation;
            Year = year;
            monthStatisticsService = new OwnerMonthStatisticsService();
            BusiestMonth = monthStatisticsService.GetBusiestMonth(Accommodation, Year);
            StatisticsByMonth = new ObservableCollection<OwnerOneMonthStatisticViewModel>(monthStatisticsService.GetMonthStatistics(Accommodation, Year));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string BusiestMonth
        {
            get => monthStatisticsService.GetBusiestMonth(Accommodation, Year);
            set
            {
                if (!value.Equals(busiestMonth))
                {
                    busiestMonth = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
