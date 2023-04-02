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
        private VoucherRepository voucherRepository;
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
    }

}
