using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.APPLICATION.UseCases
{
    public class UserService
    {
        private IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();

        public UserService()
        {

        }
        public User GetByUsername(string username)
        {
            return userRepository.GetByUsername(username);
        }
    }
}
