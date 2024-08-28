using BarbeariaApp.Model;
using BarbeariaApp.View;
using BarbeariaApp.ViewModel.PopUp;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.Page
{
    public class ProdutoViewModel : BaseViewModel
    {
        public bool Incluindo = true;
        public bool Alterando = false;
        public int Codigo = 0;

        private int IDmsg = 0;
        MensagemPopUpView mensagemPopUp = new MensagemPopUpView();

        private bool _TelaConsultaVisivel = true;
        public bool TelaConsultaVisivel
        {
            get => _TelaConsultaVisivel;
            set
            {
                _TelaConsultaVisivel = value;
                OnPropertyChanged();
            }
        }

        private bool _TelaModificacoesVisivel = false;
        public bool TelaModificacoesVisivel
        {
            get => _TelaModificacoesVisivel;
            set
            {
                _TelaModificacoesVisivel = value;
                OnPropertyChanged();
            }
        }

        private bool _OpcoesCadastroVisivel = false;
        public bool OpcoesCadastroVisivel
        {
            get => _OpcoesCadastroVisivel;
            set
            {
                _OpcoesCadastroVisivel = value;
                OnPropertyChanged();
            }
        }

        private bool _OpcoesAlteracoesVisivel = false;
        public bool OpcoesAlteracoesVisivel
        {
            get => _OpcoesAlteracoesVisivel;
            set
            {
                _OpcoesAlteracoesVisivel = value;
                OnPropertyChanged();
            }
        }

        private bool _InputReadOnly = false;
        public bool InputReadOnly
        {
            get => _InputReadOnly;
            set
            {
                _InputReadOnly = value;
                OnPropertyChanged();
            }
        }

        private string _DescricaoProduto;
        public string DescricaoProduto
        {
            get => _DescricaoProduto;
            set
            {
                _DescricaoProduto = value;
                OnPropertyChanged();
            }
        }

        private string _ValorProduto = null;
        public string ValorProduto
        {
            get => _ValorProduto;
            set
            {
                _ValorProduto = value;
                OnPropertyChanged();
            }
        }

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

        public Command CommandTelaAdicionaProduto { get; set; }
        public Command CommandVoltaTelaConsulta { get; set; }
        public Command CommandConfirmaModificaoProduto { get; set; }
        public Command CommandDeletaProduto { get; set; }
        public Command CommandAlteraProduto { get; set; }

        public ProdutoViewModel()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;

            CommandTelaAdicionaProduto = new Command(TelaAdicionaProduto);
            CommandVoltaTelaConsulta = new Command(VoltaTelaConsulta);
            CommandConfirmaModificaoProduto = new Command(ConfirmaModificacaoProduto);
            CommandDeletaProduto = new Command(DeletaProduto);
            CommandAlteraProduto = new Command(AlteraProduto);

            CarregaProdutos();
        }

        private void TelaAdicionaProduto()
        {
            Incluindo = true;
            Alterando = false;
            InputReadOnly = false;

            TelaConsultaVisivel = false;
            TelaModificacoesVisivel = true;

            OpcoesCadastroVisivel = true;
            OpcoesAlteracoesVisivel = false;

            Codigo = 0;
            DescricaoProduto = "";
            ValorProduto = null;
        }

        private void VoltaTelaConsulta()
        {
            if (Incluindo)
            {
                TelaConsultaVisivel = true;
                TelaModificacoesVisivel = false;

                OpcoesCadastroVisivel = false;
                OpcoesAlteracoesVisivel = false;
            }
            else if (Alterando)
            {
                if (!InputReadOnly)
                {
                    InputReadOnly = true;

                    TelaConsultaVisivel = false;
                    TelaModificacoesVisivel = true;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = true;
                }
                else
                {
                    Incluindo = true;
                    Alterando = false;

                    TelaConsultaVisivel = true;
                    TelaModificacoesVisivel = false;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = false;
                }

            }
        }

        private async void CarregaProdutos()
        {
            Produtos = new ObservableCollection<Produto>(await Connection.db.Table<Produto>().ToListAsync());
        }

        private async void ConfirmaModificacaoProduto()
        {
            if (DescricaoProduto.Trim() == "") { return; }
            if (ValorProduto == null) { return; }

            if (Incluindo)
            {
                Produto produto = new Produto();
                produto.Descricao = DescricaoProduto;
                produto.Valor = double.Parse(ValorProduto);

                await Connection.db.InsertAsync(produto);
            }
            else if (Alterando)
            {
                if (Codigo <= 0) { return; }

                Produto produto = new Produto();
                produto.Codigo = Codigo;
                produto.Descricao = DescricaoProduto;
                produto.Valor = double.Parse(ValorProduto);

                await Connection.db.UpdateAsync(produto);
            }

            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
            CarregaProdutos();
        }

        private async void DeletaProduto()
        {
            if (Codigo <= 0) { return; }

            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo deletar o produto \"" + Produtos.FirstOrDefault(p => p.Codigo == Codigo).Descricao + "\"?");
            IDmsg++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaProdutoResposta{IDmsg}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);

            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaProdutoResposta{IDmsg}", async (sender, resposta) =>
            {
                if (resposta == true)
                {
                    await Connection.db.DeleteAsync(Produtos.FirstOrDefault(p => p.Codigo == Codigo));

                    TelaConsultaVisivel = true;
                    TelaModificacoesVisivel = false;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = false;

                    CarregaProdutos();
                }
            });
        }

        private async void AlteraProduto()
        {
            if (Codigo <= 0) { return; }

            Incluindo = false;
            Alterando = true;

            InputReadOnly = false;

            TelaConsultaVisivel = false;
            TelaModificacoesVisivel = true;

            OpcoesCadastroVisivel = true;
            OpcoesAlteracoesVisivel = false;
        }
    }
}
