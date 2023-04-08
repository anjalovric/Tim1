﻿using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuideAndTourReviewsRepository:IGenericRepository<GuideAndTourReview>
    {
        public bool HasReview(TourInstance tourInstance);

        public void Save(GuideAndTourReview review);

        public List<GuideAndTourReview> GetReviewsByGuide(int guideId);
        public GuideAndTourReview Update(GuideAndTourReview review);
    }
}
