﻿using MCup.Database.Data;
using MCup.Database.Models;
using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaStrutture : ContentPage
	{   // Le cose commentate le ho messe perchè ora non sono implementabili dato che non abbiamo i servizi
        // List<Struttura> listaStrutture = new List<Struttura>();
        List<Struttura> listaDiProva = new List<Struttura>();
        REST<Struttura> connessione = new REST<Struttura>();
        public ListaStrutture ()
		{
			InitializeComponent ();
            riempimentoStruttura();
            
		}
        public async void riempimentoStruttura()
        {
            //  CreazioneGrigliaStrutture grigliaStruttura1 = new CreazioneGrigliaStrutture();
            //   grigliaStruttura1.CreazioneGriglia(grigliaStrutture);
            caricamentoPagina.IsRunning = true;
            caricamentoPagina.IsVisible = true;
            listaDiProva = await connessione.GetJson(URL.Strutture);
            ImageSource imgSrc="";

            foreach (var i in listaDiProva)
            {
                imgSrc = Xamarin.Forms.ImageSource.FromStream(
           () => new MemoryStream(Convert.FromBase64String(i.Logo_struttura)));
                i.imgStruttura = imgSrc;
            }
            ListaStruttura.SeparatorColor = Color.Black;
            caricamentoPagina.IsRunning = false;
            caricamentoPagina.IsVisible = false;
            ListaStruttura.ItemsSource = listaDiProva;

        }

        private void ListaStruttura_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int count = StrutturePreferite.GetCountStrutturePreferite();
            Struttura elemTapped = e.Item as Struttura;
            if (count > 0)
            {
                TbStrutturePreferite struttura = new TbStrutturePreferite(elemTapped.Codice_struttura, elemTapped.Nome_struttura);
                StrutturePreferite.UpdateStrutturaPreferita(elemTapped.Nome_struttura, elemTapped.Codice_struttura);
                DisplayAlert("STRUTTURA PREFERITA", "Hai selezionato: " + struttura.NomeStruttura, "OK");
            }
            else
            {
                TbStrutturePreferite struttura = new TbStrutturePreferite(elemTapped.Codice_struttura, elemTapped.Nome_struttura);
                StrutturePreferite.InserisciStrutturaPreferita(struttura);
                Navigation.PushModalAsync(new MenuPrincipale());
            }  
        }
    }
}