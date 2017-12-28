using Rg.Plugins.Popup.Pages;
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
    public partial class PopUpImage : PopupPage
    {

        public PopUpImage(string imagename)
        {
            InitializeComponent();
            popImage.Source = imagename;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        //Metodo per l'animazione del figlio nella pagina di popup
        //Viene invocato dopo la fine dell'animazione custom
        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        //Metodo per l'animazione del figlio nella pagina di popup
        //Viene invocato prima dell'inizio dell'animazione custom
        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }

        protected override bool OnBackButtonPressed()
        {
            //Rende hide il popup
            return true;
        }

        //Viene invocato quando si clicca il background del popup
        protected override bool OnBackgroundClicked()
        {
            //ritorna il valore di default - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }

    }
}