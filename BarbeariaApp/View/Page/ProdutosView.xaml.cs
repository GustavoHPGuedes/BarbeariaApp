using BarbeariaApp.ViewModel.Page;
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
    public partial class ProdutosView : ContentPage
    {
        ProdutoViewModel viewModel = new ProdutoViewModel();
        public ProdutosView()
        {
            this.BindingContext = viewModel;
            InitializeComponent();
        }

        private void ModoficaProduto(object sender, EventArgs args)
        {
            if (sender is ImageButton)
            {
                ImageButton botao = (ImageButton)sender;
                if (int.TryParse(botao.ClassId, out int codigo))
                {
                    viewModel.TelaConsultaVisivel = false;
                    viewModel.TelaModificacoesVisivel = true;

                    viewModel.OpcoesCadastroVisivel = false;
                    viewModel.OpcoesAlteracoesVisivel = true;

                    viewModel.Codigo = codigo;
                    viewModel.DescricaoProduto = viewModel.Produtos.FirstOrDefault(p => p.Codigo == codigo).Descricao;
                    viewModel.ValorProduto = viewModel.Produtos.FirstOrDefault(p => p.Codigo == codigo).Valor.ToString();
                    viewModel.InputReadOnly = true;
                }
            }   
        }
    }
}