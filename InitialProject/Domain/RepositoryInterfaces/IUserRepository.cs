using InitialProject.Model;
using InitialProject.Serializer;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IUserRepository:IGenericRepository<User>
    {
        public User GetByUsername(string username);
    }
}
