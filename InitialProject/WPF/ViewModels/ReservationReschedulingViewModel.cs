using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels
{
    public class ReservationReschedulingViewModel : INotifyPropertyChanged
    {
        private Owner profileOwner;
        public ObservableCollection<RequestForReshcedulingViewModel> Requests { get; set; }
        private RequestForReschedulingService requestService;
        private ReschedulingAccommodationRequestService reschedulingRequestService;
        private CompletedAccommodationReschedulingRequestService completedReschedulingRequestService;
        private RequestForReshcedulingViewModel selectedRequest;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReservationReschedulingViewModel(Owner owner)
        {
            profileOwner = owner;
            requestService = new RequestForReschedulingService();
            reschedulingRequestService = new ReschedulingAccommodationRequestService();
            Requests = new ObservableCollection<RequestForReshcedulingViewModel>(requestService.GetPendingRequests(owner));
            InitializeSelectedRequest();
            completedReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
        }

        private void InitializeSelectedRequest()
        {
            SelectedRequest = new RequestForReshcedulingViewModel(profileOwner);
            if(Requests.Count>0)
                SelectedRequest = Requests[0];
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestForReshcedulingViewModel SelectedRequest
        {
            get => selectedRequest;
            set
            {
                if (value !=selectedRequest)
                {
                    selectedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public void DeclineRequest()
        {
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Declined);
            completedReschedulingRequestService.DeclineRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
        }

        public void ApproveRequest()
        {
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Approved);
            completedReschedulingRequestService.ApproveRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
        }

    }
}
