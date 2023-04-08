using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationInputFormViewModel : INotifyPropertyChanged
    {
        private Owner owner;
        private Location location;
        private string name;
        private AccommodationType type;
        private int capacity;
        private int minDaysForReservation;
        private int minDaysToCancel;
        public AccommodationInputFormViewModel(Owner owner)
        {
            this.owner = owner;
            location= new Location();
        }

        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinDaysForReservation
        {
            get { return minDaysForReservation; }
            set
            {
                if (value != minDaysForReservation)
                {
                    minDaysForReservation = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinDaysToCancel
        {
            get { return minDaysToCancel; }
            set
            {
                if (value != minDaysToCancel)
                {
                    minDaysToCancel = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
