using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Help
{
    public class ShowToursHelp
    {
        public static string GetHelpKey(DependencyObject obj)
        {
            return obj.GetValue(HelpKeyProperty) as string;
        }

        public static void SetHelpKey(DependencyObject obj, string value)
        {
            obj.SetValue(HelpKeyProperty, value);
        }

        public static readonly DependencyProperty HelpKeyProperty =
            DependencyProperty.RegisterAttached("HelpKey", typeof(string), typeof(ShowToursHelp), new PropertyMetadata("index", HelpKey));
        private static void HelpKey(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //NOOP
        }

        public static void ShowHelp(string key, ShowToursView originator)
        {
            ShowToursHelpForm hh = new ShowToursHelpForm(key, originator);
            hh.Show();
        }
        public static void ShowHelpForFinished(string key, FinishedTourInstancesFormView originator)
        {
            FinishedTourInstancesHelpForm hh = new FinishedTourInstancesHelpForm(key, originator);
            hh.Show();
        }
        public static void ShowHelpForActive(string key, ActiveToursFormView originator)
        {
            ActiveToursHelpForm hh = new ActiveToursHelpForm(key, originator);
            hh.Show();
        }
        public static void ShowHelpForVouchers(string key, VoucherFormView originator)
        {
            VouchersHelpForm hh = new VouchersHelpForm(key, originator);
            hh.Show();
        }
        public static void ShowHelpForNotification(string key, NotificationsFormView originator)
        {
            NotificationsHelpForm hh = new NotificationsHelpForm(key, originator);
            hh.Show();
        }
        public static void ShowHelpForRequests(string key, MyRequestsFormView originator)
        {
            RequestsHelpForm hh = new RequestsHelpForm(key, originator);
            hh.Show();
        }
    }
}
