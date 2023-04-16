using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for RateTourAndGuide.xaml
    /// </summary>
    public partial class GuideAndTourReviewFormView : Window
    {
        public GuideAndTourReviewFormView(TourInstance tourInstance, Guest2 guest2)
        {
            InitializeComponent();
            DataContext = new GuideAndTourReviewViewModel(tourInstance, guest2, knowledge, language, interestingFacts, imagePicture, comment);
        }
    }
}
