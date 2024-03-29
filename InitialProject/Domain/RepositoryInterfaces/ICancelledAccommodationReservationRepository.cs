﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICancelledAccommodationReservationRepository : IGenericRepository<AccommodationReservation>
    {
        public void Add(AccommodationReservation reservation);
        public int GetMaxId();
        public void Delete(AccommodationReservation reservation);
    }
}
