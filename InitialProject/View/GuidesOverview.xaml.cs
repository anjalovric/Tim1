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
    /// Interaction logic for GuidesOverview.xaml
    /// </summary>
    public partial class GuidesOverview : Window,INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public Tour _selected;
        private TourRepository _tourRepository;
        public Tour Selected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                    _selected = value;
                OnPropertyChanged();
            }
        }
        public GuidesOverview()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_tourRepository.GetByStart(DateTime.Now));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TourForm tourForm = new TourForm();
            tourForm.Show();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {
            TourCheckPoints checkPoints = new TourCheckPoints(Selected);    
            checkPoints.Show();
            
        }
    }
}
