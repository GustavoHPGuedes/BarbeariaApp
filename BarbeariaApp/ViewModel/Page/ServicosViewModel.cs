using BarbeariaApp.Model;
using BarbeariaApp.View;
using BarbeariaApp.ViewModel.PopUp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.Page
{
    public class ServicosViewModel : BaseViewModel
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

        private string _DescricaoServico;
        public string DescricaoServico
        {
            get => _DescricaoServico;
            set
            {
                _DescricaoServico = value;
                OnPropertyChanged();
            }
        }

        private string _ValorServico = null;
        public string ValorServico
        {
            get => _ValorServico;
            set
            {
                _ValorServico = value;
                OnPropertyChanged();
            }
        }

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

        public Command CommandTelaAdicionaServico { get; set; }
        public Command CommandVoltaTelaConsulta { get; set; }
        public Command CommandConfirmaModificaoServico { get; set; }
        public Command CommandDeletaServico { get; set; }
        public Command CommandAlteraServico { get; set; }

        public ServicosViewModel()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;

            CommandTelaAdicionaServico = new Command(TelaAdicionaServico);
            CommandVoltaTelaConsulta = new Command(VoltaTelaConsulta);
            CommandConfirmaModificaoServico = new Command(ConfirmaModificacaoServico);
            CommandDeletaServico = new Command(DeletaServico);
            CommandAlteraServico = new Command(AlteraServico);

            CarregaServicos();
        }

        private async void CarregaServicos()
        {
            Servicos = new ObservableCollection<Servico>(await Connection.db.Table<Servico>().ToListAsync());
        }

        private void TelaAdicionaServico()
        {
            Incluindo = true;
            Alterando = false;
            InputReadOnly = false;

            TelaConsultaVisivel = false;
            TelaModificacoesVisivel = true;

            OpcoesCadastroVisivel = true;
            OpcoesAlteracoesVisivel = false;

            Codigo = 0;
            DescricaoServico = "";
            ValorServico = null;
        }

        private void VoltaTelaConsulta()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
        }

        private async void ConfirmaModificacaoServico()
        {
            if (DescricaoServico.Trim() == "") { return; }
            if (ValorServico == null) { return; }

            if (Incluindo)
            {
                Servico Servico = new Servico();
                Servico.Descricao = DescricaoServico;
                Servico.Valor = double.Parse(ValorServico);

                await Connection.db.InsertAsync(Servico);
            }
            else if (Alterando)
            {
                if (Codigo <= 0) { return; }

                Servico Servico = new Servico();
                Servico.Codigo = Codigo;
                Servico.Descricao = DescricaoServico;
                Servico.Valor = double.Parse(ValorServico);

                await Connection.db.UpdateAsync(Servico);
            }

            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
            CarregaServicos();
        }

        private async void DeletaServico()
        {
            if (Codigo <= 0) { return; }

            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo deletar o serviço \"" + Servicos.FirstOrDefault(p => p.Codigo == Codigo).Descricao + "\"?");
            IDmsg++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaServiçoResposta{IDmsg}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);

            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaServiçoResposta{IDmsg}", async (sender, resposta) =>
            {
                if (resposta)
                {
                    await Connection.db.DeleteAsync(Servicos.FirstOrDefault(p => p.Codigo == Codigo));

                    TelaConsultaVisivel = true;
                    TelaModificacoesVisivel = false;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = false;

                    CarregaServicos();
                }
            });
        }

        private void AlteraServico()
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
