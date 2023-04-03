using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository:IGenericRepository<Voucher>
    {
        public Voucher Save(Voucher voucher);

        public Voucher Update(Voucher voucher);

        public void Delete(Voucher voucher);
    }
}
