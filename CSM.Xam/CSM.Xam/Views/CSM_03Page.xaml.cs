using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSM.Xam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CSM_03Page : ContentPage
    {
        public CSM_03Page()
        {
            InitializeComponent();
            picker.SelectedIndex = 0;
        }
    }
}