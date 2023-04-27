using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class LanguageSuggestService
    {
        private List<OrdinaryTourRequests> GetRequestsInLanstYear()
        {
            DateTime today=DateTime.Now;
            string yearago=today.Month+"/"+today.Day+"/"+(today.Year-1)+" "+today.ToString().Split(" ")[1]+" "+today.ToString().Split(" ")[2];  
            DateTime yearAgo=Convert.ToDateTime(yearago);
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            if(ordinaryTourRequestsService.GetAll().Count > 0)
                foreach(OrdinaryTourRequests request in ordinaryTourRequestsService.GetAll())
                    if( request.StartDate >= yearAgo)
                       if(request.StartDate <= today)
                        ordinaryTourRequests.Add(request);
            return ordinaryTourRequests;
        }

        private List<string> GetLanguages()
        {
            List<string> languages = new List<string>();
            if(GetRequestsInLanstYear().Count > 0)
            {
                languages.Add(GetRequestsInLanstYear()[0].Language);
                foreach (OrdinaryTourRequests request in GetRequestsInLanstYear())
                    if (!languages.Contains(request.Language))
                        languages.Add(request.Language);
            }
            return languages;
        }
        private int CountRequestForLanguage(string language)
        {
            int count = 0;
            foreach(OrdinaryTourRequests request in GetRequestsInLanstYear())
                if (request.Language == language)
                    count++;
            return count;
        }
        private Dictionary<string,int> GetRequestNumber()
        {
            Dictionary<string,int> languagesRequests= new Dictionary<string,int>();
            if(GetLanguages().Count > 0)
                foreach (string language in GetLanguages())
                    languagesRequests.Add(language, CountRequestForLanguage(language));
            return languagesRequests;
        }
        public string GetMostWantedLanguage()
        {
            string langugage = null;
            if (GetRequestNumber().Count > 0)
            {
                int maximum = 0;
                Dictionary<string,int>languages=GetRequestNumber();
                for (int index = 0; index < languages.Count; index++)
                {
                    var item = languages.ElementAt(index);
                    if (item.Value>maximum)
                    {
                        maximum = item.Value;
                        langugage= item.Key;
                    }
                }

            }
            return langugage;
        }
    }
}
