using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.CustomPopUp;
using MCup.Model;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MCup.Service;

namespace MCup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Help : ContentPage
    {
        private ObservableCollection<Video> video { get; set; }
        public Help()
        {
            InitializeComponent();
            video = new ObservableCollection<Video>();
            lstVideo.ItemsSource = video;
            video.Add (new Video() { nome = "Registrazione", descrizione = "Il video mostra come registrarsi", immagine = "eCUPT.png", link = "http://192.168.125.24:3001/helpApp/Registrazione.mp4" });
        }

        private async void LstVideo_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var videoTap = e.Item as Video;
            await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpVideoPlayer(videoTap.link));
        }

        private async void introPagina()
        {
            List<Video> listaTemp;
            REST<object, VideoHelp> connessione = new REST<object, VideoHelp>();

        }
    }
}