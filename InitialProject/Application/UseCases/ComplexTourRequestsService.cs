using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = InitialProject.Domain.Model.Type;

namespace InitialProject.APPLICATION.UseCases
{
    public class ComplexTourRequestsService
    {
        private IComplexTourRequestsRepository complexTourRequestsRepository;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public ComplexTourRequestsService()
        {
            complexTourRequestsRepository = Injector.CreateInstance<IComplexTourRequestsRepository>();
            ordinaryTourRequestsService= new OrdinaryTourRequestsService();
        }
        public ComplexTourRequests Save(ComplexTourRequests request)
        {
            return complexTourRequestsRepository.Save(request);
        }
        public List<ComplexTourRequests> GetAll()
        {
            return complexTourRequestsRepository.GetAll();
        }
        public List<ComplexTourRequests> GetByGuestId(int id)
        {
            return complexTourRequestsRepository.GetByGuestId(id);
        }
        public ComplexTourRequests Update(ComplexTourRequests request)
        {
            return complexTourRequestsRepository.Update(request);
        }
        
        public List<ComplexTourRequests> GetOnWaitingComplexRequests(Guide guide)
        {
            List<ComplexTourRequests> complexTourRequests = new List<ComplexTourRequests>();
            foreach(ComplexTourRequests complex in GetAll())
            {
                if(complex.Status==Type.ONWAITING && CheckGuideIncludeness(guide.Id,complex.Id))
                    complexTourRequests.Add(complex);
            }
            return complexTourRequests;
        }

        private bool CheckGuideIncludeness(int guideId, int complexId)
        {
            foreach (OrdinaryTourRequests ordinaryTourRequests in ordinaryTourRequestsService.GetByComplexId(complexId))
                if (ordinaryTourRequests.GuideId == guideId)
                    return false;
            return true;
        }
        
    }
}
