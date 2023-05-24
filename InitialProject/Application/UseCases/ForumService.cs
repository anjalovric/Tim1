using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Model;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Domain.RepositoryInterfaces;

namespace InitialProject.Service
{
    public class ForumService
    {
        private IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
        public ForumService()
        {
        }

        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }
        public void Add(Forum forum)
        {
            forumRepository.Add(forum);
        }

       public Forum GetById(int id)
       {
           return forumRepository.GetById(id);
       }
    }
}
