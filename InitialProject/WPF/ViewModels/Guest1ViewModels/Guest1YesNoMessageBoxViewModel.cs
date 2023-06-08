using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1YesNoMessageBoxViewModel
    {
        private readonly TaskCompletionSource<bool> _result;
        public string Text { get; set; }
        public BitmapImage Image { get; set; }
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }
        public Guest1YesNoMessageBoxViewModel(string messageText, string path, TaskCompletionSource<bool> result)
        {
            _result = result;
            Text = messageText;
            Image = new BitmapImage(new Uri(path, UriKind.Relative));
            MakeCommands();
        }
        private void MakeCommands()
        {
            YesCommand = new RelayCommand(Yes_Executed, CanExecute);
            NoCommand = new RelayCommand(No_Executed, CanExecute);
        }
        private void Yes_Executed(object sender)
        {
            _result.SetResult(true);
            Application.Current.Windows.OfType<Guest1YesNoMessageBoxView>().FirstOrDefault().Close();
        }
        private void No_Executed(object sender)
        {
            _result.SetResult(false);
            Application.Current.Windows.OfType<Guest1YesNoMessageBoxView>().FirstOrDefault().Close();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
    }
}
