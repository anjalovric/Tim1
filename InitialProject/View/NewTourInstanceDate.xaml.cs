using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for NewTourInstanceDate.xaml
    /// </summary>
    public partial class NewTourInstanceDate : Window, INotifyPropertyChanged
    {
        private readonly TourInstanceRepository _tourInstanceRepository;
        private string _startHour;
        public string InstanceStartHour 
        {
            get => _startHour;
            set
            {
                if (value != _startHour)
                {
                    _startHour = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _start;
        private TourInstance newInstance;
        private ObservableCollection<TourInstance> tourInstances;

        public DateTime InstanceStartDate {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private Tour _currentTour;
        public NewTourInstanceDate(Tour tour, ObservableCollection<TourInstance> instances)
        {
            InitializeComponent();
            DataContext = this;
           // _tourInstanceRepository = new TourInstanceRepository();
            _currentTour = tour;
            newInstance = new TourInstance();
            tourInstances = instances;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //TourInstance newTourInstance = new TourInstance(_currentTour, _start, InstanceStartHour);
            //TourInstance saved = _tourInstanceRepository.Save(newTourInstance);
            newInstance.StartDate = InstanceStartDate;
            newInstance.StartClock = InstanceStartHour;
            tourInstances.Add(newInstance);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
