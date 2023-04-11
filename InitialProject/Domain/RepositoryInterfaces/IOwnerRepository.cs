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
        public void Save(Owner owner);
        public int NextId();
        public Owner GetById(int id);
    }
}
