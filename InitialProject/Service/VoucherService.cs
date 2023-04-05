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

        public ObservableCollection<Voucher> Vouchers { get; set; }
        public List<Voucher> storedVouchers;
        public VoucherService() 
        {
            voucherRepository = new VoucherRepository();
            Vouchers= new ObservableCollection<Voucher>();
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
        public ObservableCollection<Voucher> FindAllVouchers(ObservableCollection<Voucher> Vouchers, Guest2 guest2)
        {
            foreach (Voucher voucher in storedVouchers)
            {
                if (voucher.GuestId == guest2.Id && voucher.Used == false)
                {
                    Vouchers.Add(voucher);
                }
            }
            return Vouchers;
        }
    }

}
