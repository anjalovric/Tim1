using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        ShowToursView prozor;
        public JavaScriptControlHelper(ShowToursView w)
        {
            prozor = w;
        }

       
    }
}
