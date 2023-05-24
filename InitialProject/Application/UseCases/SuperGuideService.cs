﻿using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Service;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace InitialProject.Application.UseCases
{
    public class SuperGuideService
    {
        private ISuperGuideRepository superGuideRepository = Injector.CreateInstance<ISuperGuideRepository>();
        private TourInstanceService tourInstanceService;
        private GuideAndTourReviewService guideAndTourReviewService;
        private TourService tourService;
        public SuperGuideService() 
        {
            tourInstanceService=new TourInstanceService();
            tourService=new TourService();
            guideAndTourReviewService=new GuideAndTourReviewService();
        }

        public SuperGuide Save(SuperGuide superGuide)
        {
            return superGuideRepository.Save(superGuide);
        }

        public void Delete(SuperGuide superGuide)
        {
            superGuideRepository.Delete(superGuide);
        }

        public List<SuperGuide> GetAll()
        {
            return superGuideRepository.GetAll();
        }
        public List<string> getLanguagesForGuideTours(Guide guide)
        {
            List<string> languages=new List<string>();
            if (tourInstanceService.GetAll()!= null)
            {
                foreach (TourInstance tourInstance in tourInstanceService.GetAll())
                {
                    tourService.SetTour(tourInstance);
                    if (!languages.Contains(tourInstance.Tour.Language) && tourInstance.Finished && tourInstance.Guide == guide)
                        languages.Add(tourInstance.Tour.Language);
                }
            }
            return languages;
        }

        public bool getGradesForTours(string language, Guide guide)
        {
            int tourCount = 0;
            double averageGrade = 0;
            List<double> grades = new List<double>();
            DateTime today = DateTime.Now;
            string yearago = today.Month + "/" + today.Day + "/" + (today.Year - 1) + " " + today.ToString().Split(" ")[1] + " " + today.ToString().Split(" ")[2];
            foreach (TourInstance tourInstance in tourInstanceService.GetAll())
            {
                tourService.SetTour(tourInstance);
                if (tourInstance.Tour.Language.Equals(language) && tourInstance.Finished && tourInstance.Guide == guide && tourInstance.StartDate >= Convert.ToDateTime(yearago) && tourInstance.StartDate<=DateTime.Now)
                {
                    tourCount++;
                    GuideAndTourReview guideAndTourReview = guideAndTourReviewService.GetReviewsByGuide(guide.Id).Find(x=>x.TourInstance.Id== tourInstance.Id);
                    if (guideAndTourReview != null)
                        averageGrade += (guideAndTourReview.Language + guideAndTourReview.InterestingFacts + guideAndTourReview.Knowledge) / 3;
                }
            }

            return CheckIsSuperGuide(tourCount, averageGrade);
        }    
        
        public bool CheckIsSuperGuide(int tourCount, double averageGrade)
        {
            return (!(tourCount < 20 || averageGrade / tourCount < 4.0));
        }

        public void CheckSuperGuideStatus(Guide guide)
        {
            List<string> languages = getLanguagesForGuideTours(guide);
            if (languages.Count > 0)
            {
                foreach (String language in languages)
                {
                    if (getGradesForTours(language, guide))
                    {
                        SuperGuide superGuide = new SuperGuide();
                        superGuide.Language = language;
                        superGuide.guideId = guide.Id;
                        if (GetAll().Find(x => x.Language == language) == null)
                            Save(superGuide);
                    }
                }
            }
        }

       public void CheckLostOfTitle(Guide guide)
       {
          List<SuperGuide> superGuides = new List<SuperGuide>();
            foreach(SuperGuide superGuide  in GetAll())
            {
                if(getGradesForTours(superGuide.Language, guide)==false && superGuide.guideId==guide.Id)
                    Delete(superGuide);
                break;
            }
       }

        public List<SuperGuide> UpdateSuperGuideStatus(Guide guide)
        {
            CheckSuperGuideStatus(guide);
            CheckLostOfTitle(guide);
            return GetAll();
        }
    }
    
}
