﻿using Rg.Plugins.Popup.Extensions;
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


        public PopupInfoScan(string codRicetta)
        {
            InitializeComponent();

            
            this.codiceRicetta = codRicetta;
            var imagecontainer = new ContentView();
            Content = imgInfo;
            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            imagecontainer.GestureRecognizers.Add(pinchGesture);
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += PanGestureRecognizer_OnPanUpdated;
            imagecontainer.GestureRecognizers.Add(panGesture);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            imgInfo.Source = codiceRicetta;
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

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);
                Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (currentScale - 1)));
                Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (currentScale - 1)));
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        private void Handle_Tapped(object sender, EventArgs e)
        {
           /* if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5;
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }*/
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