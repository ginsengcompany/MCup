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
            imgInfo.Source = imgName;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var TaskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            FrameContainer.Animate("HideAnimation", d =>
             {
                 FrameContainer.HeightRequest = d;
             }, start: currentHeight, end: 170, finished: async (d,b) => {
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

        private async void closeAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }
    }
}