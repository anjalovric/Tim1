using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Service;
using System.Configuration;

namespace InitialProject.APPLICATION.UseCases
{
    public class ForumCommentService
    {
        private IForumCommentRepository forumCommentRepository = Injector.CreateInstance<IForumCommentRepository>();
        private Guest1Service guest1Service;
        private OwnerService ownerService;
        private AccommodationReservationService accommodationReservationService;
        private CommentReportService commentReportService;
        public ForumCommentService()
        {
            guest1Service = new Guest1Service();
            ownerService = new OwnerService();
            accommodationReservationService = new AccommodationReservationService();
            commentReportService = new CommentReportService();
        }

        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }
        public void Add(ForumComment forumComment)
        {
            if(!forumComment.IsOwnerComment)
                forumComment.WasOnLocation = accommodationReservationService.WasGuestOnLocation(((Guest1)forumComment.User), forumComment.Forum.Location.Id, forumComment.CreatingDate);
            forumCommentRepository.Add(forumComment);
        }

        public ForumComment GetById(int id)
        {
            return forumCommentRepository.GetById(id);
        }
        private void SetUsers(List<ForumComment> storedForumComments)
        {
            foreach (ForumComment comment in storedForumComments)
            {
                
                Guest1 guest1 = guest1Service.GetByUsername(comment.User.Username);
                if (guest1 != null)
                {
                    comment.User = guest1;
                    comment.User.Role = Role.GUEST1;
                }
                else
                    comment.User = ownerService.GetByUsername(comment.User.Username);
                
                comment.IsOwnerComment = comment.User.Role == Role.OWNER;
            }
        }
        public List<ForumComment> GetAllByForumId(int id)
        {
            List<ForumComment> storedForumComments = new List<ForumComment>(forumCommentRepository.GetAllByForumId(id));
            SetUsers(storedForumComments);
            SetReportNumber(storedForumComments);

            return storedForumComments;
        }

        public List<ForumComment> GetAllByForumIdForOwner(Owner owner, int id)
        {
            List<ForumComment> storedForumComments = new List<ForumComment>(forumCommentRepository.GetAllByForumId(id));
            SetUsers(storedForumComments);
            SetReportNumber(storedForumComments);
            IsAlreadyReportedByThisOwner(owner, storedForumComments);

            return storedForumComments;
        }

        public int GetNumberOfGuestComments(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentRepository.GetAllByForumId(forum.Id);
            return forumComments.FindAll(n => n.User.Role == Role.GUEST1).Count();
        }

        public int GetNumberOfOwnerComments(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentRepository.GetAllByForumId(forum.Id);
            return forumComments.FindAll(n => n.User.Role == Role.OWNER).Count();
        }

        private void SetReportNumber(List<ForumComment> comments)
        {
            foreach(ForumComment comment in comments)
            {
                comment.ReportsNumber = commentReportService.GetReportNumber(comment);
            }
        }

        private void IsAlreadyReportedByThisOwner(Owner owner, List<ForumComment> comments)
        {
            foreach (ForumComment comment in comments)
            {
                comment.IsAlreadyReportedByThisOwner = commentReportService.IsAlreadyReported(owner,comment);
            }
        }

        public void Report(ForumComment comment, Owner owner)
        {
            CommentReport report = new CommentReport();
            report.Owner = owner;
            report.ForumComment = comment;
            commentReportService.Add(report);
        }
    }
}
