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
        private ReschedulingAccommodationRequestService reschedulingRequestService;
        private RequestForReshcedulingViewModel selectedRequest;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReservationReschedulingViewModel()
        {
            requestService= new RequestForReschedulingService();
            reschedulingRequestService= new ReschedulingAccommodationRequestService();
            Requests = new ObservableCollection<RequestForReshcedulingViewModel>(requestService.GetPendingRequests());
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
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Declined);
            Requests.Remove(SelectedRequest);
        }

        public void ApproveRequest()
        {
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Approved);
            Requests.Remove(SelectedRequest);
        }
    }
}
