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
    public class AgendaViewModel : BaseViewModel
    {
        public bool Incluindo = true;
        public bool Alterando = false;
        public int Codigo = 0;

        private int IDmsg = 0;
        MensagemPopUpView mensagemPopUp = new MensagemPopUpView();

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

        private DateTime _DataSelecionadaConsulta = DateTime.Today;
        public DateTime DataSelecionadaConsulta
        {
            get => _DataSelecionadaConsulta;
            set
            {
                _DataSelecionadaConsulta = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DataSelecionada = null;
        public DateTime? DataSelecionada
        {
            get => _DataSelecionada;
            set
            {
                _DataSelecionada = value;
                if (DataSelecionada != null)
                {
                    DataSelecionadaText = $"{((DateTime)DataSelecionada).Day}/{((DateTime)DataSelecionada).Month}/{((DateTime)DataSelecionada).Year}";
                }
                OnPropertyChanged();
            }
        }

        private TimeSpan? _HoraSelecionada = null;
        public TimeSpan? HoraSelecionada
        {
            get => _HoraSelecionada;
            set
            {
                _HoraSelecionada = value;
                if (HoraSelecionada != null)
                {
                    HoraSelecionadaText = $"{((TimeSpan)HoraSelecionada).Hours}:{((TimeSpan)HoraSelecionada).Minutes}";
                }
                OnPropertyChanged();
            }
        }

        private string _ClienteAgendamentoSelecionado = "";
        public string ClienteAgendamentoSelecionado
        {
            get => _ClienteAgendamentoSelecionado;
            set
            {
                _ClienteAgendamentoSelecionado = value;
                OnPropertyChanged();
            }
        }

        private string _HoraSelecionadaText = "SELECIONAR";
        public string HoraSelecionadaText
        {
            get => _HoraSelecionadaText;
            set
            {
                _HoraSelecionadaText = value;
                OnPropertyChanged();
            }
        }

        private string _DataSelecionadaText = "SELECIONAR";
        public string DataSelecionadaText
        {
            get => _DataSelecionadaText;
            set
            {
                _DataSelecionadaText = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Agendamento> _Agendamentos = new ObservableCollection<Agendamento>();
        public ObservableCollection<Agendamento> Agendamentos
        {
            get => _Agendamentos;
            set
            {
                _Agendamentos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Agendamento> _AgendamentosDoDia = new ObservableCollection<Agendamento>();
        public ObservableCollection<Agendamento> AgendamentosDoDia
        {
            get => _AgendamentosDoDia;
            set
            {
                _AgendamentosDoDia = value;
                OnPropertyChanged();
            }
        }

        public Command CommandAdicionaAgendamento { get; set; }
        public Command CommandVoltaTelaConsulta { get; set; }
        public Command CommandAdicionaData { get; set; }
        public Command CommandSubtraiData { get; set; }
        public Command CommandConfirmaModificaoAgendamento { get; set; }
        public Command CommandDeletaAgendamento { get; set; }
        public Command CommandAlteraAgendamento { get; set; }

        public AgendaViewModel()
        {
            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;

            CommandConfirmaModificaoAgendamento = new Command(ConfirmaModificaoAgendamento);
            CommandAdicionaAgendamento = new Command(AdicionaAgendamento);
            CommandVoltaTelaConsulta = new Command(VoltaTelaConsulta);
            CommandAdicionaData = new Command(AdicionaData);
            CommandSubtraiData = new Command(SubtraiData);
            CommandAlteraAgendamento = new Command(AlteraAtendimento);
            CommandDeletaAgendamento = new Command(DeletaAgendamento);

            CarregaAgendamentos();
        }

        private async void AlteraAtendimento()
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

        private async void DeletaAgendamento()
        {
            if (Codigo <= 0) { return; }

            MessagingCenter.Send(this, "TextoMensagem", "Deseja mesmo deletar o agendamento do cliente \"" + ClienteAgendamentoSelecionado + "\"?");
            IDmsg++;
            MessagingCenter.Send(this, "TituloBinding", $"DeletaAgendamentoResposta{IDmsg}");
            await PopupNavigation.Instance.PushAsync(mensagemPopUp);
            MessagingCenter.Subscribe<MensagemPopUpViewModel, bool>(this, $"DeletaAgendamentoResposta{IDmsg}", async (sender, resposta) =>
            {
                if (resposta == true)
                {
                    await Connection.db.DeleteAsync(AgendamentosDoDia.FirstOrDefault(p => p.Codigo == Codigo));

                    TelaConsultaVisivel = true;
                    TelaModificacoesVisivel = false;

                    OpcoesCadastroVisivel = false;
                    OpcoesAlteracoesVisivel = false;

                    CarregaAgendamentos();
                }
            });
        }

        private async void ConfirmaModificaoAgendamento()
        {
            if (ClienteAgendamentoSelecionado.Trim() == "") { return; }
            if (DataSelecionada == null) { return; }
            if (HoraSelecionada == null) { return; }

            if (Incluindo)
            {
                Agendamento agendamento = new Agendamento();
                agendamento.Cliente = ClienteAgendamentoSelecionado;
                agendamento.Data = (DateTime)DataSelecionada;
                agendamento.Horario = (TimeSpan)HoraSelecionada;

                await Connection.db.InsertAsync(agendamento);
            }
            else if (Alterando)
            {
                if (Codigo <= 0) { return; }

                Agendamento agendamento = new Agendamento();
                agendamento.Codigo = Codigo;
                agendamento.Cliente = ClienteAgendamentoSelecionado;
                agendamento.Data = (DateTime)DataSelecionada;
                agendamento.Horario = (TimeSpan)HoraSelecionada;

                await Connection.db.UpdateAsync(agendamento);
            }

            TelaConsultaVisivel = true;
            TelaModificacoesVisivel = false;

            OpcoesCadastroVisivel = false;
            OpcoesAlteracoesVisivel = false;
            CarregaAgendamentos();
        }

        private async void CarregaAgendamentos()
        {
            AgendamentosDoDia = new ObservableCollection<Agendamento>(await Connection.db.Table<Agendamento>().Where(a => a.Data == DataSelecionadaConsulta).ToListAsync());
        }

        private void AdicionaData()
        {
            DataSelecionadaConsulta = DataSelecionadaConsulta.AddDays(1);
            CarregaAgendamentos();
        }

        private void SubtraiData()
        {
            DataSelecionadaConsulta = DataSelecionadaConsulta.AddDays(-1);
            CarregaAgendamentos();
        }

        private void AdicionaAgendamento()
        {
            Incluindo = true;
            Alterando = false;
            InputReadOnly = false;

            TelaConsultaVisivel = false;
            TelaModificacoesVisivel = true;

            OpcoesCadastroVisivel = true;
            OpcoesAlteracoesVisivel = false;

            Codigo = 0;
            ClienteAgendamentoSelecionado = "";
            DataSelecionada = null;
            DataSelecionadaText = "SELECIONAR";
            HoraSelecionada = null;
            HoraSelecionadaText = "SELECIONAR";
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
