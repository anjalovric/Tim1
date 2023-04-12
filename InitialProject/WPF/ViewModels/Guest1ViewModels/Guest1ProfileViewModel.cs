using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ProfileViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        private BitmapImage imageSource { get; set; }
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                    imageSource = value;
                OnPropertyChanged("ImageSource");
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest1ProfileViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            string relative = FindImageRelativePath();
            ImageSource = new BitmapImage(new Uri(relative, UriKind.Relative));
        }
        private string FindImageRelativePath()
        {
            return guest1.ImagePath;
        }
        
    }
}
