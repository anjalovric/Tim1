using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        public VoucherService()
        {
            voucherRepository = new VoucherRepository();
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
            GuideService guideService = new GuideService();
            TourReservationService tourReservationService = new TourReservationService();
            foreach (TourReservation reservation in tourReservationService.GetReservationsForTourInstance(tourInstanceId))
            {
                Voucher voucher = new Voucher(false, reservation.GuestId, guideService.GetByUsername(tourInstanceGuide.Username).Id, DateTime.Now);
                Voucher savedVoucher = Save(voucher);

            }

        }

    }
}