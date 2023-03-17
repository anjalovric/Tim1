﻿using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for CheckPointForm.xaml
    /// </summary>
    public partial class CheckPointForm : Window, INotifyPropertyChanged
    {
        private readonly CheckPointRepository checkPointRepository;
        public ObservableCollection<CheckPoint> checkPoints;
        public int pointCounter;
     
        private string name;
        public string NameT
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public CheckPointForm(CheckPointRepository _checkPointRepository,ObservableCollection<CheckPoint> tourPoints)
        {
            InitializeComponent();
            DataContext = this;
            checkPointRepository = new CheckPointRepository();
            checkPoints=tourPoints;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                CheckPoint newCheckPoint = new CheckPoint(NameT,false,-1,-1);
                CheckPoint savedCheckPoint = checkPointRepository.Save(newCheckPoint);
                checkPoints.Add(savedCheckPoint);

                
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool IsValid()
        {
            bool valid = false;
            if (CheckPointName.Text.Trim().Equals(""))
            {
                CheckPointName.BorderBrush = Brushes.Red;
                CheckPointName.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else
            {
                valid = true;
                CheckPointName.BorderBrush = Brushes.Green;
                NameLabel.Content = string.Empty;
            }
            return valid;
        }
    }
}
