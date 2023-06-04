using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICommentReportRepository : IGenericRepository<CommentReport>
    {
        public void Delete(CommentReport report);
        public int GetReportNumber(ForumComment comment);
        public bool IsAlreadyReported(Owner owner, ForumComment comment);
        public void Add(CommentReport report);
        public void Update(CommentReport owner);
    }
}
