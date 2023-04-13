using InitialProject.Model;
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
        public bool IsOnTour(int guest2Id, int instanceId)
        {
            foreach (AlertGuest2 alert in alertGuest2Service.GetByInstanceIdAndGuestId(instanceId, guest2Id))
                if (alert.Availability)
                    return true;            
            return false;
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
            return 0;
        }
        public double MakeWithoutVoucherPrecentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountWithoutVouchers(selectedId) /(double) CountGuest(selectedId) * 100;
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
            return 0.0;
        }
        public double MakeBetween18And50Precentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountBetween18And50(selectedId) / (double)CountGuest(selectedId) * 100;
            return 0.0;
        }
        public double MakeOver50Precentage(int selectedId)
        {
            if(CountGuest(selectedId)!=0)
                return (double)CountOver50(selectedId) / (double)CountGuest(selectedId) * 100;
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
                return (double)CountAttendance(selectedId) / (double)CountGuest(selectedId) * 100;
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
    }
}
