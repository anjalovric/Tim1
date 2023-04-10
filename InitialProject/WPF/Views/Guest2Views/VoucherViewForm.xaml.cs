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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for VoucherViewForm.xaml
    /// </summary>
    public partial class VoucherViewForm : UserControl
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
        private ObservableCollection<Voucher> voucher; 
        public VoucherViewForm(Model.Guest2 guest)
        {
            InitializeComponent();
            DataContext = this;
            this.guest2 = guest;
            voucherService = new VoucherService();
            voucher = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(voucher,guest2));
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
