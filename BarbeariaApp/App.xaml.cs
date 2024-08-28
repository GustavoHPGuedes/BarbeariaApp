using BarbeariaApp.Model;
using BarbeariaApp.View;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarbeariaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task.Run(async () => await Connection.Open());
            MainPage = new MenuLateralShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
