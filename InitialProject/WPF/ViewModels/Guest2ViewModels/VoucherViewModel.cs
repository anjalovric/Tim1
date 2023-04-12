using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class VoucherViewModel:INotifyPropertyChanged
    {
        private VoucherService voucherService;
        private Model.Guest2 guest2;
        private ObservableCollection<Voucher> vouchers;
        public ObservableCollection<Voucher> Vouchers
        {
            get { return vouchers; }
            set
            {
                if (value != vouchers)
                    vouchers = value;
                OnPropertyChanged("Vouchers");
            }
        }
        public VoucherViewModel(Model.Guest2 guest)
        {
            this.guest2 = guest;
            voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(guest2));
            VoucherValidity(Vouchers);
        }
        private void VoucherValidity(ObservableCollection<Voucher> Vouchers)
        {
            foreach (Voucher voucher in Vouchers)
            {
                voucher.CreateDate = voucher.CreateDate.AddYears(1);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

