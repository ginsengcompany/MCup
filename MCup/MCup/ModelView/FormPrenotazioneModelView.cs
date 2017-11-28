﻿using MCup.Model;
using MCup.Service;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UtenzaPrenotazione utenza;

        private Ricetta ricetta;

        private FormPrenotazione model;

        private const string url = "http://192.168.125.14:3000/ricetta";

        public string nomeUtente
        {
            get { return utenza.nome; }
            set
            {
                utenza.nome = value;
                OnPropertyChanged();
            }
        }
        public string cognomeUtente
        {
            get { return utenza.cognome; }
            set
            {
                utenza.cognome = value;
                OnPropertyChanged();
            }
        }
        public string codicefiscaleUtente
        {
            get { return utenza.getCodiceFiscale(); }
            set
            {
                utenza.setCodiceFiscale(value);
                OnPropertyChanged();
            }
        }

        public string codiceUno
        {
            get { return ricetta.codice_uno; }
            set
            {
                ricetta.codice_uno = value;
                OnPropertyChanged();
            }
        }

        public string codiceDue
        {
            get { return ricetta.codice_due; }
            set
            {
                ricetta.codice_due = value;
                OnPropertyChanged();
            }
        }

        public FormPrenotazioneModelView(FormPrenotazione Model)
        {
            utenza = new UtenzaPrenotazione();
            ricetta = new Ricetta();
            model = Model;
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.setCodiceFiscale("");
            InviaRichiesta = new Command(async () =>
            {
                await InvioDatiAsync();
                Debug.WriteLine(utenza.nome);
                Debug.WriteLine(utenza.cognome);
                Debug.WriteLine(utenza.getCodiceFiscale());
            });
        }

        private class Prestazioni
        {
            public string prestazione { get; set; }

            public bool erogato { get; set; }
        }

        private class sendRicetta
        {
            public string codice_nre;

            public List<Prestazioni> prestazioni { get; set; }

            public string codice_fiscale_assistito { get; set; }
            
            public sendRicetta(string nre)
            {
                this.codice_nre = nre;
            }
        }

        public ICommand InviaRichiesta { protected set; get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task InvioDatiAsync ()
        {
            if (utenza.getCodiceFiscale().Trim() != "")
            {
                REST<sendRicetta> connessione = new REST<sendRicetta>();
                sendRicetta nre = new sendRicetta(ricetta.codice_uno.ToString() + ricetta.codice_due.ToString());
                sendRicetta x = await connessione.PostJson(url,nre);
                Debug.WriteLine(x.codice_fiscale_assistito);
                for (int j = 0; j < x.prestazioni.Count; j++)
                {
                    Debug.WriteLine(x.prestazioni[j].prestazione);
                    Debug.WriteLine(x.prestazioni[j].erogato);
                }
                model.metodoPush();
            }
            
        }
    }
}
