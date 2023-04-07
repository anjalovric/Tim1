using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static CompletedAccommodationReschedulingRequest MakeCompletedRequest(ReschedulingAccommodationRequest requestToDecline)
        {
            CompletedAccommodationReschedulingRequest completedRequest = new CompletedAccommodationReschedulingRequest();
            completedRequest.Request = requestToDecline;
           
            return completedRequest;
        }

        private void SetRequests()
        {
            ReschedulingAccommodationRequestService requestService = new ReschedulingAccommodationRequestService();
            List<ReschedulingAccommodationRequest> allRequest = new List<ReschedulingAccommodationRequest>();

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
