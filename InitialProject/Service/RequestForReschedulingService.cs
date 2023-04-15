using System.Collections.Generic;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using Org.BouncyCastle.Asn1.Ocsp;

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

        public List<ReschedulingAccommodationRequest> GetApprovedRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> approvedRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requestsService.GetAll())
            {
                if (request.state == State.Approved && request.Reservation.Guest.Id == guest1.Id)
                {
                    approvedRequests.Add(request);
                }
            }
            approvedRequests.Reverse();
            return approvedRequests;
        }
        public List<ReschedulingAccommodationRequest> GetDeclinedRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> declinedRequests = new List<ReschedulingAccommodationRequest>();
            foreach (ReschedulingAccommodationRequest request in requestsService.GetAll())
            {
                if (request.state == State.Declined && request.Reservation.Guest.Id == guest1.Id)
                {
                    declinedRequests.Add(request);
                }
            }
            declinedRequests.Reverse();
            return declinedRequests;
        }

        public List<ReschedulingAccommodationRequest> GetPendingRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requestsService.GetAll())
            {
                if (request.state == State.Pending && request.Reservation.Guest.Id == guest1.Id)
                {
                    pendingRequests.Add(request);
                }
            }
            pendingRequests.Reverse();
            return pendingRequests;
        }


    }
}
