using MCup.CustomPopUp;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoContattiApp : ContentPage
	{
		public InfoContattiApp ()
		{
			InitializeComponent ();
		}

        private async void TapGestureRecognizer_TappedTermini(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopUpTerminiServizioRegistrazione(null));
        }

        private void TapGestureRecognizer_TappedContattaciEmail(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri($"mailto:{"ecuptservice.mail@gmail.com"}"));
        }
    }
}