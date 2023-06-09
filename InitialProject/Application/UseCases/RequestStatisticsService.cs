using InitialProject.Domain.Model;
using InitialProject.Model;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class RequestStatisticsService
    {
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public RequestStatisticsService()
        {
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
        }
        public double AverageNumberOfPeopleInAcceptedRequests(Guest2 Guest2)
        {
            int counter = 0;
            double averageNumberOfPeople = 0;
            List<OrdinaryTourRequests> ordinaryTours=new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnlyOrdinaryRequestsByGuestId(Guest2.Id));
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == Status.ACCEPTED && request.GuestId == Guest2.Id)
                {
                    averageNumberOfPeople += request.MaxGuests;
                    counter++;
                }
            }
            if (averageNumberOfPeople == 0)
                return averageNumberOfPeople;
            return (averageNumberOfPeople /= counter).Round(2);
        }
        public double ProcentOfInvalidRequest(Guest2 Guest2)
        {
            double invalidRequest = 0;
            List<OrdinaryTourRequests> ordinaryTours = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnlyOrdinaryRequestsByGuestId(Guest2.Id));
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == Status.INVALID && request.GuestId == Guest2.Id)
                {
                    invalidRequest++;
                }
            }
            if (invalidRequest == 0)
                return 0;
            invalidRequest /= ordinaryTours.Count();
            return (invalidRequest *= 100).Round(2);
        }
        public double ProcentOfAcceptedRequest(Guest2 Guest2)
        {
            double acceptedRequest = 0;
            List<OrdinaryTourRequests> ordinaryTours = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnlyOrdinaryRequestsByGuestId(Guest2.Id));
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == Status.ACCEPTED && request.GuestId == Guest2.Id)
                {
                    acceptedRequest++;
                }
            }
            if (acceptedRequest == 0)
                return acceptedRequest;
            acceptedRequest /= ordinaryTours.Count();
            return (acceptedRequest *= 100).Round(2);
        }
    }
}
