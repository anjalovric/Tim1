using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Domain.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ISuperGuestTitleRepository : IGenericRepository<SuperGuestTitle>
    {
        public void Add(SuperGuestTitle superGuestTitle);
        public void Delete(SuperGuestTitle title);
        public void Update(SuperGuestTitle title);
        public SuperGuestTitle GetByGuestId(int id);
    }
}
