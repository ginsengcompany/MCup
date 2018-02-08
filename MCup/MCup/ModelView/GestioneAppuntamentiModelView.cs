﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Assistito contatto;
        private Color colore;
        private List<Assistito> contatti = new List<Assistito>();
        private List<AppuntamentoProposto> appuntamenti = new List<AppuntamentoProposto>();
        private AppuntamentoProposto date = new AppuntamentoProposto();
        private AppuntamentoProposto appuntamentoSelezionato = new AppuntamentoProposto();
        private Boolean visibileLabel = false;
        List<AppuntamentoPrestazioneProposto> appunt= new List<AppuntamentoPrestazioneProposto>();
        private Boolean visibile = true;
        private string visi;


        public List<AppuntamentoPrestazioneProposto> Appunt
        {
            get { return appunt; }
            set
            {
                OnPropertyChanged();
                appunt = value;
            }
        }
        public string VisibileL
        {
            get { return visi; }
            set
            {
                OnPropertyChanged();
                visi = value;
            }
        }
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

        public List<AppuntamentoProposto> Appuntamenti
        {
            get { return appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamenti = value;
            }
        }


        public GestioneAppuntamentiModelView( AppuntamentoProposto appuntamentoSelezionato)
        {
            VisibileL = "false";
            this.appuntamentoSelezionato = appuntamentoSelezionato;
            invioDatiAssistito();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task invioDatiAssistito()
        {
            try
            {
               

                Appunt = appuntamentoSelezionato.appuntamenti;
                if (Appuntamenti.Count==0)
                {
                    Visibile = false;
                    VisibileLabel = true;
                }
                else
                {
                    Visibile = true;
                    VisibileL = "true";
                    VisibileLabel = false;
                  /*  foreach (var i in Appuntamenti)
                    {
                        Appunt = i.appuntamenti;
                    }*/
                    
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
