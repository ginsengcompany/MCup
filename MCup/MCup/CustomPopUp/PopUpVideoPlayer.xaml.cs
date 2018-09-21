using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.FormsVideoLibrary;
using MCup.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.CustomPopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpVideoPlayer : PopupPage
    {
        public PopUpVideoPlayer(string linkVideo)
        {
            InitializeComponent();
            Content = FrameContainer;
            CloseWhenBackgroundIsClicked = true;
            videoPlayer.Source = VideoSource.FromUri(linkVideo);
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            FrameContainer.Animate("HideAnimation", d => { FrameContainer.HeightRequest = d; },
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

    }
}