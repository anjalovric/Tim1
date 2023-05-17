using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class SuggestedLanguageService
    {
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public SuggestedLanguageService() 
        {
            ordinaryTourRequestsService=new OrdinaryTourRequestsService();
        }
        private List<OrdinaryTourRequests> GetRequestsFromLastYear()
        {
            DateTime today=DateTime.Now;
            string yearago=today.Month+"/"+today.Day+"/"+(today.Year-1)+" "+today.ToString().Split(" ")[1]+" "+today.ToString().Split(" ")[2];  
            DateTime yearAgo=Convert.ToDateTime(yearago);
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            if(ordinaryTourRequestsService.GetAll().Count > 0)
                foreach(OrdinaryTourRequests request in ordinaryTourRequestsService.GetAll())
                    if( request.CreateDate >= yearAgo)
                       if(request.CreateDate <= today)
                        ordinaryTourRequests.Add(request);
            return ordinaryTourRequests;
        }

        private List<string> GetRequestsLanguagesFromLastYear()
        {
            List<string> languages = new List<string>();
            if(GetRequestsFromLastYear().Count > 0)
            {
                languages.Add(GetRequestsFromLastYear()[0].Language);
                foreach (OrdinaryTourRequests request in GetRequestsFromLastYear())
                    if (!languages.Contains(request.Language))
                        languages.Add(request.Language);
            }
            return languages;
        }
        private int CountRequestForLanguage(string language)
        {
            int count = 0;
            foreach(OrdinaryTourRequests request in GetRequestsFromLastYear())
                if (request.Language == language)
                    count++;
            return count;
        }
        private Dictionary<string,int> SetRequestsNumberForLanguage()
        {
            Dictionary<string,int> languagesRequests= new Dictionary<string,int>();
            if(GetRequestsLanguagesFromLastYear().Count > 0)
                foreach (string language in GetRequestsLanguagesFromLastYear())
                    languagesRequests.Add(language, CountRequestForLanguage(language));
            return languagesRequests;
        }
        public string GetMostWantedLanguage()
        {
            string langugage = null;
            if (SetRequestsNumberForLanguage().Count > 0)
            {
                int maximum = 0;
                Dictionary<string,int>languages=SetRequestsNumberForLanguage();
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
