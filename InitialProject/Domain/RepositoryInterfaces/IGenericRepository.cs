using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        List<T> GetAll();
        int NextId();
        T GetById(int id);

    }
}
