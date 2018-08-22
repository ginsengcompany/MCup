using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
   public class DatiEmail
    {
        public string url { get; set; }
        public string email { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }

        public DatiEmail()
        {
            url = "";
            email = "";
            nome = "";
            cognome = "";
        }
        public DatiEmail(string urlE, string emailE, string nomeE, string cognomeE)
        {
            url = urlE;
            email = emailE;
            nome = nomeE;
            cognome = cognomeE;

        }
    }

    public class Email
    {
        public DatiEmail datiEmail { get; set; }

        public Email(string urlE, string emailE, string nomeE, string cognomeE)
        {
            datiEmail= new DatiEmail(urlE, emailE, nomeE, cognomeE);

        }
    }
}
