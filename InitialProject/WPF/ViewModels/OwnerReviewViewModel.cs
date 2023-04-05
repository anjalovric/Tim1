using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerReviewViewModel
    {
        public OwnerReview OwnerReview { get; set; }
        public OwnerReviewViewModel(OwnerReview ownerReview)
        {
            OwnerReview = ownerReview;
        }
    }
}
