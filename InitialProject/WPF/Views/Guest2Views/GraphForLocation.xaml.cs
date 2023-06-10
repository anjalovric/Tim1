using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using Xceed.Wpf.Toolkit.Core;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class GraphForLocation : UserControl
    {
        public GraphForLocation()
        {
            requestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<Domain.Model.OrdinaryTourRequests>(requestsService.GetOnlyOrdinary());
            locationService = new LocationService();
            locations = new List<Model.Location>(locationService.GetAll());
            FindLocation();
            Values = new ChartValues<int>();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Broj zahtjeva",
                    Values = SetValues()
                }
            };

            Labels = new List<string>();
            Labels = SetLabels();
            Formatter = value => value.ToString();


            DataContext = this;
            InitializeComponent();
        }

        public int MaxValue { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        
        public Func<double, string> Formatter { get; set; }
        private OrdinaryTourRequestsService requestsService;
        private List<Domain.Model.OrdinaryTourRequests> OrdinaryTourRequests;
        private LocationService locationService;
        private List<Model.Location> locations;
        private int tCounter = 0;
        private int sCounter = 0;
        private int bCounter = 0;
        private int aCounter = 0;
        private int hCounter = 0;
        private int rCounter = 0;
        private int iCounter = 0;
        private int gCounter = 0;
        public ChartValues<int> Values { get; set; }
        
        private void FindLocation()
        {
            foreach(var o in OrdinaryTourRequests)
            {
                foreach (Model.Location location in locations)
                {
                    if (location.Country == "Turkey" && location.Id==o.Location.Id)
                        tCounter++;
                    else if (location.Country == "Serbia" && location.Id == o.Location.Id)
                        sCounter++;
                    else if (location.Country == "Russia" && location.Id == o.Location.Id)
                        rCounter++;
                    else if (location.Country == "Austria" && location.Id == o.Location.Id)
                        aCounter++;
                    else if (location.Country == "Greece" && location.Id == o.Location.Id)
                        gCounter++;
                    else if (location.Country == "Hungary" && location.Id == o.Location.Id)
                        hCounter++;
                    else if (location.Country == "Italy" && location.Id == o.Location.Id)
                        iCounter++;
                    else if (location.Country == "BiH" && location.Id == o.Location.Id)
                        bCounter++;
                }
            }
        }
        private ChartValues<int> SetValues()
        {
            if (tCounter != 0)
                Values.Add(tCounter);
            if (sCounter != 0)
                Values.Add(sCounter);
            if (rCounter != 0)
                Values.Add(rCounter);
            if (aCounter != 0)
                Values.Add(aCounter);
            if (gCounter != 0)
                Values.Add(gCounter);
            if (hCounter != 0)
                Values.Add(hCounter);
            if (iCounter != 0)
                Values.Add(iCounter);
            if (bCounter != 0)
                Values.Add(bCounter);
            return Values;
        }
        private List<string> SetLabels()
        {
            if (tCounter != 0)
                Labels.Add("TUR");
            if (sCounter != 0)
                Labels.Add("SRB");
            if (rCounter != 0)
                Labels.Add("RUS");
            if (aCounter != 0)
                Labels.Add("AUS");
            if (gCounter != 0)
                Labels.Add("GRC");
            if (hCounter != 0)
                Labels.Add("HUN");
            if (iCounter != 0)
                Labels.Add("ITA");
            if (bCounter != 0)
                Labels.Add("BiH");
            return Labels;
        }
    }
}
