using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.CustomPopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpTerminiServizioRegistrazione : PopupPage
    {
        private Utente utente;

        public PopUpTerminiServizioRegistrazione(Utente utente)
        {
            this.utente = utente;
            InitializeComponent();
            Content = FrameContainer;
            CloseWhenBackgroundIsClicked = true;
        }

        protected async override void OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            FrameContainer.Animate("HideAnimation", d => { FrameContainer.HeightRequest = d; },
                start: currentHeight,
                end: 170,
                finished: async (d, b) =>
                {
                    await Task.Delay(300);
                    taskSource.TrySetResult(true);
                });

            await taskSource.Task;
        }

        protected override bool OnBackgroundClicked()
        {
            closeAllPopup();
            return false;
        }

        private async void closeAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }

        private void BtnDeclina_OnClicked(object sender, EventArgs e)
        {
            closeAllPopup();
        }

        private async void BtnAccetta_OnClicked(object sender, EventArgs e)
        {
            REST<Utente, ResponseRegistrazione>
                rest =
                    new REST<Utente, ResponseRegistrazione>(); //Crea un oggetto rest per effettuare la registrazione da remoto
            utente.Maiuscolo();
            ResponseRegistrazione
                response = await rest.PostJson(SingletonURL.Instance.getRotte().Registrazione,
                    utente); //Effettua una POST che restituisce nella variabile response se la registrazione ha avuto successo

            if (rest.responseMessage != HttpStatusCode.Created)
            {
                await MessaggioConnessione.displayAlert((int) rest.responseMessage, rest.warning);
            }
            else if (response.auth) //Controlla se response contiene un oggetto e che indica che la registrazione è avvenuta con successo
            {
                //Visualizza un display alert che indica all'utente che la registrazione è avvenuta con successo
                await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione effettuata con successo. A breve riceverai una mail per attivare il tuo account", "OK");
                App.Current.MainPage = new NavigationPage(new Login()); //Ritorna alla pagina di login
                closeAllPopup();
            }
            else //Errore imprevisto durante la registrazione
                await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione fallita", "OK");
        }
    }
}