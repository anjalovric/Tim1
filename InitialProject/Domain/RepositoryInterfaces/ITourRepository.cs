using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRepository:IGenericRepository<Tour>
    {

        public void Delete(Tour tour);

        public Tour Update(Tour tour);

        public Tour Save(Tour tour);

    }
}
