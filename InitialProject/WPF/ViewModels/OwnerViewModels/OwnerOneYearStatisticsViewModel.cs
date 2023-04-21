using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerOneYearStatisticsViewModel
    {
        public int Year { get; set; }
        public Accommodation Accommodation { get; set; }
        public int Reservations { get; set; }
        public int Cancellations { get; set; }
        public int Reschedulings { get; set; }
        public int RenovationSuggestions { get; set; }
        public OwnerOneYearStatisticsViewModel()
        {
            Accommodation = new Accommodation();
        }
    }
}
