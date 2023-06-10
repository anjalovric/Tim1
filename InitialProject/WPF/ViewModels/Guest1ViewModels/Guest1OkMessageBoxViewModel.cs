using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Media.Imaging;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1OkMessageBoxViewModel
    {
        public string Text { get; set; }
        public BitmapImage Image { get; set; }
        public RelayCommand OkCommand { get; set; }

        public Guest1OkMessageBoxViewModel(string messageText, string path)
        {
            Text = messageText;
            Image = new BitmapImage(new Uri(path, UriKind.Relative));
            MakeCommands();
        }
        private void MakeCommands()
        {
            OkCommand = new RelayCommand(Ok_Executed, CanExecute);
        }
        private void Ok_Executed(object sender)
        {
            Application.Current.Windows.OfType<Guest1OkMessageBoxView>().FirstOrDefault().Close();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        

    }
}
