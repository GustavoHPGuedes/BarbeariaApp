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
    public partial class MensagemPopUpView : Rg.Plugins.Popup.Pages.PopupPage
    {
        MensagemPopUpViewModel viewModel = new MensagemPopUpViewModel();
        public MensagemPopUpView()
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}