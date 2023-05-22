using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AnywhereAnytimeViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;

        public AnywhereAnytimeViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
