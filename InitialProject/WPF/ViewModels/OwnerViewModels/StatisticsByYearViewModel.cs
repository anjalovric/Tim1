using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class StatisticsByYearViewModel : INotifyPropertyChanged
    {
        public Accommodation Accommodation { get; set; }
        private int busiestYear;
        private OwnerYearStatisticsService yearStatisticsService;
        public ObservableCollection<OwnerOneYearStatisticsViewModel> StatisticsByYear { get; set; }
        public RelayCommand SelectYearCommand { get; set; }
        private OwnerOneYearStatisticsViewModel selectedYear;
        public StatisticsByYearViewModel(Accommodation accommodation)
        {
            Accommodation = accommodation;
            SelectedYear = new OwnerOneYearStatisticsViewModel();
            yearStatisticsService = new OwnerYearStatisticsService();
            StatisticsByYear = new ObservableCollection<OwnerOneYearStatisticsViewModel>(yearStatisticsService.GetStatisticsByYear(accommodation));
            SelectYearCommand = new RelayCommand(SelectYear_Executed, CanExecute);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int BusiestYear
        {
            get => yearStatisticsService.GetBusiestYear(Accommodation);
            set
            {
                if (value != busiestYear)
                {
                    busiestYear = value;
                    OnPropertyChanged();
                }
            }
        }

        public OwnerOneYearStatisticsViewModel SelectedYear
        {
            get => selectedYear;
            set
            {
                if (!value.Equals(selectedYear))
                {
                    selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void SelectYear_Executed(object sender)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = new StatisticsByMonthView(Accommodation, SelectedYear.Year);
        }
    }
}
