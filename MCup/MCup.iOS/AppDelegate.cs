using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Com.OneSignal;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xfx;

namespace MCup.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            XfxControls.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0, 57, 100);
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {TextColor = UIColor.White});
            UIBarButtonItem.Appearance.TintColor = UIColor.LightTextColor;
            DatePickerRenderer.Appearance.TintColor= UIColor.Black;
            return base.FinishedLaunching(app, options);
        }
      
    }
}
