using System.Collections.Generic;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class CompletedAccommodationReschedulingRequestService
    {
        private CompletedAccommodationReschedulingRequestRepository completedRequestRepository;
        private List<CompletedAccommodationReschedulingRequest> completedRequests;
        private ReschedulingAccommodationRequestService requestService;
        public CompletedAccommodationReschedulingRequestService()
        {
            completedRequestRepository = new CompletedAccommodationReschedulingRequestRepository();
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

    }
}
