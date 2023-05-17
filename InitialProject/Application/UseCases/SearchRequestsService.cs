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
            requests= new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnWaitingRequests());
        }
        public List<OrdinaryTourRequests> GetRequestsByCountry(List<OrdinaryTourRequests> appropriateRequests, string country,bool entered)
        {
            List<OrdinaryTourRequests> addaptedList = new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered==false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.Location.Country == country && !appropriateRequests.Contains(request))
                        appropriateRequests.Add(request);
            }
            else
            {
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(request.Location.Country != country)
                        temporaryList.Add(request);
                appropriateRequests.Clear();
                foreach (OrdinaryTourRequests request in addaptedList)
                    if (!temporaryList.Contains(request))
                        appropriateRequests.Add(request);

            }
            return InspectRequests(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetRequestsByCity(List<OrdinaryTourRequests> appropriateRequests, string city,bool entered)
        {
            List<OrdinaryTourRequests> addaptedList = new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered==false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.Location.City == city && !appropriateRequests.Contains(request))
                        if (!appropriateRequests.Contains((OrdinaryTourRequests)request))
                            appropriateRequests.Add(request);
            }
            else
            {
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(request.Location.City != city)
                        temporaryList.Add(request) ;
                appropriateRequests.Clear();
                foreach (OrdinaryTourRequests request in addaptedList)
                    if (!temporaryList.Contains(request))
                        appropriateRequests.Add(request);
            }
            return InspectRequests(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetRequestsByLanguage(List<OrdinaryTourRequests> appropriateRequests, string language, bool entered)
        {
            List<OrdinaryTourRequests> addaptedList=new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered == false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.Language == language && !appropriateRequests.Contains(request))
                            appropriateRequests.Add(request);        
            }
            else
            {
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(request.Language != language)
                       temporaryList.Add(request);
                appropriateRequests.Clear();
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(!temporaryList.Contains(request))    
                        appropriateRequests.Add(request);
            }
            return InspectRequests(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetRequestsByCapacity(List<OrdinaryTourRequests> appropriateRequests, int capacity, bool entered)
        {
            List<OrdinaryTourRequests> addaptedList = new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered == false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.MaxGuests == capacity && !appropriateRequests.Contains(request))
                        appropriateRequests.Add(request);
            }else
            {
                foreach (OrdinaryTourRequests request in addaptedList )
                    if (request.MaxGuests != capacity)
                        temporaryList.Add(request);
                appropriateRequests.Clear();
                foreach (OrdinaryTourRequests request in addaptedList)
                    if (!temporaryList.Contains(request))
                        appropriateRequests.Add(request);
            }
            return InspectRequests(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetRequestsByStart(List<OrdinaryTourRequests> appropriateRequests, DateTime Start, bool entered)
        {
            List<OrdinaryTourRequests> addaptedList = new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered == false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.StartDate >= Start && !appropriateRequests.Contains(request))
                            appropriateRequests.Add(request);
            }
            else
            {
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(!(request.StartDate >=Start))
                        temporaryList.Add(request);
                appropriateRequests.Clear();
                foreach (OrdinaryTourRequests request in addaptedList)
                    if (!temporaryList.Contains(request))
                        appropriateRequests.Add(request);

            }
            return InspectRequests(appropriateRequests);
        }
        public List<OrdinaryTourRequests> GetRequestsByEnd(List<OrdinaryTourRequests> appropriateRequests, DateTime End, bool entered)
        {
         
            List<OrdinaryTourRequests> addaptedList = new List<OrdinaryTourRequests>(appropriateRequests);
            List<OrdinaryTourRequests> temporaryList = new List<OrdinaryTourRequests>();
            if (appropriateRequests.Count == 0 && entered == false)
            {
                foreach (OrdinaryTourRequests request in requests)
                    if (request.EndDate <= End && !appropriateRequests.Contains(request))
                            appropriateRequests.Add(request);
            }
            else
            {
                foreach(OrdinaryTourRequests request in addaptedList)
                    if(!(request.EndDate <= End))
                        temporaryList.Add(request);
                appropriateRequests.Clear();
                foreach (OrdinaryTourRequests request in addaptedList)
                    if (!temporaryList.Contains(request))
                        appropriateRequests.Add(request);

            }
            return InspectRequests(appropriateRequests);
        }
        private List<OrdinaryTourRequests> InspectRequests(List<OrdinaryTourRequests> requests)
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
