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
        private ChangeAccommodationReservationDateRequestService requestService;
        public CompletedAccommodationReschedulingRequestService()
        {
            completedRequestRepository = new CompletedAccommodationReschedulingRequestRepository();
            completedRequests = new List<CompletedAccommodationReschedulingRequest>(completedRequestRepository.GetAll());
            SetRequests();
            requestService = new ChangeAccommodationReservationDateRequestService();
        }

        public List<CompletedAccommodationReschedulingRequest> GetAll()
        {
            return completedRequests;
        }

        public void DeclineRequest(ChangeAccommodationReservationDateRequest requestToDecline)
        {
            CompletedAccommodationReschedulingRequest completedRequest = MakeCompletedRequest(requestToDecline);
            completedRequests.Add(completedRequest);
            completedRequestRepository.Add(completedRequest);
        }

        private static CompletedAccommodationReschedulingRequest MakeCompletedRequest(ChangeAccommodationReservationDateRequest requestToDecline)
        {
            CompletedAccommodationReschedulingRequest completedRequest = new CompletedAccommodationReschedulingRequest();
            completedRequest.Request = requestToDecline;
            completedRequest.State = ChangeAccommodationReservationDateRequest.State.Declined;
            completedRequest.OwnersExplanation = "bbbbbbbbbb";
            return completedRequest;
        }

        private void SetRequests()
        {
            ChangeAccommodationReservationDateRequestService requestService = new ChangeAccommodationReservationDateRequestService();
            List<ChangeAccommodationReservationDateRequest> allRequest = new List<ChangeAccommodationReservationDateRequest>();

            foreach(CompletedAccommodationReschedulingRequest completedRequest in completedRequests)
            {
                ChangeAccommodationReservationDateRequest request = allRequest.Find(n => n.Id == completedRequest.Request.Id);
                if(request != null)
                {
                    completedRequest.Request = request;
                }
            }

        }

    }
}
