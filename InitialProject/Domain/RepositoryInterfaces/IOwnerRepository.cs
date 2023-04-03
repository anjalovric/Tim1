using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IOwnerRepository:IGenericRepository<Owner>
    {
        public Owner GetByUsername(string username);
    }
}
