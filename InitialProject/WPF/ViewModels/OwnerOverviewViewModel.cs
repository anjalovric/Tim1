using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;
using MathNet.Numerics.Distributions;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerOverviewViewModel
    {
        public Owner ProfileOwner { get; set; }
        public OwnerOverviewViewModel(User user)
        {
            MakeProfileOwner(user);
        }

        private void MakeProfileOwner(User user)
        {
            ProfileOwner = new Owner();
            OwnerService ownerService = new OwnerService();
            ProfileOwner = ownerService.GetByUsername(user.Username);
        }
    }
}
