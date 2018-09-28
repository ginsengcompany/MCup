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
            introPagina();
        }

        private async void LstVideo_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var videoTap = e.Item as Video;
            await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpVideoPlayer(videoTap.link), false);
        }

        private async void introPagina()
        {
            List<Video> listaTemp;
            REST<object, Video> connessione = new REST<object, Video>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header { header = "struttura", value = "150907" });
            listaTemp = await connessione.GetListJson(SingletonURL.Instance.getRotte().listaVideo, headers);
            if (connessione.responseMessage != System.Net.HttpStatusCode.OK)
                await DisplayAlert("Errore",connessione.warning,"OK");
            else
                for(int i=0;i<listaTemp.Count;i++)
                    video.Add(listaTemp[i]);
        }
    }
}