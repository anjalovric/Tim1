﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourReservationService
    {
        private TourReservationRepository tourReservationrepository;
        public TourReservationService() 
        {
            tourReservationrepository= new TourReservationRepository();
        }
        public TourReservation Save(TourReservation tourReservation)
        {
            return tourReservationrepository.Save(tourReservation);
        }
        public void Delete(TourReservation tourReservation)
        {
            tourReservationrepository.Delete(tourReservation);
        }
        public TourReservation Update(TourReservation tourReservation, int guestsNumber,Boolean withVoucher)
        {
            return tourReservationrepository.Update(tourReservation, guestsNumber,withVoucher);
        }
        public void Add(TourReservation tourReservation)
        {
            tourReservationrepository.Add(tourReservation);
        }
        public List<TourReservation> GetAll()
        {
            return tourReservationrepository.GetAll();
        }

        public List<TourReservation> GetReservationsForTourInstance(int tourInstanceId)
        {
            List<TourReservation> list = new List<TourReservation>();
            foreach (TourReservation tour in tourReservationrepository.GetAll())
            {
                if (tour.TourInstanceId == tourInstanceId)
                    list.Add(tour);
            }
            return list;
        }

        public TourReservation GetTourReservationById(int tourReservationId)
        {
            return tourReservationrepository.GetById(tourReservationId);
        }
        public List<TourReservation> GetByInstanceId(int instanceId)
        {
            return tourReservationrepository.GetReservationsForTourInstance(instanceId);
        }
    }
}
