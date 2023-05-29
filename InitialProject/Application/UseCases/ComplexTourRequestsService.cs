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
        public ComplexTourRequestsService()
        {
            complexTourRequestsRepository = Injector.CreateInstance<IComplexTourRequestsRepository>();
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
        

        
    }
}
