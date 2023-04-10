using System;
using System.Collections.Generic;
using System.Linq;
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
using InitialProject.Domain.Model;
using InitialProject.Model;
using NPOI.SS.Formula.PTG;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Profile.xaml
    /// </summary>
    public partial class Guest1Profile : Page
    {
        public Guest1 Guest1 {get; set;}
        public Guest1Profile(Guest1 guest1)
        {
            InitializeComponent();
            this.DataContext = this;
            Guest1 = guest1;
            Uri imageUri = new Uri(guest1.ImagePath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            accommodationImage.Source = imageBitmap;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
