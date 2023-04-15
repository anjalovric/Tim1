using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Page
    {
        private Owner owner;
        public AccommodationView(Owner owner)
        {
            InitializeComponent();
            this.owner = owner;
            DataContext = new AccommodationViewModel(owner);
        }
    }
}
