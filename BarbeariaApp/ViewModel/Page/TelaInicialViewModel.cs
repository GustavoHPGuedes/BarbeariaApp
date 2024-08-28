using BarbeariaApp.View;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.Page
{
    public class TelaInicialViewModel : BaseViewModel
    {
        TelaInicialPopUpView telaInicialPopUp = new TelaInicialPopUpView();

        public Command CommandAbreTelaInicialPopUp { get; set; }

        public TelaInicialViewModel()
        {
            CommandAbreTelaInicialPopUp = new Command(AbreTelaInicialPopUp);
        }

        private async void AbreTelaInicialPopUp()
        {
            await PopupNavigation.Instance.PushAsync(telaInicialPopUp);
        }
    }
}
