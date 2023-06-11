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
            return _serializer.FromCSV(FilePath);
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
            List<CommentReport> reports = new List<CommentReport>(_serializer.FromCSV(FilePath));
            reports.Add(report);
            _serializer.ToCSV(FilePath, reports);
        }

        public void Delete(CommentReport report)
        {
            List<CommentReport> reports = new List<CommentReport>(_serializer.FromCSV(FilePath));
            CommentReport reportForDeleting = reports.Find(n => n.Owner.Id == report.Owner.Id && n.ForumComment.Id == report.ForumComment.Id);
            if(reportForDeleting != null)
            {
                reports.Remove(reportForDeleting);
            }
            _serializer.ToCSV(FilePath, reports);
        }
        public CommentReport GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsAlreadyReported(Owner owner, ForumComment comment)
        {
            return _serializer.FromCSV(FilePath).Find(n => n.Owner.Id == owner.Id && n.ForumComment.Id == comment.Id) != null;
        }

        public int GetReportNumber(ForumComment comment)
        {
            return _serializer.FromCSV(FilePath).FindAll(n => n.ForumComment.Id == comment.Id).Count();
        }
    }
}
