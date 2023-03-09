using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
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
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        public ObservableCollection<Tour> Tours
        {
            get { return tours; }
            set
            {
                if(value!=tours)
                    tours = value;
                OnPropertyChanged("Tours");
            }
        }
        private ObservableCollection<Tour> tours { get; set; }  
        public Tour _selected;
        private TourRepository _tourRepository;
        public Guest2Overview()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<Tour> listTours = _tourRepository.GetAll();
            Tours.Clear();
            foreach (Tour tour in listTours)
            {
                Tours.Add(tour);
            }
            foreach (Tour tour in listTours)
            {
                if (tour.Location.City != null)
                {
                    if (!tour.Location.City.ToLower().Contains(cityInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.Location.Country != null)
                {
                    if (!tour.Location.Country.ToLower().Contains(countryInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.Duration != null)
                {
                    if (durationInput.Text != "")
                    {
                        if (tour.Duration < Convert.ToDouble(durationInput.Text))
                        {
                            Tours.Remove(tour);
                        }
                    }
                }
                if (tour.Language != null)
                {
                    if (!tour.Language.ToLower().Contains(languageInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.MaxGuests != null)
                {
                    if (Convert.ToInt32(capacityNumber.Text) > tour.MaxGuests)
                    {
                        Tours.Remove(tour);
                    }
                    
                }
                


            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void incrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedCapacityNumber.ToString();
        }

        private void decrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedCapacityNumber.ToString();
            }
        }
    }
}
