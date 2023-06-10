using InitialProject.Help;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

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
        public ICommand HelpCommandInViewModel { get; }
        private VoucherFormView org;
        public VoucherViewModel(Model.Guest2 guest, VoucherFormView org)
        {
            this.guest2 = guest;
            this.org = org;
            WinningAVoucher winningAVoucher = new WinningAVoucher(guest2.Id);
            winningAVoucher.CountOfTours();
            voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>();
            Vouchers = new ObservableCollection<Voucher>(voucherService.FindAllVouchers(guest2));
            VoucherValidity(Vouchers);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private void VoucherValidity(ObservableCollection<Voucher> Vouchers)
        {
            foreach (Voucher voucher in Vouchers)
            {
                if (voucher.Type == VoucherType.CANCELED_TOUR)
                    voucher.CreateDate = voucher.CreateDate.AddYears(1);
                else if(voucher.Type == VoucherType.VISITED_TOUR)
                    voucher.CreateDate = voucher.CreateDate.AddMonths(6);
                else
                    voucher.CreateDate = voucher.CreateDate.AddYears(2);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForVouchers(str, org);
            }
        }
    }
}

