using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class RequestStatisticMonthlyLanguageViewModel:INotifyPropertyChanged
    {
        private string language;
        public string Language
        {
            get { return language; }
            set
            {
                if (value != language)
                    language = value;
                OnPropertyChanged();
            }
        }
        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                if (value != year)
                    year = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MonthRequestStatisticViewModel> Statistics { get; set; }
        public RequestStatisticMonthlyLanguageViewModel(string language,GuideOneYearRequestStatisticViewModel selectedYear) 
        {
            Language = language;
            Year=selectedYear.Year.ToString();
            TourRequestStatisticMonthlyLanguageService monthLanguageStatisticService = new TourRequestStatisticMonthlyLanguageService();
            Statistics = new ObservableCollection<MonthRequestStatisticViewModel>(monthLanguageStatisticService.GetMonthStatistic(Language, selectedYear.Year));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
