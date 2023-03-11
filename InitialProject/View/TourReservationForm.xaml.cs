using System;
using System.Collections.Generic;
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
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class TourReservationForm : Window, INotifyPropertyChanged
    {
        private const string FilePath = "../../../Resources/Data/tourReservations.csv";
        private int CurrentGuestsNumber;
        private int GuestsNumber;
        private Tour CurrentTour;
        private TourInstance _tourInstance;
        private TourReservationRepository _tourReservationRepository;
        private List<TourReservation> _tourReservations;
        private readonly Serializer<TourReservation> _serializerTourReservations;
        public TourReservationForm(Tour currentTour,TourInstance tourInstance)
        {
            InitializeComponent();
            DataContext = this;
            _tourInstance = tourInstance;
            _serializerTourReservations = new Serializer<TourReservation>();
            CurrentTour = currentTour;
            _tourReservations = _serializerTourReservations.FromCSV(FilePath);
            _tourReservationRepository = new TourReservationRepository();
            GetCurrentGuestsNumber();   
        }
        public int GetCurrentGuestsNumber()
        {
            foreach (TourReservation tourReservation in _tourReservations)
            {
                if (tourReservation.TourId == CurrentTour.Id && _tourInstance.Id == tourReservation.TourInstanceId)
                {
                    CurrentGuestsNumber = tourReservation.CurrentGuestsNumber;
                }
                else
                {
                    CurrentGuestsNumber = _tourInstance.Tour.MaxGuests;
                }
            }
            if (_tourReservations.Count == 0)
            {
                CurrentGuestsNumber = _tourInstance.Tour.MaxGuests;
            }
            return CurrentGuestsNumber;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void incrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedGuestsNumber.ToString();
        }

        private void decrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedGuestsNumber.ToString();
            }
        }
        
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            GuestsNumber = CurrentGuestsNumber - Convert.ToInt32(capacityNumber.Text);
            if (CurrentGuestsNumber == 0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Tour is completed.");
                AvailableTours availableTours = new AvailableTours(_tourInstance,CurrentTour);
                availableTours.Show();
                this.Close();
                return;
            }
            if (GuestsNumber < 0 && CurrentGuestsNumber!=0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Available number of places for guest is "+CurrentGuestsNumber+".");
                return;
            }
            foreach(TourReservation tourReservation in _tourReservations)
            {
                if (CurrentTour.Id == tourReservation.TourId && _tourInstance.Id==tourReservation.TourInstanceId)
                {
                    _tourReservationRepository.Delete(tourReservation);
                }
            }
            TourReservation newTourReservation = new TourReservation(CurrentTour.Id,_tourInstance.Id,GuestsNumber,0);
            _tourReservationRepository.Save(newTourReservation);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
