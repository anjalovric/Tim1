﻿using InitialProject.Domain.Model;
using InitialProject.Model;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class RequestStatisticsService
    {
        public double AverageNumberOfPeopleInAcceptedRequests(List<OrdinaryTourRequests> ordinaryTours, Guest2 Guest2)
        {
            int counter = 0;
            double averageNumberOfPeople = 0;
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
        public double ProcentOfInvalidRequest(List<OrdinaryTourRequests> ordinaryTours, Guest2 Guest2)
        {
            double invalidRequest = 0;
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
        public double ProcentOfAcceptedRequest(List<OrdinaryTourRequests> ordinaryTours, Guest2 Guest2)
        {
            double acceptedRequest = 0;
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
    }
}
