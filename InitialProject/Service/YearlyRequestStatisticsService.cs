using InitialProject.Domain.Model;
using InitialProject.Model;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class YearlyRequestStatisticsService
    {
        public double AverageNumberOfPeopleInAcceptedRequests(int year, Guest2 Guest2)
        {
            int counter = 0;
            double averageNumberOfPeople = 0;
            List<OrdinaryTourRequests> ordinaryTours=GetRequestsForChosenYear(year, Guest2);
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Accepted" && request.GuestId == Guest2.Id)
                {
                    averageNumberOfPeople += request.MaxGuests;
                    counter++;
                }
            }
            if (averageNumberOfPeople == 0)
                return averageNumberOfPeople;
            return (averageNumberOfPeople /= counter).Round(2);
        }
        public double ProcentOfInvalidRequest(int year, Guest2 Guest2)
        {
            double invalidRequest = 0;
            List<OrdinaryTourRequests> ordinaryTours = GetRequestsForChosenYear(year, Guest2);
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Invalid" && request.GuestId == Guest2.Id)
                {
                    invalidRequest++;
                }
            }
            if (invalidRequest == 0)
                return 0;
            invalidRequest /= ordinaryTours.Count();
            return invalidRequest *= 100;
        }
        public double ProcentOfAcceptedRequest(int year, Guest2 Guest2)
        {
            double acceptedRequest = 0;
            List<OrdinaryTourRequests> ordinaryTours = GetRequestsForChosenYear(year, Guest2);
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Accepted" && request.GuestId == Guest2.Id)
                {
                    acceptedRequest++;
                }
            }
            if (acceptedRequest == 0)
                return acceptedRequest;
            acceptedRequest /= ordinaryTours.Count();
            return acceptedRequest *= 100;
        }
        public List<OrdinaryTourRequests> GetRequestsForChosenYear(int year, Guest2 guest)
        {
            List<OrdinaryTourRequests> ordinaryTours = new List<OrdinaryTourRequests>();
            OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetByGuestId(guest.Id))
            {
                if (request.StartDate.Year == year)
                    ordinaryTours.Add(request);
            }
            return ordinaryTours;
        }
    }
}
