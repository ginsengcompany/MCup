using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.ModelView;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.Model
{
    public class VisualizzaAppuntamenti : ObservableCollection<AppuntamentoPrestazioneProposto>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string codiceFiscale { get; set; }
        public string dataEmissioneRicetta { get; set; }
        public bool scaduto { get; set; } = true;


        public void ControlloAccettazione()
        {
            for (int i = 0;  i < this.Count;  i++)
            {
                if (!string.IsNullOrEmpty(this[i].dataaccettazione))
                {
                    scaduto = false;
                }
                
            }
        }

        public void ControlloCodiceMancante()
        {
            for (int i = 0;  i< this.Count;  i++)
        {
            if (string.IsNullOrEmpty(LongName))
            {
                switch (this[i].tipoprenotazione)
                {
                    case "A":
                        LongName = "Prestazione Ambulatoriale";
                        break;
                    case "I":
                        LongName = "Prestazione Interna";
                        break;
                    case "L":
                        LongName = "Prestazione di tipo Alpi";
                        break;
                    case "D":
                        LongName = "Prestazione di tipo Domiciliare";
                        break;
                    case "P":
                        LongName = "Prestazione di tipo PACC";
                        break;

                }
            }
        

        }
        }

        public void Scaduto()
        {
            for (int i = 0; i < this.Count; i++)
            {
                DateTime data_appuntamento = DateTime.ParseExact(this[i].dataAppuntamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dataOdierna = DateTime.Today;
                if ((data_appuntamento - dataOdierna).TotalDays < 0)
                {
                    scaduto = false;
                    break;
                }
                else
                {
                    scaduto = true;
                }
            }
        }
        public Assistito contatto { get; set; }

        public ICommand EliminaAppuntamento
        {
            get
            {
                return new Command(async () =>
                {
                    await EliminazioneAppuntamento();
                });
            }
        }

        //Comando che richiama il metodo dello spostamento di un appuntamento
        public ICommand SpostaAppuntamento
        {
            get
            {
                return new Command(async () =>
                {
                    await SpostaAppuntamentoMethod();
                });
            }
        }

        public async Task EliminazioneAppuntamento()
        {
            var messDisplay = "";
            try
            {
                DateTime dataEmissione = DateTime.ParseExact(dataEmissioneRicetta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dataOdierna = DateTime.Today;
                PaginaAppuntamentiModelView pagina;
                if ((dataOdierna - dataEmissione).TotalDays > 30)
                {
                    messDisplay = "Sei sicuro di voler annullare la prenotazione?\nse confermi non sarà più possibile prenotare con questa impegnativa, inquanto la data di emissione dell'impegnativa ha superato i 30 giorni utili per utilizzarla";
                }
                else
                    messDisplay = "Sei sicuro di voler annullare la prenotazione?";
                var esitoDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione", messDisplay, "si", "no");
                REST<AppuntamentoProposto, ResponseAnnullaImpegnativa> connessioneAnnullamentoImpegnativa = new REST<AppuntamentoProposto, ResponseAnnullaImpegnativa>();
                List<Header> headers = new List<Header>();
                headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                headers.Add(new Header("struttura", "150907"));
                if (esitoDisplayAlert)
                {

                    AppuntamentoProposto appuntamentoSelezionato = new AppuntamentoProposto();
                    appuntamentoSelezionato.codiceImpegnativa = this.LongName;
                    appuntamentoSelezionato.assistito = new Assistito();
                    appuntamentoSelezionato.assistito.codice_fiscale = this.codiceFiscale;
                    ResponseAnnullaImpegnativa response = await connessioneAnnullamentoImpegnativa.PostJson(SingletonURL.Instance.getRotte().annullaImpegnativa,
                        appuntamentoSelezionato, headers);
                    if (connessioneAnnullamentoImpegnativa.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Attenzione " + (int) connessioneAnnullamentoImpegnativa.responseMessage,
                            connessioneAnnullamentoImpegnativa.warning, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione ",
                            connessioneAnnullamentoImpegnativa.warning, "OK");
                        App.Current.MainPage = new MenuPrincipale("Appuntamenti");
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FormatException)
                {
                    await App.Current.MainPage.DisplayAlert("Mcup", "Impossibile recuperare la data di emissione dell'impegnativa", "ok");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Mcup", "Connessione non riuscita", "ok");
            }
        }

        //Metodo che implementa lo spostamento di un appuntamento
        public async Task SpostaAppuntamentoMethod()
        {
            try
            {
                List<Header> headers = new List<Header>();
                headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                headers.Add(new Header("struttura", "150907"));
                REST<object, string> connessioneSpostamento = new REST<object, string>();
                string messaggioDalServer = await connessioneSpostamento.getString(SingletonURL.Instance.getRotte().spostamentoPrenotazione,
                        headers);
                if (connessioneSpostamento.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneSpostamento.responseMessage, connessioneSpostamento.warning, "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDalServer, "ok");
                }

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
            }

        }

    }
}
