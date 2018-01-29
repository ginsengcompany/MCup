using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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


        public PopupInfoScan(string imgName)
        {
            InitializeComponent();
            imgInfo.Source = imgName;
            Content = FrameContainer;

            //FrameContainer.HeightRequest = -1;
           CloseWhenBackgroundIsClicked = true;
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            await Task.WhenAll(
                imgInfo.FadeTo(0));

            FrameContainer.Animate("HideAnimation", d =>
                {
                    FrameContainer.HeightRequest = d;
                },
                start: currentHeight,
                end: 170,
                finished: async (d, b) =>
                {
                    await Task.Delay(300);
                    taskSource.TrySetResult(true);
                });

            await taskSource.Task;
        }

        protected override bool OnBackgroundClicked()
        {
            closeAllPopup();
            return false;
        }

        private async void closeAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }


        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            closeAllPopup();
        }
    }
}