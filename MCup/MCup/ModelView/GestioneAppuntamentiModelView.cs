using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;

namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Contacts contacts;
        private List<Contatto> contatti = new List<Contatto>();
        private List<Appuntamento> appuntamenti = new List<Appuntamento>();
        private Appuntamento date = new Appuntamento();
        private Boolean visibileLabel = false;
        private Boolean visibile = true;

        public Boolean Visibile
        {
            get { return visibile; }
            set
            {
                OnPropertyChanged();
                visibile = value;
            }
        }
        public Boolean VisibileLabel
        {
            get { return visibileLabel; }
            set
            {
                OnPropertyChanged();
                visibileLabel = value;
            }
        }

        public List<Appuntamento> Appuntamenti
        {
            get { return appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamenti = value;
            }
        }

        public List<Contatto> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Contatto>(value);
            }
        }

        private async void leggiContatti()
        {
            REST<object, Contacts> rest = new REST<object, Contacts>();
            contacts = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            List<Contatto> temp = new List<Contatto>();
            temp.Add(new Contatto
            {
                nome = contacts.nome,
                cognome = contacts.cognome,
                codice_fiscale = contacts.codice_fiscale,
                data_nascita = contacts.data_nascita,
                statocivile = contacts.statocivile,
                codStatoCivile = contacts.codStatoCivile,
                luogo_nascita = contacts.luogo_nascita,
                sesso = contacts.sesso,
                AccountPrimario = true,
                istatComuneNascita = contacts.istatComuneNascita,
                istatComuneResidenza = contacts.istatComuneResidenza,
                nomeCompletoConCodiceFiscale = contacts.nome + " " + contacts.cognome + " " + contacts.codice_fiscale,
                comune_residenza = contacts.comune_residenza,
                telefono = contacts.telefono
            });
            for (int i = 0; i < contacts.contatti.Count; i++)
            {
                contacts.contatti[i].nomeCompletoConCodiceFiscale = contacts.contatti[i].nome + " " + contacts.contatti[i].cognome + " " + contacts.contatti[i].codice_fiscale;
                temp.Add(contacts.contatti[i]);
            }
            Contatti = temp;
        }

        public GestioneAppuntamentiModelView()
        {
            leggiContatti();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void autoCompila(Contatto elementSelected)
        {
            date.assistito = elementSelected;
            await invioDatiAssistito();
        }

        public async Task invioDatiAssistito()
        {
            try
            {
                var invioContatto = date.assistito;
                REST<Contatto, Appuntamento> connessione = new REST<Contatto, Appuntamento>();
                Appuntamenti = await connessione.PostJsonList(URL.appuntamenti, invioContatto,App.Current.Properties["tokenLogin"].ToString());
                if(Appuntamenti.Count==0)
                {
                    Visibile = false;
                    VisibileLabel = true;
                }
                else
                {
                    Visibile = true;
                    VisibileLabel = false;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }
    }
}
