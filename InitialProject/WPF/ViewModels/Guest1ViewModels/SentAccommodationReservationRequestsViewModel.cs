using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class SentAccommodationReservationRequestsViewModel : INotifyPropertyChanged
    {
        private RequestForReschedulingService requestForReschedulingService;
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
        public SentAccommodationReservationRequestsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            requestForReschedulingService = new RequestForReschedulingService();
            ApprovedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetApprovedRequests(guest1));
            PendingRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetPendingRequests(guest1));
            DeclinedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetDeclinedRequests(guest1));

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
