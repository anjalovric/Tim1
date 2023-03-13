﻿using System;
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
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AlertGuestForm.xaml
    /// </summary>
    public partial class AlertGuestForm : Window
    {
        private const string FilePath = "../../../Resources/Data/alertsGuest2.csv";

        private readonly Serializer<AlertGuest2> _serializer;

        private List<AlertGuest2> alerts;
        private AlertGuest2Repository _alertGuest2Repository;
        private CheckPointRepository _checkPointRepository;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        private int AlertId;
        public AlertGuestForm(int alertId)
        {
            InitializeComponent();
            AlertId = alertId;
            DataContext = this;
            _alertGuest2Repository = new AlertGuest2Repository();
            _checkPointRepository = new CheckPointRepository();
            _tourRepository = new TourRepository();
            _tourInstanceRepository = new TourInstanceRepository();

            _serializer = new Serializer<AlertGuest2>();
            alerts = _serializer.FromCSV(FilePath);

            CreateLabelContent();
            
        }
        private void CreateLabelContent()
        {
            int pointId = _alertGuest2Repository.GetAll().Find(n => n.Id == AlertId).CheckPointId;
            int instanceId = _alertGuest2Repository.GetAll().Find(n => n.Id == AlertId).InstanceId;
            Tour thisTour = _tourInstanceRepository.GetAll().Find(n => n.Id == instanceId).Tour;
            PointLabel.Content = "Are you present on point " + _checkPointRepository.GetAll().Find(n => n.Id == pointId).Name + " on tour " +
                                thisTour.Name + " ?";
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            foreach(AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id==AlertId)
                {
                    alertGuest2.Availability = true;
                    alertGuest2.Informed = true;
                    _alertGuest2Repository.Update(alertGuest2);
                }
            }
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id == AlertId)
                {
                   
                    alertGuest2.Informed = true;
                    _alertGuest2Repository.Update(alertGuest2);
                }
            }
            this.Close();
        }
    }
}
