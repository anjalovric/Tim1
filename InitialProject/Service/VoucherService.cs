using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Voucher savedVoucher = Save(voucher);
            }
        }
        public ObservableCollection<Voucher> FindAllVouchers(Guest2 guest2)
        {
            ObservableCollection<Voucher> Vouchers = new ObservableCollection<Voucher>();
            foreach (Voucher voucher in storedVouchers)
            {
                if (voucher.GuestId == guest2.Id && voucher.Used == false && voucher.CreateDate.AddYears(1)>=DateTime.Now)
                {
                    Vouchers.Add(voucher);
                }
            }
            return Vouchers;
        }
    }
}
