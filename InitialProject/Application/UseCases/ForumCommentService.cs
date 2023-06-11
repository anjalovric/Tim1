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
        private AccommodationService accommodationService;
        private CommentReportService commentReportService;
        private OwnerNotificationsService notificationsService;
        public ForumCommentService()
        {
            guest1Service = new Guest1Service();
            ownerService = new OwnerService();
            accommodationReservationService = new AccommodationReservationService();
            notificationsService = new OwnerNotificationsService();
            accommodationService = new AccommodationService();
            commentReportService = new CommentReportService();
        }

        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }
        public void Add(ForumComment forumComment)
        {
            if(!forumComment.IsOwnerComment)
                forumComment.WasOnLocation = accommodationReservationService.IsGuestCommentSpecial(((Guest1)forumComment.User), forumComment.Forum.Location.Id, forumComment.CreatingDate);
            else
            {
                forumComment.WasOnLocation = true;
            }
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

        public int GetNumberOfGuestOnLocationComments(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentRepository.GetAllByForumId(forum.Id);
            SetUsers(forumComments);
            int total = 0;
            foreach (ForumComment comment in forumComments)
            {
                if (comment.User.Role == Role.GUEST1 && accommodationReservationService.HadReservationOnLocation(comment.User.Username, forum.Location))
                {
                    total++;
                }
            }
            return total;
        }
        public int GetNumberOfGuestComments(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentRepository.GetAllByForumId(forum.Id);
            SetUsers(forumComments);
            int total = 0;
            foreach(ForumComment comment in forumComments)
            {
                if(comment.User.Role==Role.GUEST1)
                {
                    total++;
                }
            }
            return total;
        }

        
        public int GetNumberOfOwnerComments(Forum forum)
        {
            List<ForumComment> forumComments = forumCommentRepository.GetAllByForumId(forum.Id);
            SetUsers(forumComments);
            int total = 0;
            foreach (ForumComment comment in forumComments)
            {
                if (comment.User.Role == Role.OWNER)
                {
                    total++;
                }
            }
            return total;
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
            comment.ReportsNumber++;
            report.Owner = owner;
            report.ForumComment = comment;
            commentReportService.Add(report);
            forumCommentRepository.Report(comment);
            notificationsService.Add(OwnerNotificationType.COMMENT_REPORTED, owner);
        }
    }
}
