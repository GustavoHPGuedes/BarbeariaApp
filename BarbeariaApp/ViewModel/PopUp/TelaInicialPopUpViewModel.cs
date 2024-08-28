using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BarbeariaApp.ViewModel.PopUp
{
    public class TelaInicialPopUpViewModel : BaseViewModel
    {
        private string _VersaoApp = "";
        public string VersaoApp
        {
            get => _VersaoApp;
            set
            {
                _VersaoApp = value;
                OnPropertyChanged();
            }
        }

        public Command CommandEnviaBackup { get; set; }
        public Command CommandRestauraBackup { get; set; }
        public Command CommandFechaPopUp { get; set; }

        public TelaInicialPopUpViewModel()
        {
            VersaoApp = $"Versão do App: {VersionTracking.CurrentVersion}";

            CommandEnviaBackup = new Command(EnviaBackup);
            CommandRestauraBackup = new Command(RestauraBackup);
            CommandFechaPopUp = new Command(FechaPopUp);
        }

        private async void EnviaBackup()
        {
            string dbPath;
            if (Preferences.Get("CaminhoBD", "") != "")
            {
                dbPath = Preferences.Get("CaminhoBD", "");
            }
            else
            {
                dbPath = Path.Combine(FileSystem.AppDataDirectory, "db.db");
            }

            if (File.Exists(dbPath))
            {
                var request = new ShareFileRequest();

                request.Title = "Compartilhar backup";
                request.File = new ShareFile(dbPath);
                await Share.RequestAsync(request);
            }
        }

        private async void RestauraBackup()
        {
            try
            {
                FileResult result = await FilePicker.PickAsync();

                if (result is null) { return; }
                string extensao = result.FileName.Substring(result.FileName.Length - 3, 3);
                if (extensao == ".db" || extensao == "bin")
                {
                    Preferences.Set("CaminhoBD", result.FullPath);
                    Thread.Sleep(1000);
                    MessagingCenter.Send(this, "SairApp");
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        private async void FechaPopUp()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
