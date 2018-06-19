﻿using MCup.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.ModelView;
using MCup.Service;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

/*
 * Questa pagina Visualizza all'utente il contenuto della ricetta a cui fa riferimento in base ai dati inseriti nella pagina precedente (FormPrenotazione).
 * Le informazioni che vengono visualizzate sono: codice della ricetta, cognome e nome dell'assistito, codice fiscale del medico e la lista delle prestazioni
 * contenute nella ricetta. La pagina non utilizza l'MWWM.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificaRicettaAsl : ContentPage
    {
        private VerificaRicettaAslModelView ModelViewVerifica;
        private List<Header> headers = new List<Header>();
        private ListView listView = new ListView();
        private Impegnativa ricetta;

        //Costruttore della pagina che inizializza e visualizza le informazioni descritte nel commento della pagina
        public VerificaRicettaAsl(Impegnativa ricetta, Assistito contatto)
        {
            InitializeComponent();
            this.ricetta = ricetta;
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            ModelViewVerifica = new VerificaRicettaAslModelView(ricetta, this, contatto);
            BindingContext = ModelViewVerifica;
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as Reparto;
            ModelViewVerifica.selectedReparto(b);
        }

        protected override bool OnBackButtonPressed()
        {
            AnnullaAppuntamentoSospeso();
            return false;
        }

        private async void AnnullaAppuntamentoSospeso()
        {
            var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione", "Sei sicuro di voler annullare la prenotazione?", "si",
                "no");
            if (responseDisplayAlert)
            {
                REST<object, string> connessioneAnnullamento = new REST<object, string>();
                string messaggioDiAnnullamento = await connessioneAnnullamento.PostJson(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,ricetta,
                    headers);
                await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
            }

        }
    }
}