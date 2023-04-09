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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for SentAccommodationReservationRequests.xaml
    /// </summary>
    public partial class SentAccommodationReservationRequests : Page, INotifyPropertyChanged
    {
        private ReschedulingAccommodationRequestService reschedulingAccommodationRequestService;
        private ObservableCollection<ReschedulingAccommodationRequest> approvedRequests;
        private Guest1 guest1;
        public ObservableCollection<ReschedulingAccommodationRequest> ApprovedRequests
        {
            get { return approvedRequests; }
            set
            {
                if (value != approvedRequests)
                    approvedRequests = value;
                OnPropertyChanged("ApprovedRequests");
            }

        }
        private ObservableCollection<ReschedulingAccommodationRequest> pendingRequests;
        public ObservableCollection<ReschedulingAccommodationRequest> PendingRequests
        {
            get { return pendingRequests; }
            set
            {
                if (value != pendingRequests)
                    pendingRequests = value;
                OnPropertyChanged("PendingRequests");
            }

        }
        private ObservableCollection<ReschedulingAccommodationRequest> declinedRequests;
        public ObservableCollection<ReschedulingAccommodationRequest> DeclinedRequests
        {
            get { return declinedRequests; }
            set
            {
                if (value != declinedRequests)
                    declinedRequests = value;
                OnPropertyChanged("DeclinedRequests");
            }

        }

        public SentAccommodationReservationRequests(Guest1 guest1)
        {
            InitializeComponent();
            this.DataContext = this;
            this.guest1 = guest1;
            reschedulingAccommodationRequestService = new ReschedulingAccommodationRequestService();
            ApprovedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(reschedulingAccommodationRequestService.GetApprovedRequests(guest1));
            PendingRequests = new ObservableCollection<ReschedulingAccommodationRequest>(reschedulingAccommodationRequestService.GetPendingRequests(guest1));
            DeclinedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(reschedulingAccommodationRequestService.GetDeclinedRequests(guest1));


        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
