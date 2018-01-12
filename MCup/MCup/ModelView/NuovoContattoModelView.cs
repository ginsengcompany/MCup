using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;
using XLabs;

namespace MCup.ModelView
{
    public class NuovoContattoModelView:INotifyPropertyChanged

    {
    public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged

    private Contatto contatto; //Oggetto che astrae l'utenza del cliente

    private string confermaPassword,
        nameErrorTextNome,
        nameErrorTextCognome,
        nameErrorTextCodice,
        nameErrorTextPassword,
        nameErrorTextConfermaPassword;

    public string nameerrortextdatanascita, nameErrorTextLuogoNascita, nameErrorTextProvincia;

    public ICommand registraNuovoContatto { protected set; get; } //Command per il tentativo di registrazione dell'utenza
        public ICommand annullaRegistrazioneNuovoContatto { protected set; get; }

    public string CodiceFiscaleNuovoContatto //Proprietà relativa al campo codice fiscale
    {
        get { return contatto.codice_fiscale; }
        set
        {
            OnPropertyChanged();
            contatto.codice_fiscale = value;
        }
    }

    public string NomeNuovoContatto //Proprietà relativa al campo nome
    {
        get { return contatto.nome; }
        set
        {
            OnPropertyChanged();
            contatto.nome = value;
        }
    }

    public string CognomeNuovoContatto //Proprietà relativa al campo cognome
    {
        get { return contatto.cognome; }
        set
        {
            OnPropertyChanged();
            contatto.cognome = value;
        }
    }

    public Char sceltaSesso //Proprietà relativa al campo sesso
    {
        get { return contatto.sesso; }
        set
        {
            OnPropertyChanged();
            contatto.sesso = value;
        }
    }

    public string Data_nascitaNuovoContatto //Proprietà relativa al campo data di nascita
    {
        get { return contatto.data_nascita; }
        set
        {
            OnPropertyChanged();
            contatto.data_nascita = value;
        }
    }

    public string Luogo_nascitaNuovoContatto //Proprietà relativa al campo luogo di nascita
    {
        get { return contatto.luogo_nascita; }
        set
        {
            OnPropertyChanged();
            contatto.luogo_nascita = value;
        }
    }

    public string ProvinciaNuovoContatto
    {
        get { return contatto.provincia; }
        set
        {
            OnPropertyChanged();
            contatto.provincia = value;
        }
    }



    public string NameErrorTextNome
    {
        get { return nameErrorTextNome; }
        set
        {
            OnPropertyChanged();
            nameErrorTextNome = value;
        }
    }

    public string NameErrorTextCognome
    {
        get { return nameErrorTextCognome; }
        set
        {
            OnPropertyChanged();
            nameErrorTextCognome = value;
        }
    }

    public string NameErrorTextCodice
    {
        get { return nameErrorTextCodice; }
        set
        {
            OnPropertyChanged();
            nameErrorTextCodice = value;
        }
    }

    public string NameErrorTextDataNascita
    {
        get { return nameerrortextdatanascita; }
        set
        {
            OnPropertyChanged();
            nameerrortextdatanascita = value;
        }
    }

    public string NameErrorTextLuogoNascita
    {
        get { return nameErrorTextLuogoNascita; }
        set
        {
            OnPropertyChanged();
            nameErrorTextLuogoNascita = value;
        }
    }

