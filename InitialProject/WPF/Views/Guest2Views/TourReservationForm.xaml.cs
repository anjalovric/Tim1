using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Cache;
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
using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using NPOI.SS.Formula.Functions;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class TourReservationForm : Window, INotifyPropertyChanged
    {
        private int AvailableGuestsNumber;
        private int GuestsNumber;
        private int GuestId;
        private TourInstance CurrentTourInstance;
        private TourReservationRepository tourReservationRepository;
        private List<TourReservation> tourReservations;
        private List<TourInstance> tourInstances;
        private TourInstanceRepository tourInstanceRepository;
        private ShowTours ShowTours;
        private bool withVoucher;
        private int capacityOfThisTour;
        //private ActivatingVoucher ActivatingVoucher;
        public ObservableCollection<TourInstance> TourInstances { get; set; }
        public Label Label { get; set; }
        private string age;
        public string Age
        {
            get => age;
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private Guest2 guest2;
        private VoucherService voucherService;
        private ObservableCollection<Voucher> vouchers;
        public ObservableCollection<Voucher> Vouchers
        {
            get { return vouchers; }
            set
            {
                if (value != vouchers)
                    vouchers = value;
                OnPropertyChanged("Vouchers");
            }
        }
        public TourReservationForm(TourInstance currentTourInstance,Guest2 guest2,ObservableCollection<TourInstance> TourInstance,TourInstanceRepository tourInstanceRepository,Label label)
        {
            InitializeComponent();
            DataContext = this;
            CurrentTourInstance = currentTourInstance;
            this.TourInstances = TourInstance;
            this.Label = label;
            withVoucher = false;
            //ActivatingVoucher = new ActivatingVoucher(guest2);
            this.tourInstanceRepository = tourInstanceRepository;
            tourInstances = tourInstanceRepository.GetAll();
            tourReservationRepository = new TourReservationRepository();
            tourReservations = tourReservationRepository.GetAll();
            ShowTours = new ShowTours(guest2);
            this.guest2 = guest2;
            GuestId = guest2.Id;
            capacityOfThisTour = currentTourInstance.Tour.MaxGuests;
            voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(guest2));
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
        public void FindAvailableTours()
        {
            int suma = 0;
            ObservableCollection<TourInstance> storedTourInstances= new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            ShowTours.SetTours(storedTourInstances);
            TourInstances.Clear();
            foreach (TourInstance tourInstance in storedTourInstances)
            {
                foreach (TourReservation tourReservation in tourReservations)
                {
                    if (tourReservation.TourInstanceId == tourInstance.Id && tourInstance.Id != CurrentTourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country && tourInstance.Finished==false)
                    {
                        suma += tourReservation.Capacity;
                    }
                }
                if (suma!= capacityOfThisTour && tourInstance.Finished==false && CurrentTourInstance.Id != tourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country)
                {
                    TourInstances.Add(tourInstance);
                }
            }
        }
        private int SumOfAllGuests()
        {
            int totalGuests = 0;
            GuestsNumber = Convert.ToInt32(capacityNumber.Text); 
            foreach (TourReservation tourReservation in tourReservations)
            {
                if (CurrentTourInstance.Id == tourReservation.TourInstanceId)
                {
                    GuestsNumber += tourReservation.Capacity;
                    totalGuests += tourReservation.Capacity;
                    AvailableGuestsNumber = capacityOfThisTour - totalGuests;
                }
                else
                {
                    AvailableGuestsNumber = capacityOfThisTour - totalGuests;
                }
            }
            return totalGuests;
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            int totalGuests=SumOfAllGuests();
            if (GuestsNumber > capacityOfThisTour && totalGuests != capacityOfThisTour) 
            {
                MessageBox.Show("There is no enough places for choosen number of people. Available number of places for guest is " + AvailableGuestsNumber + ".");
                return;
            } else if (totalGuests == capacityOfThisTour) 
            {
                MessageBox.Show("There is no enough places for choosen number of people. Tour is completed.");
                FindAvailableTours();
                Label.Content = "Showing available tours: ";
                this.Close();
                return;
            }
            TourReservation newTourReservation = new TourReservation(CurrentTourInstance.Id, GuestsNumber, GuestId, Convert.ToDouble(Age), Convert.ToInt32(capacityNumber.Text), withVoucher);
            tourReservationRepository.Save(newTourReservation);
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Activate(object sender, RoutedEventArgs e)
        {
            Voucher currentVoucher = (Voucher)ActivationVoucherDataGrid.CurrentItem;
            voucherService.Update(currentVoucher);
            this.withVoucher = true;
        }
    }
}
