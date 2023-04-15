using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.Service;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        public Guest2Overview(User user)
        {
            InitializeComponent();
            DataContext = new Guest2OverviewViewModel(user,CC);
        }
    }
}
