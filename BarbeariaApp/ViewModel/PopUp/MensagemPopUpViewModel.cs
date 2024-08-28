using BarbeariaApp.Model;
using BarbeariaApp.ViewModel.Page;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.PopUp
{
    public class MensagemPopUpViewModel : BaseViewModel
    {
        private string TituloBinding;

        private string _TextoMensagem = "";
        public string TextoMensagem
        {
            get => _TextoMensagem;
            set
            {
                _TextoMensagem = value;
                OnPropertyChanged();
            }
        }

        public Command CommandRespostaSim { get; set; }
        public Command CommandRespostaNao { get; set; }

        public MensagemPopUpViewModel()
        {
            CommandRespostaSim = new Command(RespostaSim);
            CommandRespostaNao = new Command(RespostaNao);

            TextoMensagem = "Se essa mensagem estiver aparecendo, clique em 'NÃO'.'";

            //Produtos
            MessagingCenter.Subscribe<ProdutoViewModel, string>(this, "TextoMensagem", (sender, mensagem) =>
            {
                TextoMensagem = mensagem;
            });
            MessagingCenter.Subscribe<ProdutoViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });

            //Serviços
            MessagingCenter.Subscribe<ServicosViewModel, string>(this, "TextoMensagem", (sender, mensagem) =>
            {
                TextoMensagem = mensagem;
            });
            MessagingCenter.Subscribe<ServicosViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });

            //Agenda
            MessagingCenter.Subscribe<AgendaViewModel, string>(this, "TextoMensagem", (sender, mensagem) =>
            {
                TextoMensagem = mensagem;
            });
            MessagingCenter.Subscribe<AgendaViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });

            //Atendimento
            MessagingCenter.Subscribe<AtendimentosViewModel, string>(this, "TextoMensagem", (sender, mensagem) =>
            {
                TextoMensagem = mensagem;
            });
            MessagingCenter.Subscribe<AtendimentosViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });
        }

        private void RespostaSim()
        {
            MessagingCenter.Send(this, TituloBinding, true);
            FechaPopUp();
        }

        private void RespostaNao()
        {
            MessagingCenter.Send(this, TituloBinding, false);
            FechaPopUp();
        }

        private async void FechaPopUp()
        {
            MessagingCenter.Unsubscribe<ProdutoViewModel>(this, "TextoMensagem");
            MessagingCenter.Unsubscribe<ProdutoViewModel>(this, "TituloBinding");

            MessagingCenter.Unsubscribe<ServicosViewModel>(this, "TextoMensagem");
            MessagingCenter.Unsubscribe<ServicosViewModel>(this, "TituloBinding");

            MessagingCenter.Unsubscribe<AgendaViewModel>(this, "TextoMensagem");
            MessagingCenter.Unsubscribe<AgendaViewModel>(this, "TituloBinding");

            MessagingCenter.Unsubscribe<AtendimentosViewModel>(this, "TextoMensagem");
            MessagingCenter.Unsubscribe<AtendimentosViewModel>(this, "TituloBinding");

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
