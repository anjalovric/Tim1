using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumComments.csv";

        private readonly Serializer<ForumComment> _serializer;

        private List<ForumComment> _comments;

        public ForumCommentRepository()
        {
            _serializer = new Serializer<ForumComment>();
            _comments = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(FilePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public void Add(ForumComment forumComment)
        {
            forumComment.Id = NextId();
            _comments.Add(forumComment);
            _serializer.ToCSV(FilePath, _comments);
        }

        public List<ForumComment> GetAll()
        {
            return _comments;
        }


        public ForumComment GetById(int id)
        {
            return _comments.Find(n => n.Id == id);
        }

        public List<ForumComment> GetAllByForumId(int id)
        {
            return _comments.FindAll(n => n.Forum.Id == id);
        }

        public void Report(ForumComment comment)
        {
            ForumComment commentToReport = _comments.Find(n => n.Id == comment.Id);
            commentToReport.ReportsNumber = comment.ReportsNumber;
            _serializer.ToCSV(FilePath, _comments);
        }

    }

}