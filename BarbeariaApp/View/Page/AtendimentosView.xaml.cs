using BarbeariaApp.Model;
using BarbeariaApp.ViewModel.Page;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarbeariaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AtendimentosView : ContentPage
	{
        AtendimentosViewModel viewModel = new AtendimentosViewModel();
        public AtendimentosView ()
		{
            this.BindingContext = viewModel;
            InitializeComponent ();
		}

        private void ModoficaAtendimento(object sender, EventArgs args)
        {
            if (sender is ImageButton)
            {
                ImageButton botao = (ImageButton)sender;
                if (int.TryParse(botao.ClassId, out int codigo))
                {
                    ModificaAtendimento(codigo);
                }
            }
        }

        private async void ModificaAtendimento(int codigo)
        {
            viewModel.TelaConsultaVisivel = false;
            viewModel.TelaModificacoesVisivel = true;

            viewModel.OpcoesCadastroVisivel = false;
            viewModel.OpcoesAlteracoesVisivel = true;

            viewModel.Codigo = codigo;
            viewModel.ClienteAtendimentoSelecionado = viewModel.AtendimentosDoDia.FirstOrDefault(p => p.Codigo == codigo).Cliente;

            List<Produto> produtos = await Connection.db.Table<Produto>().ToListAsync();
            string[] codigoProdutos = viewModel.AtendimentosDoDia.FirstOrDefault(p => p.Codigo == codigo).CodigoProdutos.Split(',');
            viewModel.ProdutosSelecionados.Clear();
            foreach (string codigoProduto in codigoProdutos)
            {
                if (int.TryParse(codigoProduto, out int cod))
                {
                    viewModel.ProdutosSelecionados.Add(produtos.FirstOrDefault(p => p.Codigo == cod));
                }
            }

            List<Servico> servicos = await Connection.db.Table<Servico>().ToListAsync();
            string[] codigoServicos = viewModel.AtendimentosDoDia.FirstOrDefault(p => p.Codigo == codigo).CodigoServicos.Split(',');
            viewModel.ServicosSelecionados.Clear();
            foreach (string codigoServico in codigoServicos)
            {
                if (int.TryParse(codigoServico, out int cod))
                {
                    viewModel.ServicosSelecionados.Add(servicos.FirstOrDefault(p => p.Codigo == cod));
                }
            }

            viewModel.AtualizaAlturaGrids();
            viewModel.ObservacaoAtendimentoSelecionado = viewModel.AtendimentosDoDia.FirstOrDefault(p => p.Codigo == codigo).Observacao;
            viewModel.InputReadOnly = true;
        }

        private void DeletaServico(object sender, EventArgs args)
        {
            if (sender is ImageButton)
            {
                ImageButton botao = (ImageButton)sender;
                if (int.TryParse(botao.ClassId, out int codigo))
                {
                    viewModel.DeletaServico(codigo);
                }
            }
        }

        private void DeletaProduto(object sender, EventArgs args)
        {
            if (sender is ImageButton)
            {
                ImageButton botao = (ImageButton)sender;
                if (int.TryParse(botao.ClassId, out int codigo))
                {
                    viewModel.DeletaProduto(codigo);
                }
            }
        }
    }
}