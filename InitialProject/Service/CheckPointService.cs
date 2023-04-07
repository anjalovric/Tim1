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
    public class CheckPointService
    {
        private ICheckPointRepository repository=Injector.CreateInstance<ICheckPointRepository>();
        public CheckPointService() 
        { 
            repository = new CheckPointRepository();
        }

        public List<CheckPoint> GetAll()
        {
            return repository.GetAll();
        }

        public CheckPoint Save(CheckPoint checkPoint)
        {
            return repository.Save(checkPoint);
        }

        public void Delete(CheckPoint checkPoint)
        {
            repository.Delete(checkPoint);
        }
        public CheckPoint Update(CheckPoint checkPoint)
        {
            return repository.Update(checkPoint);
        }
        public CheckPoint GetById(int id)
        {
            return repository.GetById(id);
        }
        public List<CheckPoint> GetByInstance(int tourId)
        {
            return repository.GetByInstance(tourId);
        }
    }
}
