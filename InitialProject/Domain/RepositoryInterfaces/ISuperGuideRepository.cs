using InitialProject.Domain.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    interface ISuperGuideRepository:IGenericRepository<SuperGuide>
    {
        public SuperGuide Save(SuperGuide superGuide);

        public void Delete(SuperGuide superGuide);
    }
}
