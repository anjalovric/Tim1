using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
using Syncfusion.XPS;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Page
    {
        private Owner owner;
        public AccommodationViewModel ViewModel { get; set; }
        public AccommodationView(Owner owner)
        {
            InitializeComponent();
            this.owner = owner;
            ViewModel = new AccommodationViewModel(owner);
            DataContext = ViewModel;
        }
    }
}
