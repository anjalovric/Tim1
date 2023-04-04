using System.Collections.Generic;

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