    public string NameErrorTextProvincia
    {
        get { return nameErrorTextProvincia; }
        set
        {
            OnPropertyChanged();
            nameErrorTextProvincia = value;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
    public NuovoContattoModelView()
    {
        contatto = new Contatto(); //Crea un utenza vuota
        var token = App.Current.Properties["tokenLogin"].ToString();
            annullaRegistrazioneNuovoContatto = new Command(() =>
            {
                App.Current.MainPage = new MenuPrincipale(); 

            });

            registraNuovoContatto = new Command(async () =>
        {
            //Imposta gli errori ad una stringa vuota
            NameErrorTextNome = String.Empty;
            NameErrorTextCognome = String.Empty;
            NameErrorTextCodice = String.Empty;
            
            /*
             * variabile locale utilizzata per verificare se l'utente ha inserito i campi obbligatori per effettuare il tentativo di registrazione.
             * Questa viene inizializzata a true supponendo a priori che l'utente abbia inserito tutti i campi correttamente.
             * Ogni qualvota che uno dei seguenti controlli non andasse a buon fine viene assegnata a questa variabile il valore false
             */
            bool controllPass = true;
            if (string.IsNullOrEmpty(NomeNuovoContatto)) //Controlla se il campo nome è vuoto o null
            {
                NameErrorTextNome = "Attenzione, campo obbligatorio";
                controllPass = false;
            }
            else
            {
                NameErrorTextNome = string.Empty;
            }
            if (string.IsNullOrEmpty(CognomeNuovoContatto)) //Controlla se il campo cognome è vuoto o null
            {
                NameErrorTextCognome = "Attenzione, campo obbligatorio";
                controllPass = false;
            }
            else
            {
                NameErrorTextCognome = string.Empty;
            }
            if (string.IsNullOrEmpty(CodiceFiscaleNuovoContatto)) //Controlla se il campo codice fiscale è vuoto o null
            {
                NameErrorTextCodice = "Attenzione, campo obbligatorio";
                controllPass = false;
            }
            else
            {
                NameErrorTextCodice = string.Empty;
            }
            if (string.IsNullOrEmpty(Data_nascitaNuovoContatto)) //Controlla se il campo data di nascita è vuoto o null
            {
                NameErrorTextDataNascita = "Attenzione, campo obbligatorio";
                controllPass = false;
            }
            else
            {
                NameErrorTextDataNascita = string.Empty;
            }
            if (string.IsNullOrEmpty(Luogo_nascitaNuovoContatto)) //Controlla se il campo luogo di nascita è vuoto o null
            {
                NameErrorTextLuogoNascita = "Attenzione, campo obbligatorio";
                controllPass = false;
            }
            else
            {
                NameErrorTextLuogoNascita = string.Empty;
            }
            if (contatto.sesso.Equals(' ')) //Controlla se il campo sesso è vuoto
            {
                controllPass = false;
            }

            if (string.IsNullOrEmpty(ProvinciaNuovoContatto)) //Controlla se il campo provincia è vuoto, null o length è diverso da due
            {
                NameErrorTextProvincia = "Attenzione, campo obbligatorio ";
                controllPass = false;
            }
          
                else if (ProvinciaNuovoContatto.Length<2)
                {
                    NameErrorTextProvincia = "Attenzione, provincia non valida ";
                    controllPass = false;

                }
                else if(ProvinciaNuovoContatto.Length > 2)
                {
                    ProvinciaNuovoContatto = ProvinciaNuovoContatto.Substring(0, 2);
                    controllPass = true;
                }
            else
            {
                NameErrorTextProvincia = string.Empty;
            }
            if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
            {
                string giorno, mese, anno;
                giorno = contatto.data_nascita.Substring(3, 2);
                mese = contatto.data_nascita.Substring(0, 2);
                anno = contatto.data_nascita.Substring(6,4);
                contatto.data_nascita = giorno + "/" + mese + "/" + anno;
                REST<Contatto, ResponseRegistrazione> connessioneNuovoContatto = new REST<Contatto, ResponseRegistrazione>();
                try
                {
                    await connessioneNuovoContatto.PostJson(URL.AggiungiNuovoContatto, contatto, token);
                    await App.Current.MainPage.DisplayAlert("Evvai", "utente inserito con successo", "ok");
                    App.Current.MainPage = new MenuPrincipale();
                }
                catch (Exception)
                {
                  await  App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
                }                
            }
        });
    }
    }
}
