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
        private readonly CheckPointRepository _checkPointRepository;
        public ObservableCollection<CheckPoint> _checkPoints;

        private string _name;
        public string NameT
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public CheckPointForm(CheckPointRepository checkPointRepository,ObservableCollection<CheckPoint> tourPoints)
        {
            InitializeComponent();
            DataContext = this;
            _checkPointRepository = checkPointRepository;
            _checkPoints=tourPoints;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddCheckPoint(object sender, RoutedEventArgs e)
        {
            CheckPoint newCheckPoint = new CheckPoint();
            newCheckPoint.Name = NameT;
            newCheckPoint.Order = -1;
            newCheckPoint.TourId = -1;
            CheckPoint savedCheckPoint = _checkPointRepository.Save(newCheckPoint);
            _checkPoints.Add(savedCheckPoint);
            this.Close();
        }

        private void CancelCheckPoint(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
