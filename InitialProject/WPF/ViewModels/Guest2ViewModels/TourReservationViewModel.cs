using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using InitialProject.Help;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourReservationViewModel:INotifyPropertyChanged
    {
        private int AvailableGuestsNumber;
        private int GuestsNumber;
        private int GuestId;
        private TourInstance CurrentTourInstance;
        private TourReservationRepository tourReservationRepository;
        private List<TourReservation> tourReservations;
        private TourInstanceRepository tourInstanceRepository;
        private ShowToursViewModel ShowTours;
        private bool withVoucher;
        private int capacityOfThisTour;
        public RelayCommand Guests_Increment_Command { get; set; }
        public RelayCommand Guests_Decrement_Command { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand ActivateCommand { get; set; }
        public ObservableCollection<TourInstance> TourInstances { get; set; }
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
        private Voucher selected;
        public Voucher Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                OnPropertyChanged("Selected");
            }

        }
        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                if (value != label)
                    label = value;
                OnPropertyChanged("Label");
            }

        }
        public Guest2 guest2 { get; set; }
        private VoucherService voucherService;
        private ObservableCollection<Voucher> vouchers;
        private string capacity;

        public string Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                    capacity = value;
                OnPropertyChanged("Capacity");
            }

        }
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
        public ICommand HelpCommandInViewModel { get; }
        private TourReservationFormView org;
        public TourReservationViewModel(TourInstance currentTourInstance, Guest2 guest2, ObservableCollection<TourInstance> TourInstance, TourInstanceRepository tourInstanceRepository,string label, TourReservationFormView org)
        {
            CurrentTourInstance = currentTourInstance;
            this.TourInstances = TourInstance;
            withVoucher = false;
            MakeCommands();
            Capacity = "";
            this.org = org;
            this.tourInstanceRepository = tourInstanceRepository;
            tourReservationRepository = new TourReservationRepository();
            tourReservations = tourReservationRepository.GetAll();
            this.guest2 = guest2;
            GuestId = guest2.Id;
            Label = "SHOWING AVAILABLE TOURS: ";
            capacityOfThisTour = currentTourInstance.Tour.MaxGuests;
            voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(guest2));
            VoucherValidity(Vouchers);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        
        private void VoucherValidity(ObservableCollection<Voucher> Vouchers)
        {
            foreach (Voucher voucher in Vouchers)
            {
                voucher.CreateDate = voucher.CreateDate.AddYears(1);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeCommands()
        {
            Guests_Increment_Command = new RelayCommand(Increment_Executed, CanExecute);
            Guests_Decrement_Command = new RelayCommand(Decrement_Executed, CanExecute);
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            CloseCommand = new RelayCommand(Cancel_Executed, CanExecute);
            ActivateCommand = new RelayCommand(Activate_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Increment_Executed(object sender)
        {
            int changedDaysNumber;
            if (Capacity == "")
                Capacity = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(Capacity) + 1;
                Capacity = changedDaysNumber.ToString();
            }
        }
        private void Decrement_Executed(object sender)
        {
            int changedDaysNumber;
            if (Capacity != "" && Convert.ToInt32(Capacity) > 1)
            {
                changedDaysNumber = Convert.ToInt32(Capacity) - 1;
                Capacity = changedDaysNumber.ToString();
            }
        }

    
    public void FindAvailableTours()
        {
            int suma = 0;
            ObservableCollection<TourInstance> storedTourInstances = new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            ShowTours.SetTours(storedTourInstances);
            TourInstances.Clear();
            foreach (TourInstance tourInstance in storedTourInstances)
            {
                foreach (TourReservation tourReservation in tourReservations)
                {
                    if (tourReservation.TourInstanceId == tourInstance.Id && tourInstance.Id != CurrentTourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country && tourInstance.Finished == false && tourInstance.Canceled==false)
                    {
                        suma += tourReservation.Capacity;
                    }
                }
                if (suma != capacityOfThisTour && tourInstance.Finished == false && CurrentTourInstance.Id != tourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country && tourInstance.Canceled == false)
                {
                    TourInstances.Add(tourInstance);
                }
            }
        }
        private int SumOfAllGuests()
        {
            int totalGuests = 0;
            GuestsNumber = Convert.ToInt32(Capacity);
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
        private void Confirm_Executed(object sender)
        {
            if (IsTourAvailable())
            {
                return;
            }
            else if (IsTourCompleted())
            {
                label = "SHOWING AVAILABLE TOURS: ";
                return;
            }
            TourReservation newTourReservation = new TourReservation(CurrentTourInstance.Id, GuestsNumber, GuestId, Convert.ToDouble(Age), Convert.ToInt32(Capacity), withVoucher);
            tourReservationRepository.Save(newTourReservation);
            MessageBox.Show("Tour is reserved.");
            Application.Current.Windows.OfType<TourReservationFormView>().FirstOrDefault().Close();
        }
        private Boolean IsTourAvailable()
        {
            int totalGuests = SumOfAllGuests();
            if (GuestsNumber > capacityOfThisTour && totalGuests != capacityOfThisTour)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Available number of places for guest is " + AvailableGuestsNumber + ".");
                return true;
            }
            return false;
        }
        private Boolean IsTourCompleted()
        {
            int totalGuests = SumOfAllGuests();
            if (totalGuests == capacityOfThisTour)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Tour is completed.");
                FindAvailableTours();
                Application.Current.Windows.OfType<TourReservationFormView>().FirstOrDefault().Close();
                return true;
            }
            return false;
        }
        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<TourReservationFormView>().FirstOrDefault().Close();
        }
        private void Activate_Executed(object sender)
        {
            voucherService.Update(Selected);
            this.withVoucher = true;
            Vouchers.Clear();
            foreach (Voucher voucher in voucherService.FindAllVouchers(guest2))
                Vouchers.Add(voucher);
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForReservation(str, org);
            }
        }
    }
}
