using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.Model
{
   public class Utente: Assistito
   {
       public string username { get; set; }
        public string password { get; set; }
        

        public void salvaCredenzialiAccesso(string user)
       {
           if (!string.IsNullOrWhiteSpace(user))
           {
               this.username = user;
               Application.Current.Properties.Add("username", user);
               Application.Current.SavePropertiesAsync();
           }
       }

       /**
        * Questo metodo permette di recuperare l'username utente dalla memoria interna.
        * */
       public string recuperaUserName()
       {
           string u;
           /**
            * Se è già presente una chiave userName in memoria viene recuperato il campo userName 
            * dal dizionario interno dell'applicazione che è stato in precedenza settatto 
            * al valore inserito in fase di login. 
            * Tale dizionario (Application.Current.Properties) viene usato per memorizzare i dati
            * sul dispositivo.
            * */
           if (Application.Current.Properties.ContainsKey("username"))
           {
               u = Application.Current.Properties["username"].ToString();
           }
           else
           {
               u = null;
           }
           return u;
       }

       /**
        * Questo metodo agisce in caso di aggiornamento da parte dell'utente dell'username.
        * In tal modo viene salvato il nuovo username inserito in memoria del telefono, in 
        * particolare all'interno dell'Application.Current...
        * */
       public void cancellaEdAggiornaUsername(string nuovo)
       {
           this.username = nuovo;
           //Application.Current.Properties.Clear();
           Application.Current.Properties["username"] = nuovo;
           Application.Current.SavePropertiesAsync();
       }
    }

}
