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
	public partial class TelaInicialView : ContentPage
	{
		TelaInicialViewModel viewModel = new TelaInicialViewModel();
		public TelaInicialView ()
		{
			this.BindingContext = viewModel;
			InitializeComponent ();
		}
	}
}