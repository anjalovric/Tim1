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

        public List<RequestForReshcedulingViewModel> GetPendingRequests(Owner owner)
        {
            List<ReschedulingAccommodationRequest> pendingRequest = requestsService.GetPendingRequestsIfNotCancelled();
            List<RequestForReshcedulingViewModel> requestsViewModel = new List<RequestForReshcedulingViewModel>();
            
            foreach(ReschedulingAccommodationRequest request in pendingRequest)
            {
                if (request.Reservation.Accommodation.Owner.Id == owner.Id)
                {
                    AddRequestToList(owner, requestsViewModel, request);
                }
            }

            return requestsViewModel;
        }

        private static void AddRequestToList(Owner owner, List<RequestForReshcedulingViewModel> requestsViewModel, ReschedulingAccommodationRequest request)
        {
            RequestForReshcedulingViewModel requestForReshcedulingViewModel = new RequestForReshcedulingViewModel(owner);
            requestForReshcedulingViewModel.Request = request;
            requestForReshcedulingViewModel.AccommodationReservation = request.Reservation;
            requestForReshcedulingViewModel.SetAvailability();
            requestsViewModel.Add(requestForReshcedulingViewModel);
        }
    }
}
