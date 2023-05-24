using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;

namespace InitialProject.APPLICATION.UseCases
{
    public class ForumCommentService
    {
        private IForumCommentRepository forumCommentRepository = Injector.CreateInstance<IForumCommentRepository>();
        public ForumCommentService()
        {
        }

        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }
        public void Add(ForumComment forumComment)
        {
            forumCommentRepository.Add(forumComment);
        }

        public ForumComment GetById(int id)
        {
            return forumCommentRepository.GetById(id);
        }
    }
}
