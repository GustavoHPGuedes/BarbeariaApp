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
    public partial class AgendaView : ContentPage
    {
        AgendaViewModel viewModel = new AgendaViewModel();
        public AgendaView()
        {
            this.BindingContext = viewModel;
            InitializeComponent();
        }

        private void SelecionarData(object sender, EventArgs args)
        {
            if (viewModel.Incluindo)
            {
                viewModel.DataSelecionada = DateTime.Today;
            }
            DataEntradaPicker.Focus();
        }

        private void SelecionarHora(object sender, EventArgs args)
        {
            if (viewModel.Incluindo)
            {
                viewModel.HoraSelecionada = DateTime.Now.TimeOfDay;
            }
            
            TimePicker.Focus();
        }

        private void ModificaAgendamento(object sender, EventArgs args)
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
                    viewModel.ClienteAgendamentoSelecionado = viewModel.AgendamentosDoDia.FirstOrDefault(p => p.Codigo == codigo).Cliente;
                    viewModel.DataSelecionada = viewModel.AgendamentosDoDia.FirstOrDefault(p => p.Codigo == codigo).Data;
                    viewModel.HoraSelecionada = viewModel.AgendamentosDoDia.FirstOrDefault(p => p.Codigo == codigo).Horario;
                    viewModel.InputReadOnly = true;
                }
            }
        }
    }
}