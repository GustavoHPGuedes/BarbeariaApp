using BarbeariaApp.Model;
using BarbeariaApp.View;
using BarbeariaApp.ViewModel.PopUp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace BarbeariaApp.ViewModel.Page
{
    public class AtendimentosViewModel : BaseViewModel
    {
        public bool Incluindo = true;
        public bool Alterando = false;
        public int Codigo = 0;

        MensagemPopUpView mensagemPopUp = new MensagemPopUpView();
        ServicosPopUpView servicosPopUp = new ServicosPopUpView();
        ProdutosPopUpView produtosPopUp = new ProdutosPopUpView();

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

        private bool _InputButtonEnabled = true;
        public bool InputButtonEnabled
        {
            get => _InputButtonEnabled;
            set
            {
                _InputButtonEnabled = value;
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
                InputButtonEnabled = !_InputReadOnly;
                OnPropertyChanged();
            }
        }

        private DateTime _DataSelecionada = DateTime.Now;
        public DateTime DataSelecionada
        {
            get => _DataSelecionada;
            set
            {
                _DataSelecionada = value;
                OnPropertyChanged();
            }
        }

        private string _ClienteAtendimentoSelecionado = "";
        public string ClienteAtendimentoSelecionado
        {
            get => _ClienteAtendimentoSelecionado;
            set
            {
                _ClienteAtendimentoSelecionado = value;
                OnPropertyChanged();
            }
        }

        private string _ObservacaoAtendimentoSelecionado = "";
        public string ObservacaoAtendimentoSelecionado
        {
            get => _ObservacaoAtendimentoSelecionado;
            set
            {
                _ObservacaoAtendimentoSelecionado = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Atendimento> _AtendimentosDoDia = new ObservableCollection<Atendimento>();
        public ObservableCollection<Atendimento> AtendimentosDoDia
        {
            get => _AtendimentosDoDia;
            set
            {
                _AtendimentosDoDia = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Servico> _ServicosSelecionados = new ObservableCollection<Servico>();
        public ObservableCollection<Servico> ServicosSelecionados
        {
            get => _ServicosSelecionados;
            set
            {
                _ServicosSelecionados = value;
                OnPropertyChanged();
            }
        }

        private int _GridServicosAltura;
        public int GridServicosAltura
        {
            get => _GridServicosAltura;
            set
            {
                _GridServicosAltura = value;
                FrameServicosAltura = _GridServicosAltura + 60;
                OnPropertyChanged();
            }
        }

        private int _FrameServicosAltura;
        public int FrameServicosAltura
        {
            get => _FrameServicosAltura;
            set
            {
                _FrameServicosAltura = value;
                OnPropertyChanged();
            }
        }

        private double GanhoDiarioAtendimentos;

        private string _GanhoDiarioAtendimentosText;
        public string GanhoDiarioAtendimentosText
        {
            get => _GanhoDiarioAtendimentosText;
            set
            {
                _GanhoDiarioAtendimentosText = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Produto> _ProdutosSelecionados = new ObservableCollection<Produto>();
        public ObservableCollection<Produto> ProdutosSelecionados
        {
            get => _ProdutosSelecionados;
            set
            {
                _ProdutosSelecionados = value;
                OnPropertyChanged();
            }
        }

        private int _GridProdutosAltura;
        public int GridProdutosAltura
        {
            get => _GridProdutosAltura;
            set
            {
                _GridProdutosAltura = value;
                FrameProdutosAltura = _GridProdutosAltura + 60;
                OnPropertyChanged();
            }
        }

        private int _FrameProdutosAltura;
        public int FrameProdutosAltura
        {
            get => _FrameProdutosAltura;
            set
            {
                _FrameProdutosAltura = value;
                OnPropertyChanged();
            }
        }

        private int MsgRemoverServicoID = 0;
        private int MsgRemoverProdutoID = 0;
        private int ServicoSelecionadoID = 0;
        private int ProdutoSelecionadoID = 0;
        private int MsgDeletarAtendimentoID = 0;

        public Command CommandAdicionaAtendimento { get; set; }
        public Command CommandAlterarAtendimento { get; set; }
        public Command CommandDeletarAtendimento { get; set; }
        public Command CommandVoltaTelaConsulta { get; set; }
        public Command CommandAdicionaData { get; set; }
        public Command CommandSubtraiData { get; set; }
        public Command CommandSelecionaServico { get; set; }
        public Command CommandSelecionaProduto { get; set; }
        public Command CommandConfimaModificacaoAtendimento { get; set; }


        public AtendimentosViewModel()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;

            CommandAdicionaAtendimento = new Command(AdicionaAtendimento);
            CommandAlterarAtendimento = new Command(AlteraAtendimento);
            CommandDeletarAtendimento = new Command(DeletaAtendimento);
            CommandVoltaTelaConsulta = new Command(VoltaTelaConsulta);
            CommandAdicionaData = new Command(AdicionaData);
            CommandSubtraiData = new Command(SubtraiData);
            CommandSelecionaServico = new Command(SelecionaServico);
            CommandSelecionaProduto = new Command(SelecionaProduto);
            CommandConfimaModificacaoAtendimento = new Command(ConfimaModificacaoAtendimento);

            AtualizaAlturaGrids();
            CarregaAtendimentos();
        }

        private async void DeletaAtendimento()
        {
            if (Codigo <= 0) { return; }

            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo deletar o atendimento do cliente \"" + ClienteAtendimentoSelecionado + "\"?");
            MsgDeletarAtendimentoID++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaAtendimentoResposta{MsgDeletarAtendimentoID}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);
            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaAtendimentoResposta{MsgDeletarAtendimentoID}", async (sender, resposta) =>
            {
                if (resposta == true)
                {
                    await Connection.db.DeleteAsync(AtendimentosDoDia.FirstOrDefault(p => p.Codigo == Codigo));

                    TelaConsultaVisivel = true;
                    TelaModificacoesVisivel = false;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = false;

                    CarregaAtendimentos();
                }
            });
        }

        private void AlteraAtendimento()
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

        private async void ConfimaModificacaoAtendimento()
        {
            if (ClienteAtendimentoSelecionado.Trim() == "") { return; }

            if (Incluindo)
            {
                Atendimento atendimento = new Atendimento();
                atendimento.Cliente = ClienteAtendimentoSelecionado;
                double valorTotal = 0;

                string codigosServicos = "";
                foreach (Servico servico in ServicosSelecionados)
                {
                    if (codigosServicos != "") { codigosServicos += ","; }
                    codigosServicos += servico.Codigo;
                    valorTotal += servico.Valor;
                }

                string codigosProdutos = "";
                foreach (Produto produto in ProdutosSelecionados)
                {
                    if (codigosProdutos != "") { codigosProdutos += ","; }
                    codigosProdutos += produto.Codigo;
                    valorTotal += produto.Valor;
                }

                atendimento.CodigoServicos = codigosServicos;
                atendimento.CodigoProdutos = codigosProdutos;
                atendimento.Data = DataSelecionada;
                atendimento.Observacao = ObservacaoAtendimentoSelecionado;
                atendimento.ValorTotal = valorTotal;

                await Connection.db.InsertAsync(atendimento);
            }
            else if (Alterando)
            {
                if (Codigo <= 0) { return; }

                Atendimento atendimento = new Atendimento();
                atendimento.Codigo = Codigo;
                atendimento.Cliente = ClienteAtendimentoSelecionado;
                double valorTotal = 0;

                string codigosServicos = "";
                foreach (Servico servico in ServicosSelecionados)
                {
                    if (codigosServicos != "") { codigosServicos += ","; }
                    codigosServicos += servico.Codigo;
                    valorTotal += servico.Valor;
                }

                string codigosProdutos = "";
                foreach (Produto produto in ProdutosSelecionados)
                {
                    if (codigosProdutos != "") { codigosProdutos += ","; }
                    codigosProdutos += produto.Codigo;
                    valorTotal += produto.Valor;
                }

                atendimento.CodigoServicos = codigosServicos;
                atendimento.CodigoProdutos = codigosProdutos;
                atendimento.Data = DataSelecionada;
                atendimento.Observacao = ObservacaoAtendimentoSelecionado;
                atendimento.ValorTotal = valorTotal;

                await Connection.db.UpdateAsync(atendimento);
            }

            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
            CarregaAtendimentos();
        }

        private async void SelecionaServico()
        {
            MessagingCenter.Send(this, "AbrePopUpServicos");
            ServicoSelecionadoID++;
            MessagingCenter.Send(this, "TituloBinding", $"EnviaServicoSelecionado{ServicoSelecionadoID}");
            await PopupNavigation.Instance.PushAsync(servicosPopUp);
            MessagingCenter.Subscribe<ServicosPopUpViewModel, int>(this, $"EnviaServicoSelecionado{ServicoSelecionadoID}", async (sender, codigo) =>
            {
                if (ServicosSelecionados.Where(s => s.Codigo == codigo).Count() <= 0)
                {
                    ServicosSelecionados.Add(await Connection.db.Table<Servico>().FirstOrDefaultAsync(s => s.Codigo == codigo));
                    AtualizaAlturaGrids();
                }
            });
        }

        private async void SelecionaProduto()
        {
            MessagingCenter.Send(this, "AbrePopUpProdutos");
            ProdutoSelecionadoID++;
            MessagingCenter.Send(this, "TituloBinding", $"EnviaProdutoSelecionado{ProdutoSelecionadoID}");
            await PopupNavigation.Instance.PushAsync(produtosPopUp);
            MessagingCenter.Subscribe<ProdutosPopUpViewModel, int>(this, $"EnviaProdutoSelecionado{ProdutoSelecionadoID}", async (sender, codigo) =>
            {
                if (ProdutosSelecionados.Where(p => p.Codigo == codigo).Count() <= 0)
                {
                    ProdutosSelecionados.Add(await Connection.db.Table<Produto>().FirstOrDefaultAsync(s => s.Codigo == codigo));
                    AtualizaAlturaGrids();
                }
            });
        }

        public async void DeletaServico(int codigo)
        {
            if (codigo <= 0) { return; }

            Servico servicoDeletar = ServicosSelecionados.FirstOrDefault(s => s.Codigo == codigo);
            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo remover desse atendimento o serviço \"" + servicoDeletar.Descricao + "\"?");
            MsgRemoverServicoID++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaServicoResposta{MsgRemoverServicoID}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);
            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaServicoResposta{MsgRemoverServicoID}", async (sender, resposta) =>
            {
                if (resposta == true)
                {
                    ServicosSelecionados.Remove(servicoDeletar);
                    AtualizaAlturaGrids();
                }
            });
        }

        public async void DeletaProduto(int codigo)
        {
            if (codigo <= 0) { return; }

            Produto produtoDeletar = ProdutosSelecionados.FirstOrDefault(s => s.Codigo == codigo);
            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo remover desse atendimento o produto \"" + produtoDeletar.Descricao + "\"?");
            MsgRemoverProdutoID++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaProdutoResposta{MsgRemoverProdutoID}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);
            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaProdutoResposta{MsgRemoverProdutoID}", async (sender, resposta) =>
            {
                if (resposta == true)
                {
                    ProdutosSelecionados.Remove(produtoDeletar);
                    AtualizaAlturaGrids();
                }
            });
        }

        private async void CarregaAtendimentos()
        {
            List<Atendimento> atendimentos = await Connection.db.Table<Atendimento>().ToListAsync();
            AtendimentosDoDia = new ObservableCollection<Atendimento>(atendimentos.Where(a => a.Data.Day == DataSelecionada.Day));

            GanhoDiarioAtendimentos = Math.Round(AtendimentosDoDia.Sum(a => a.ValorTotal), 2);
            if (AtendimentosDoDia.Count() > 0 && GanhoDiarioAtendimentos > 0)
            {
                GanhoDiarioAtendimentosText = $"R$ {(GanhoDiarioAtendimentos * 100).ToString().Substring(0, (GanhoDiarioAtendimentos * 100).ToString().Length - 2)},{(GanhoDiarioAtendimentos * 100).ToString().Substring((GanhoDiarioAtendimentos * 100).ToString().Length - 2, 2)}";
            }
            else
            {
                GanhoDiarioAtendimentosText = "R$ 0,00";
            }

        }

        private void AdicionaData()
        {
            DataSelecionada = DataSelecionada.AddDays(1);
            CarregaAtendimentos();
        }

        private void SubtraiData()
        {
            DataSelecionada = DataSelecionada.AddDays(-1);
            CarregaAtendimentos();
        }

        private void AdicionaAtendimento()
        {
            Incluindo = true;
            Alterando = false;
            InputReadOnly = false;

            TelaConsultaVisivel = false;
            TelaModificacoesVisivel = true;

            OpcoesCadastroVisivel = true;
            OpcoesAlteracoesVisivel = false;

            Codigo = 0;
            ClienteAtendimentoSelecionado = "";
            ServicosSelecionados.Clear();
            ProdutosSelecionados.Clear();
            AtualizaAlturaGrids();
            ObservacaoAtendimentoSelecionado = "";
        }

        public void AtualizaAlturaGrids()
        {
            if (ServicosSelecionados.Count * 70 <= 70)
            {
                GridServicosAltura = ServicosSelecionados.Count * 70;
            }
            else
            {
                GridServicosAltura = 112;
            }

            if (ProdutosSelecionados.Count * 70 <= 70)
            {
                GridProdutosAltura = ProdutosSelecionados.Count * 70;
            }
            else
            {
                GridProdutosAltura = 112;
            }
        }

        private void VoltaTelaConsulta()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
        }
    }
}
