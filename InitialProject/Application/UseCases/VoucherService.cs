using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        public List<Voucher> storedVouchers;
        public VoucherService() 
        {
            storedVouchers = GetAll();
        }
        public List<Voucher> GetAll()
        {
            return voucherRepository.GetAll();
        }
        public Voucher Save(Voucher voucher)
        {
            return voucherRepository.Save(voucher);
        }
        public Voucher Update(Voucher voucher)
        {
            return voucherRepository.Update(voucher);
        }
        public void SendVoucher(int tourInstanceId, User tourInstanceGuide)
        {
            TourReservationService tourReservationService=new TourReservationService();
            GuideService guideService=new GuideService();
            foreach (TourReservation reservation in tourReservationService.GetReservationsForTourInstance(tourInstanceId))
            {
                Voucher voucher = new Voucher();
                voucher.Used = false;
                voucher.GuestId = reservation.GuestId;
                voucher.GuideId = guideService.GetByUsername(tourInstanceGuide.Username).Id;
                voucher.CreateDate = DateTime.Now;
                voucher.Type = VoucherType.CANCELED_TOUR;
                Voucher savedVoucher = Save(voucher);
            }
        }
        public void SendVoucherForVisitedTours(int guest2Id)
        { 
            Voucher voucher = new Voucher();
            voucher.Used = false;
            voucher.GuestId = guest2Id;
            voucher.GuideId = -1;
            voucher.CreateDate = DateTime.Now;
            voucher.Type = VoucherType.VISITED_TOUR;
            Boolean exist = false;
            foreach (Voucher storedVoucher in GetAll())
            {
                if(storedVoucher.GuestId == guest2Id && storedVoucher.Type==VoucherType.VISITED_TOUR)
                    exist= true;
            }
            if (!exist)
            {
                Voucher savedVoucher = Save(voucher);
            }
           
        }
        public List<Voucher> FindAllVouchers(Guest2 guest2)
        {
            List<Voucher> Vouchers = new List<Voucher>();
            foreach (Voucher voucher in storedVouchers)
            {
                if (voucher.GuestId == guest2.Id && voucher.Used == false && voucher.CreateDate.AddYears(1)>=DateTime.Now)
                {
                    Vouchers.Add(voucher);
                }
            }
            return Vouchers;
        }

        public void SendVoucher(int guideId)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            TourReservationService tourReservationService = new TourReservationService();
            foreach(TourInstance tourInstance in tourInstanceService.GetAll())
            {
                if(tourInstance.StartDate>DateTime.Now && tourInstance.Finished==false && tourInstance.Canceled == false && tourInstance.Guide.Id==guideId)
                {
                    if (tourReservationService.GetAll().Find(x => x.TourInstanceId == tourInstance.Id) != null)
                    {
                        TourReservation tourReservation = tourReservationService.GetAll().Find(x => x.TourInstanceId == tourInstance.Id);
                        CreateVoucher(tourReservation.GuestId);
                    }
                    tourInstance.Canceled = true;
                    tourInstanceService.Update(tourInstance);
                }
            }

        }
        private void CreateVoucher(int guest2Id)
        {
            Voucher voucher = new Voucher();    
            voucher.GuestId = guest2Id;
            voucher.CreateDate = DateTime.Now;
            voucher.GuideId = -1;
            voucher.Used = false;
            voucher.Type = VoucherType.DISMISSAL_GUIDE;
            Save(voucher);
        }

        public void ChangeAssignedGuide(int guideId)
        {
            foreach (Voucher voucher in GetAll())
                if (voucher.GuestId == guideId && voucher.Used == false && voucher.Type==VoucherType.CANCELED_TOUR)
                {
                    voucher.GuideId = -1;
                    Update(voucher);
                }
        }
    }
}
