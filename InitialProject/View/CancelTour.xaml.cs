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
    /// Interaction logic for CancelTour.xaml
    /// </summary>
    public partial class CancelTour : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TourInstance> tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return tourInstances; }
            set
            {
                if (value != tourInstances)
                    tourInstances = value;
                OnPropertyChanged("TourInstances");
            }

        }

        private TourInstance selected;
        public TourInstance Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                OnPropertyChanged();
            }
        }
        private TourInstanceRepository tourInstanceRepository;
        private TourReservationRepository tourReservationRepository;
        private LocationRepository locationRepository;
        private TourRepository tourRepository;
        private User tourInstanceGuide;
        private VoucherRepository voucherRepository;
        private GuideRepository guideRepository;

        public CancelTour(User guide)
        {
            InitializeComponent();
            DataContext = this;
            tourInstanceRepository= new TourInstanceRepository();
            tourReservationRepository=new TourReservationRepository();
            locationRepository=new LocationRepository();
            tourRepository=new TourRepository();
            tourReservationRepository=new TourReservationRepository();
            voucherRepository=new VoucherRepository();
            guideRepository =new GuideRepository();
            tourInstances =new ObservableCollection<TourInstance>(tourInstanceRepository.GetInstancesLaterThan48hFromNow());
            SetLocationToTour();
            SetTourToTourInstance();
            tourInstanceGuide = guide;

        }
        private void SetLocationToTour()
        {
            List<Location> locations = locationRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();
            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }

        private void SetTourToTourInstance()
        {
            List<TourInstance> tourInstances = tourInstanceRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }
        private void Cancel_Click (object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            foreach (TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id == currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            currentTourInstance.Finished = true;
            TourInstances.Remove(currentTourInstance);
            SendVoucher(currentTourInstance.Id);

        }

        private void SendVoucher(int tourInstanceId)
        {
            foreach(TourReservation reservation in tourReservationRepository.GetReservationsForTourInstance(tourInstanceId))
            {
                Voucher voucher=new Voucher();
                voucher.Used = false;
                voucher.GuestId = reservation.GuestId;
                voucher.Guide = guideRepository.GetByUsername(tourInstanceGuide.Username);
                voucher.CreateDate = DateTime.Now;
                Voucher savedVoucher= voucherRepository.Save(voucher);

            }
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
