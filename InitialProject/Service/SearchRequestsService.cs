using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
namespace InitialProject.Service
{
    public class SearchRequestsService
    {      
        private OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
        private List<OrdinaryTourRequests> requests;
        public SearchRequestsService()
        { 
            requests= new List<OrdinaryTourRequests>();
            requests = ordinaryTourRequestsService.GetOnWaitingRequests();
        }
        public List<OrdinaryTourRequests> GetByCountry(List<OrdinaryTourRequests> appropriateRequests, string country)
        {
            foreach (OrdinaryTourRequests request in requests)
                if (request.Location.Country == country)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetByCity(List<OrdinaryTourRequests> appropriateRequests, string city)
        { 
            foreach (OrdinaryTourRequests request in requests)
                if (request.Location.City == city)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetByLanguage(List<OrdinaryTourRequests> appropriateRequests, string language)
        {
            foreach (OrdinaryTourRequests request in requests)
                if (request.Language == language)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetByCapacity(List<OrdinaryTourRequests> appropriateRequests, int capacity)
        {
            foreach (OrdinaryTourRequests request in requests)
                if (request.MaxGuests == capacity)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetByStart(List<OrdinaryTourRequests> appropriateRequests, DateTime Start)
        {
            foreach (OrdinaryTourRequests request in requests)
                if (request.StartDate >= Start)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetByEnd(List<OrdinaryTourRequests> appropriateRequests, DateTime End)
        {
            foreach (OrdinaryTourRequests request in requests)
                if (request.EndDate <= End)
                    appropriateRequests.Add(request);
            return Inspect(appropriateRequests);
        }
        private List<OrdinaryTourRequests> Inspect(List<OrdinaryTourRequests> requests)
        {
            for (int i = 0; i <= requests.Count - 2; i++)
            {
                for(int j= i+1; j <= requests.Count - 1; j++)
                {
                    if (requests[i].Id == requests[j].Id)
                    {
                        for(int k=j;k < requests.Count - 1; k++)
                        {
                            requests[k] = requests[i+1];
                        }
                        requests.Remove(requests[requests.Count-1]);
                    }
                        
                }
            }
            return requests;
        }
    }
}
