using MCup.Database.Data;
using MCup.Database.Models;
using MCup.Model;
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
    //ModelView della pagina FormPrenotazione, tale classe è utilizzata per implementare il binding con la relativa pagina
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        //Evento che prevede il cambiamento di proprietà all'interno della classe
        public event PropertyChangedEventHandler PropertyChanged;

        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni
        private UtenzaPrenotazione utenza;
        
        //Oggetto che astrae la ricetta NRE
        private Ricetta ricetta;

        //Oggetto che contiene tutte le informazioni della prenotazione che si vuole effettuare
        private FormPrenotazione model;
        

        //Proprietà che definisce il nome dell'utente che sta effettuando la prenotazione
        public string nomeUtente
        {
            get { return utenza.nome; }
            set
            {
                utenza.nome = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il cognome dell'utente che sta effettuando la prenotazione
        public string cognomeUtente
        {
            get { return utenza.cognome; }
            set
            {
                utenza.cognome = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il codice fiscale dell'utente che sta effettuando la prenotazione
        public string codicefiscaleUtente
        {
            get { return utenza.getCodiceFiscale(); }
            set
            {
                utenza.setCodiceFiscale(value);
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il primo codice della ricetta che sta effettuando la prenotazione
        public string codiceUno
        {
            get { return ricetta.codice_uno; }
            set
            {
                ricetta.codice_uno = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il secondo codice della ricetta che sta effettuando la prenotazione
        public string codiceDue
        {
            get { return ricetta.codice_due; }
            set
            {
                ricetta.codice_due = value;
                OnPropertyChanged();
            }
        }

        //Costruttore del ModelView, viene passato come parametro il riferimento alla pagina che lo richiama per poter effettuare una Navigation.pushAsync
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

        //Classe che identifica le prestazioni e se sono state erogate. Questa classe astrae dei possibili dati ricevuti da SOGEI
        private class Prestazioni
        {
            public string prestazione { get; set; }

            public bool erogato { get; set; }

        }

        private class RecvRicetta
        {
            public List<Prestazioni> prestazioni { get; set; }

            public string codice_fiscale_assistito { get; set; }
        }

        //Classe che contiene l'informazione della ricetta il codice fiscale dell'assistito contenuto nella ricetta e le prestazioni
        private class sendRicetta
        {
            public string codice_nre;
            
            public sendRicetta(string nre)
            {
                this.codice_nre = nre;
            }
        }

        //Comando che chiama la funzione asincrona InvioDatiAsync()
        public ICommand InviaRichiesta { protected set; get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Funzione utilizzata per l'invio della richiesta di prenotazione al servizio
        public async Task InvioDatiAsync ()
        {
            if (utenza.getCodiceFiscale().Trim() != "")
            {
                REST<sendRicetta,RecvRicetta> connessione = new REST<sendRicetta,RecvRicetta>();
                sendRicetta nre = new sendRicetta(ricetta.codice_uno.ToString() + ricetta.codice_due.ToString());
                RecvRicetta x = await connessione.PostJson(URL.Ricetta,nre);
                Debug.WriteLine(x.codice_fiscale_assistito);
                for (int j = 0; j < x.prestazioni.Count; j++)
                {
                    Debug.WriteLine(x.prestazioni[j].prestazione);
                    Debug.WriteLine(x.prestazioni[j].erogato);
                }
                List<TbStrutturePreferite> struttura = StrutturePreferite.PrelevaIdStruttura();
                model.metodoPush(this.utenza,nre.codice_nre,struttura[0].id,struttura[0].NomeStruttura);
            }
        }
    }
}
