using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuest2Repository:IGenericRepository<Guest2>
    {
        public Guest2 GetByUsername(string username);
    }
}
