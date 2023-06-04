using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository : IGenericRepository<ForumComment>
    {
        public void Add(ForumComment forumComment);
        public List<ForumComment> GetAllByForumId(int id);
        public void Report(ForumComment comment);
    }
}
