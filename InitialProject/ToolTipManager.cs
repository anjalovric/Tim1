using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject
{
    public class ToolTipManager : INotifyPropertyChanged
    {
        private bool _isToolTipEnabled = true;
        public bool IsToolTipEnabled
        {
            get { return _isToolTipEnabled; }
            set
            {
                _isToolTipEnabled = value;
                OnPropertyChanged(nameof(IsToolTipEnabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
