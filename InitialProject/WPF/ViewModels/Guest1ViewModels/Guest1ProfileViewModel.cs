using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ProfileViewModel
    {
        private Guest1 guest1;
        public Guest1ProfileViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            Uri imageUri = new Uri(guest1.ImagePath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            //accommodationImage.Source = imageBitmap;
        }
    }
}
