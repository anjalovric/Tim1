using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.Model
{
    public class LeastPopularLocation
    {
        public Accommodation Accommodation { get; set; }
        public bool IsBasedOnReservationNumber { get; set; }
        public bool IsBasedOnBusinest { get; set; }
        public bool IsLeastPopular { get; set; }
        public LeastPopularLocation() { }

        public LeastPopularLocation(Accommodation accommodation, bool isBasedOnReservationNumber, bool isBasedOnBusinest, bool isLeastPopular)
        {
            Accommodation = accommodation;
            IsBasedOnReservationNumber = isBasedOnReservationNumber;
            IsBasedOnBusinest = isBasedOnBusinest;
            IsLeastPopular = isLeastPopular;
        }
    }
}
