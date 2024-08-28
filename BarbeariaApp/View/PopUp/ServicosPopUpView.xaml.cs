using BarbeariaApp.ViewModel.PopUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarbeariaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicosPopUpView : Rg.Plugins.Popup.Pages.PopupPage
    {
        ServicosPopUpViewModel viewModel = new ServicosPopUpViewModel();
        public ServicosPopUpView()
        {
            this.BindingContext = viewModel;
            InitializeComponent();
        }
    }
}