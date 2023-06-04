using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class CommentReportRepository : ICommentReportRepository
    {
        private const string FilePath = "../../../Resources/Data/commentReports.csv";

        private readonly Serializer<CommentReport> _serializer;

        private List<CommentReport> reports;

        public CommentReportRepository()
        {
            _serializer = new Serializer<CommentReport>();
            reports = _serializer.FromCSV(FilePath);
        }

        public List<CommentReport> GetAll()
        {
            return reports;
        }

        public int NextId()
        {
            throw new NotImplementedException();
        }

        public void Update(CommentReport owner)
        {
            throw new NotImplementedException();
        }
        public void Add(CommentReport report)
        {
            reports.Add(report);
            _serializer.ToCSV(FilePath, reports);
        }

        public void Delete(CommentReport report)
        {
            CommentReport reportForDeleting = reports.Find(n => n.Owner.Id == report.Owner.Id && n.ForumComment.Id == report.ForumComment.Id);
            if(reportForDeleting != null)
            {
                reports.Remove(reportForDeleting);
            }
        }
        public CommentReport GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsAlreadyReported(Owner owner, ForumComment comment)
        {
            return reports.Find(n => n.Owner.Id == owner.Id && n.ForumComment.Id == comment.Id) != null;
        }

        public int GetReportNumber(ForumComment comment)
        {
            return reports.FindAll(n => n.ForumComment.Id == comment.Id).Count();
        }
    }
}
