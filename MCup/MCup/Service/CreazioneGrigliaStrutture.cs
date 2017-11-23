using MCup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.Service
{
    class CreazioneGrigliaStrutture
    {
        //   Grid grigliaStruttureOspedaliere = new Grid();
        List<Struttura> listaAppoggio = new List<Struttura>();
        REST<Struttura> connessione = new REST<Struttura>();
        string url = "http://192.168.125.14/servizitemporanei/strutturecup.php";

        public async void CreazioneGriglia(Grid grigliaStruttureOspedaliere)
        {
            int riga = 0;
            int colonna = 0;
            string immagineDiLogo = "";
            listaAppoggio = await connessione.GetJson(url);
            foreach(var i in listaAppoggio)
            {
                switch (i.Nome)
                {
                    case "Cardarelli":
                        immagineDiLogo = "CardarelliLogo.jpg";
                        break;
                    case "Pineta grande":
                        immagineDiLogo = "PinetaGrandeLogo.jpg";
                        break;
                    case "Ospedale del mare":
                        immagineDiLogo = "ospedaledelmare.jpg";
                        break;
                }
                var immagineLogo = new Image
                {
                    Source = immagineDiLogo,
                    HeightRequest = 80,
                    WidthRequest = 80,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                var stackLista = new StackLayout
                {
                    Orientation= StackOrientation.Horizontal,
                    HorizontalOptions= LayoutOptions.Center,
                    VerticalOptions= LayoutOptions.Center
                };
                var stackListaGrande = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                stackLista.Children.Add(immagineLogo);

            }


        }
    }
}















      /*  for(int j=0;j<listaAppoggio.Count;j++)
            {
                grigliaStruttureOspedaliere.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                grigliaStruttureOspedaliere.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            }

            foreach (var i in listaAppoggio)
            {
                var labelNomeCodiceStruttura = new Label
                {
                    Text = i.Nome,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions= LayoutOptions.Center,
                    FontSize=20
                };
                var labelCodiceStruttura = new Label
                {
                    Text = "codice: "  +  i.Codice,
                    TextColor = Color.Black,
                    FontAttributes=FontAttributes.Bold,
                    WidthRequest= 100,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 20
                };
                var stackCodiciNomiOrizzontale = new StackLayout
                {
                    Orientation= StackOrientation.Horizontal,
                    HorizontalOptions= LayoutOptions.Center,
                    VerticalOptions= LayoutOptions.Center
                };
                var stackCodiciOrizzontaleGrande = new StackLayout
                {
                    Orientation= StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                var stackCodiciOrizzontaleGrandeContenitore = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Padding=20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                stackCodiciNomiOrizzontale.Children.Add(labelNomeCodiceStruttura);
                stackCodiciOrizzontaleGrande.Children.Add(labelCodiceStruttura);
                stackCodiciOrizzontaleGrande.Children.Add(stackCodiciNomiOrizzontale);
                stackCodiciOrizzontaleGrandeContenitore.Children.Add(stackCodiciOrizzontaleGrande);
                switch(i.Nome)
                {
                    case "Cardarelli":
                        immagineDiLogo = "CardarelliLogo.jpg";
                        break;
                    case "Pineta grande":
                        immagineDiLogo = "PinetaGrandeLogo.jpg";
                        break;
                    case "Ospedale del mare":
                        immagineDiLogo = "ospedaledelmare.jpg";
                        break;
                }
                var immagineLogo = new Image
                {
                    Source = immagineDiLogo,
                    HeightRequest=80,
                    WidthRequest=80,
                    HorizontalOptions=LayoutOptions.Center,
                    VerticalOptions= LayoutOptions.Center,
                };
                grigliaStruttureOspedaliere.Children.Add(immagineLogo, colonna, riga);
                grigliaStruttureOspedaliere.Children.Add(stackCodiciOrizzontaleGrandeContenitore, colonna+1,riga);
                grigliaStruttureOspedaliere.ColumnSpacing = 5;
                grigliaStruttureOspedaliere.RowSpacing = 2;
                riga++;
                
            }
        }*/