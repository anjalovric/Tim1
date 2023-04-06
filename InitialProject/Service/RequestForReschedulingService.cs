using System.Collections.Generic;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;

namespace InitialProject.Service
{
    public class RequestForReschedulingService
    {
        private ChangeAccommodationReservationDateRequestService requestsService;
        private List<RequestForReshcedulingViewModel> requestForReshceduling;
        public RequestForReschedulingService()
        {
            requestsService = new ChangeAccommodationReservationDateRequestService();
            requestForReshceduling = new List<RequestForReshcedulingViewModel>();
        }

        public List<RequestForReshcedulingViewModel> GetPendingRequests()
        {
            List<ChangeAccommodationReservationDateRequest> pendingRequest = requestsService.GetPendingRequests();
            List<RequestForReshcedulingViewModel> requestsViewModel = new List<RequestForReshcedulingViewModel>();
            
            foreach(ChangeAccommodationReservationDateRequest request in pendingRequest)
            {
                RequestForReshcedulingViewModel requestForReshcedulingViewModel = new RequestForReshcedulingViewModel();
                requestForReshcedulingViewModel.Request = request;
                requestForReshcedulingViewModel.AccommodationReservation = request.Reservation;
                //Is Available
                requestsViewModel.Add(requestForReshcedulingViewModel);
            }

            return requestsViewModel;
        }
    }
}
