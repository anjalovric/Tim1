using System.Collections.Generic;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;

namespace InitialProject.Service
{
    public class RequestForReschedulingService
    {
        private ReschedulingAccommodationRequestService requestsService;
        public RequestForReschedulingService()
        {
            requestsService = new ReschedulingAccommodationRequestService();
        }

        public List<RequestForReshcedulingViewModel> GetPendingRequests()
        {
            List<ReschedulingAccommodationRequest> pendingRequest = requestsService.GetPendingRequests();
            List<RequestForReshcedulingViewModel> requestsViewModel = new List<RequestForReshcedulingViewModel>();
            
            foreach(ReschedulingAccommodationRequest request in pendingRequest)
            {
                RequestForReshcedulingViewModel requestForReshcedulingViewModel = new RequestForReshcedulingViewModel();
                requestForReshcedulingViewModel.Request = request;
                requestForReshcedulingViewModel.AccommodationReservation = request.Reservation;
                requestForReshcedulingViewModel.SetAvailability();
                requestsViewModel.Add(requestForReshcedulingViewModel);
            }

            return requestsViewModel;
        }
    }
}
