using System;
using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class CompletedAccommodationReschedulingRequestService
    {
        private ICompletedAccommodationReschedulingRequestRepository completedRequestRepository = Injector.CreateInstance<ICompletedAccommodationReschedulingRequestRepository>();
        private List<CompletedAccommodationReschedulingRequest> completedRequests;
        private ReschedulingAccommodationRequestService requestService;
        public CompletedAccommodationReschedulingRequestService()
        {
            completedRequests = new List<CompletedAccommodationReschedulingRequest>(completedRequestRepository.GetAll());
            SetRequests();
            requestService = new ReschedulingAccommodationRequestService();
        }

        public List<CompletedAccommodationReschedulingRequest> GetAll()
        {
            return completedRequests;
        }

        public void DeclineRequest(ReschedulingAccommodationRequest requestToDecline)
        {
            requestToDecline = requestService.ChangeState(requestToDecline, State.Declined);
            CompletedAccommodationReschedulingRequest completedRequest = MakeCompletedRequest(requestToDecline);
            completedRequests.Add(completedRequest);
            completedRequestRepository.Add(completedRequest);
        }

        public void ApproveRequest(ReschedulingAccommodationRequest requestToApprove)
        {
            requestToApprove = requestService.ChangeState(requestToApprove, State.Approved);
            CompletedAccommodationReschedulingRequest completedRequest = MakeCompletedRequest(requestToApprove);
            completedRequests.Add(completedRequest);
            completedRequestRepository.Add(completedRequest);
        }

        private static CompletedAccommodationReschedulingRequest MakeCompletedRequest(ReschedulingAccommodationRequest requestToComplete)
        {
            CompletedAccommodationReschedulingRequest completedRequest = new CompletedAccommodationReschedulingRequest();
            completedRequest.Request = requestToComplete;
           
            return completedRequest;
        }

        private void SetRequests()
        {
            ReschedulingAccommodationRequestService requestService = new ReschedulingAccommodationRequestService();
            List<ReschedulingAccommodationRequest> allRequest = new List<ReschedulingAccommodationRequest>(requestService.GetAll());

            foreach(CompletedAccommodationReschedulingRequest completedRequest in completedRequests)
            {
                ReschedulingAccommodationRequest request = allRequest.Find(n => n.Id == completedRequest.Request.Id);
                if(request != null)
                {
                    completedRequest.Request = request;
                }
            }

        }

        public List<CompletedAccommodationReschedulingRequest> GetRequestsByGuest(Guest1 guest1)
        {
            List<CompletedAccommodationReschedulingRequest> storedRequests = new List<CompletedAccommodationReschedulingRequest>(completedRequestRepository.GetAll());
            List<CompletedAccommodationReschedulingRequest> filteredRequests = new List<CompletedAccommodationReschedulingRequest>();

            foreach (CompletedAccommodationReschedulingRequest completedRequest in storedRequests)
            {
                if (completedRequest.Request.Reservation.Guest.Id == guest1.Id)
                    filteredRequests.Add(completedRequest);
            }

            return filteredRequests;
        }
        
    }
}