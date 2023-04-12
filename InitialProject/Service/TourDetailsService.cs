﻿using InitialProject.Model;
using System.Collections.Generic;
namespace InitialProject.Service
{
    public class TourDetailsService
    {
        public TourReservationService reservationService;
        public AlertGuest2Service alertGuest2Service;
        public CheckPointService checkPointService;
        public Guest2Service guest2Service;
        public TourDetailsService() 
        { 
            reservationService = new TourReservationService();
            alertGuest2Service = new AlertGuest2Service();
            checkPointService = new CheckPointService();
            guest2Service = new Guest2Service();
        }
        public int CountGuest(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                if (IsOnTour(reservation.GuestId, selectedId))
                    counter += reservation.Capacity;
            
            return counter;
        }
        public List<string> GetPointsForGuest(int guest2Id,TourInstance instance)
        {
            List<string> pointsName = new List<string>();
            foreach (CheckPoint checkPoint in checkPointService.GetByInstance(instance.Tour.Id)) 
            {
                foreach (AlertGuest2 alert in alertGuest2Service.GetByInstanceIdAndGuestId(instance.Id, guest2Id))
                {
                    if (alert.Availability && alert.CheckPointId==checkPoint.Id)
                        pointsName.Add(checkPoint.Name);
                }
            }
            return pointsName;
        }
        public bool IsOnTour(int guest2Id, int instanceId)
        {
            bool availabe = false;
            foreach (AlertGuest2 alert in alertGuest2Service.GetByInstanceIdAndGuestId(instanceId, guest2Id))
                if (alert.Availability)
                    availabe = true;
            
            return availabe;
        }
        public int CountWithoutVouchers(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                if (IsOnTour(reservation.GuestId, selectedId) && !reservation.WithVaucher)
                    counter += reservation.Capacity;
            
            return counter;
        }
        public int CountUnder18(int selectedId)
        {
            int under18Counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))   
                if (IsOnTour(reservation.GuestId, selectedId) && reservation.AverageGuestsAge <= 18)
                    under18Counter += reservation.Capacity;
            
            return under18Counter;
        }
        public int CountBetween18And50(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                if (IsOnTour(reservation.GuestId, selectedId) && reservation.AverageGuestsAge > 18 && reservation.AverageGuestsAge <= 50)
                    counter += reservation.Capacity;
            
            return counter;
        }
        public int CountOver50(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                if (IsOnTour(reservation.GuestId, selectedId) && reservation.AverageGuestsAge > 50)
                    counter += reservation.Capacity;
            
            return counter;
        }
        public double MakeWithVoucherPrecentage(int selectedId)
        {
            if (CountGuest(selectedId) != 0)
                return (double)CountWithVouchers(selectedId) /(double) CountGuest(selectedId) * 100;
            else
                return 0;
        }
        public double MakeWithoutVoucherPrecentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountWithoutVouchers(selectedId) /(double) CountGuest(selectedId) * 100;
            else
                return 0;
        }
        public int CountWithVouchers(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                if (IsOnTour(reservation.GuestId, selectedId) && reservation.WithVaucher)
                    counter += reservation.Capacity;
            
            return counter;
        }
        public double MakeUnder18Precentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountUnder18(selectedId) / (double)CountGuest(selectedId) * 100;
            else
                return 0.0;
        }
        public double MakeBetween18And50Precentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountBetween18And50(selectedId) / (double)CountGuest(selectedId) * 100;
            else
                return 0;
        }
        public double MakeOver50Precentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountOver50(selectedId) / (double)CountGuest(selectedId) * 100;
            else
                return 0.00;
        }
        public int CountAttendance(int selectedId)
        {
            int counter = 0;
            foreach (TourReservation reservation in reservationService.GetByInstanceId(selectedId))
                counter += reservation.Capacity;
            
            return counter;
        }
        public double MakeAttendancePrecentage(int selectedId)
        {
            if (CountGuest(selectedId)!=0)
                return CountAttendance(selectedId) / CountGuest(selectedId) * 100;
            else
                return 0.00;
        }
        public int CountGuestsOnPoint(int currentPointId, TourInstance selectedInstance)
        {
            int counter = 0;
            foreach (AlertGuest2 alert in alertGuest2Service.GetAll())
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                    counter += reservationService.GetTourReservationById(alert.ReservationId).Capacity;
            
            return counter;
        }
        public List<CheckPoint> GetInstancePoints(TourInstance selectedInstance)
        {
            List<CheckPoint> tourPoints = new List<CheckPoint>();
            foreach (CheckPoint point in checkPointService.GetAll())
                if (point.TourId == selectedInstance.Tour.Id)
                    tourPoints.Add(point);

            return tourPoints;
        }
    }
}
