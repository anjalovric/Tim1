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
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using InitialProject.Domain.Model;
namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class GraphForLanguage : UserControl
    {
        public GraphForLanguage()
        {
            requestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(requestsService.GetOnlyOrdinary());
            FindLangauge();
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
            Labels=SetLabels();
            Formatter = value => value.ToString();


            DataContext = this;
            InitializeComponent();
        }

        public int MaxValue { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        private int srbCounter = 0;
        public Func<double, string> Formatter { get; set; }
        private OrdinaryTourRequestsService requestsService;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        public ChartValues<int> Values { get; set; }
        private int rusCounter = 0;
        private int arabCounter = 0;
        private int engCounter = 0;
        private int itCounter = 0;
        private int spCounter = 0;
        private void FindLangauge()
        {
            foreach(var o in OrdinaryTourRequests)
            {
                if (o.Language == "serbian")
                    srbCounter++;
                else if (o.Language == "russian")
                    rusCounter++;
                else if (o.Language == "arabic")
                    arabCounter++;
                else if (o.Language == "italian")
                    itCounter++;
                else if (o.Language == "english")
                    engCounter++;
                else if (o.Language == "spanish")
                    spCounter++;
            }

        }
        private ChartValues<int> SetValues()
        {
            if (srbCounter != 0)
                Values.Add(srbCounter);
            if (rusCounter != 0)
                Values.Add(rusCounter);
            if (arabCounter != 0)
                Values.Add(arabCounter);
            if (spCounter != 0)
                Values.Add(spCounter);
            if (engCounter != 0)
                Values.Add(engCounter);
            if (itCounter != 0)
                Values.Add(itCounter);
            return Values;
        }
        private List<string> SetLabels()
        {
            if (srbCounter != 0)
                Labels.Add("SRB");
            if (rusCounter != 0)
                Labels.Add("RUS");
            if (arabCounter != 0)
                Labels.Add("ARAB");
            if (spCounter != 0)
                Labels.Add("ESP");
            if (engCounter != 0)
                Labels.Add("ENG");
            if (itCounter != 0)
                Labels.Add("ITA");
            return Labels;
        }
    }
}
