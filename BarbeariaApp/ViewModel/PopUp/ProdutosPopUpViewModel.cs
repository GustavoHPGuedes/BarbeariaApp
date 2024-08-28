using BarbeariaApp.Model;
using BarbeariaApp.ViewModel.Page;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.PopUp
{
    public class ProdutosPopUpViewModel : BaseViewModel
    {
        private string TituloBinding;

        private ObservableCollection<Produto> _Produtos = new ObservableCollection<Produto>();
        public ObservableCollection<Produto> Produtos
        {
            get => _Produtos;
            set
            {
                _Produtos = value;
                OnPropertyChanged();
            }
        }

        private Produto _ProdutoSelecionado = new Produto();
        public Produto ProdutoSelecionado
        {
            get => _ProdutoSelecionado;
            set
            {
                _ProdutoSelecionado = value;
                OnPropertyChanged();
            }
        }

        public Command CommandSelecionar { get; set; }
        public Command CommandVoltar { get; set; }

        public ProdutosPopUpViewModel()
        {
            CarregaProdutos();
            CommandSelecionar = new Command(Selecionar);
            CommandVoltar = new Command(Voltar);

            MessagingCenter.Subscribe<AtendimentosViewModel>(this, "AbrePopUpProdutos", (sender) =>
            {
                CarregaProdutos();
            });
            MessagingCenter.Subscribe<AtendimentosViewModel, string>(this, "TituloBinding", (sender, titulo) =>
            {
                TituloBinding = titulo;
            });
        }

        private async void CarregaProdutos()
        {
            Produtos = new ObservableCollection<Produto>(await Connection.db.Table<Produto>().ToListAsync());
        }

        private async void Selecionar()
        {
            if (ProdutoSelecionado.Codigo <= 0) { return; }

            MessagingCenter.Send(this, TituloBinding, ProdutoSelecionado.Codigo);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Voltar()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
