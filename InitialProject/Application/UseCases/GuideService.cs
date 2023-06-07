using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class GuideService
    {
        private IGuideRepository guideRepository=Injector.CreateInstance<IGuideRepository>();
        public GuideService() 
        { }
        public Guide GetByUsername(string username)
        {
            Guide foundedGuide = null;
            foreach (Guide guide in guideRepository.GetAll())
            {
                if (guide.Username == username)
                    foundedGuide = guide;
            }
            return foundedGuide;
        }

        public Guide Update(Guide guide)
        {
            return guideRepository.Update(guide);
        }

        public void SetGuide(TourInstance tourInstance)
        {
            foreach(Guide guide in guideRepository.GetAll())
            {
                if(tourInstance.Guide.Id == guide.Id)
                {
                    tourInstance.Guide = guide;
                }
            }
        }
    }
}
