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

namespace InitialProject.WPF.ViewModels
{
    public class ReservationReschedulingViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RequestForReshcedulingViewModel> Requests { get; set; }
        private RequestForReschedulingService requestService;
        private RequestForReshcedulingViewModel selectedRequest;
        private CompletedAccommodationReschedulingRequestService completedRequestService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReservationReschedulingViewModel()
        {
            requestService= new RequestForReschedulingService();
            Requests = new ObservableCollection<RequestForReshcedulingViewModel>(requestService.GetPendingRequests());
            completedRequestService = new CompletedAccommodationReschedulingRequestService();
            SelectedRequest = new RequestForReshcedulingViewModel();
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
                if (!value.Equals(selectedRequest))
                {
                    selectedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public void DeclineRequest()
        {
            completedRequestService.DeclineRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
        }

        public void ApproveRequest()
        {

        }
    }
}
