using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuest1Repository : IGenericRepository<Guest1>
    {
        public Guest1 GetByUsername(string userName);
    }
}
