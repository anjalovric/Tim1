using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.APPLICATION.UseCases
{
    public class CommentReportService
    {
        private ICommentReportRepository commentReportRepository = Injector.CreateInstance<ICommentReportRepository>();
        public CommentReportService()
        {

        }

        public void Add(CommentReport report)
        {
            commentReportRepository.Add(report);
        }

        public bool IsAlreadyReported(Owner owner, ForumComment comment)
        {
            return commentReportRepository.IsAlreadyReported(owner, comment);
        }

        public int GetReportNumber(ForumComment comment)
        {
            return commentReportRepository.GetReportNumber(comment);
        }
    }
}
