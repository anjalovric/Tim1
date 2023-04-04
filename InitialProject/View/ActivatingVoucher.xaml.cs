using InitialProject.Model;
using InitialProject.Repository;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ActivatingVoucher.xaml
    /// </summary>
    public partial class ActivatingVoucher : Window
    {
        private VoucherService voucherService;
        private Boolean activatedVoucher;
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
        public ActivatingVoucher(Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(vouchers, guest2));
            activatedVoucher = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Boolean IsActivate(Boolean activatedVoucher)
        {
            activatedVoucher = true;
            return activatedVoucher;
        }
        private void Activate(object sender, RoutedEventArgs e)
        {
            Voucher currentVoucher = (Voucher)ActivationVoucherDataGrid.CurrentItem;
            activatedVoucher =IsActivate(activatedVoucher);
            voucherService.Update(currentVoucher);
            this.Close();
        }
    }
}
