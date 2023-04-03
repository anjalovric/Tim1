using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICheckPointRepository:IGenericRepository<TourInstance>
    {

        public CheckPoint Save(CheckPoint checkPoint);

        public CheckPoint Update(CheckPoint checkPoint);

        public CheckPoint Delete(CheckPoint checkPoint);
    }
}
