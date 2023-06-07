using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.Model
{
    public class MostPopularLocation
    {
        public Location Location { get; set; }
        public bool IsBasedOnReservationNumber { get; set; }
        public bool IsBasedOnBusinest { get; set; }
        public bool IsMostPopular { get; set; }
        public MostPopularLocation() { }

        public MostPopularLocation(Location location, bool isBasedOnReservationNumber, bool isBasedOnBusinest, bool isMostPopular)
        {
            Location = location;
            IsBasedOnReservationNumber = isBasedOnReservationNumber;
            IsBasedOnBusinest = isBasedOnBusinest;
            IsMostPopular = isMostPopular;
        }
    }
}
