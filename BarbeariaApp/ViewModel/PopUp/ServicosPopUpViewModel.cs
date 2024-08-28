using BarbeariaApp.Model;
using BarbeariaApp.View;
using BarbeariaApp.ViewModel.Page;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.PopUp
{
    public class ServicosPopUpViewModel : BaseViewModel
    {
        private string TituloBinding;

        private ObservableCollection<Servico> _Servicos = new ObservableCollection<Servico>();
        public ObservableCollection<Servico> Servicos
        {
            get => _Servicos;
            set
            {
                _Servicos = value;
                OnPropertyChanged();
            }
        }

        private Servico _ServicoSelecionado = new Servico();
        public Servico ServicoSelecionado
        {
            get => _ServicoSelecionado;
            set
            {
                _ServicoSelecionado = value;
                OnPropertyChanged();
            }
        }

        public Command CommandSelecionar { get; set; }
        public Command CommandVoltar { get; set; }

        public ServicosPopUpViewModel()
        {
            CarregaServicos();
            CommandSelecionar = new Command(Selecionar);
            CommandVoltar = new Command(Voltar);

            MessagingCenter.Subscribe<AtendimentosViewModel>(this, "AbrePopUpServicos", (sender) =>
            {
                CarregaServicos();
            });
            MessagingCenter.Subscribe<AtendimentosViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });
        }

        private async void CarregaServicos()
        {
            Servicos = new ObservableCollection<Servico>(await Connection.db.Table<Servico>().ToListAsync());
        }

        private async void Selecionar()
        {
            if (ServicoSelecionado.Codigo <= 0) { return; }

            MessagingCenter.Send(this, TituloBinding, ServicoSelecionado.Codigo);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Voltar()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
