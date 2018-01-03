using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using MCup.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.CustomPopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupInfoScan : PopupPage
    {
        private string codiceRicetta;
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        double x, y;
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 1.2;
        private const double OVERSHOOT = 0.15;


        public PopupInfoScan(string imgName)
        {
            InitializeComponent();
            imgInfo.Source = imgName;
            var imagecontainer = new ContentView();
            Content = imgInfo;
            /*  var pinchGesture = new PinchGestureRecognizer();
              pinchGesture.PinchUpdated += OnPinchUpdated;
              imagecontainer.GestureRecognizers.Add(pinchGesture);*/
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += PanGestureRecognizer_OnPanUpdated;
            imagecontainer.GestureRecognizers.Add(panGesture);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;
            CloseImage.Rotation = 30;
            CloseImage.Scale = 0.3;
            CloseImage.Opacity = 0;
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var TaskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            }, start: currentHeight, end: 400, finished: async (d, b) =>
            {
                await Task.Delay(300);
                TaskSource.TrySetResult(true);
            });
            await TaskSource.Task;
        }
     

        protected override bool OnBackgroundClicked()
        {
            closeAllPopup();
            return false;
        }
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            closeAllPopup();
        }
        private async void closeAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }

       

        private void Handle_Tapped(object sender, EventArgs e)
        {
            if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.05;
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }

      

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    Content.TranslationX = Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - 0));
                    Content.TranslationY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - 0));
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;
            }
        }

      
    }
}