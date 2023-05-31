using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class PartsOfComplexRequestTourViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests { get; set; }
        private OrdinaryTourRequestsService requestsService;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (!value.Equals(name))
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Status { get; set; }
        private DateTime Start;
        public DateTime StartDate
        {
            get => Start;
            set
            {
                if (value != Start)
                {
                    Start = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PartsOfComplexRequestTourViewModel(ComplexTourRequests complex)
        {
            requestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>(requestsService.GetOrdinaryTourRequestsByComplex(complex.Id));
        }
    }
}
