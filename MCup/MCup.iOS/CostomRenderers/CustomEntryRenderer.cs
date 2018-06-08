
using System;
      
using MCup.iOS.CostomRenderers;
using MCup;
      using UIKit;
      using Xamarin.Forms;
      using Xamarin.Forms.Platform.iOS;
using MCup.CostomControls;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MCup.iOS.CostomRenderers{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
                //Below line is useful to give border color 

                Control.Layer.BorderColor =UIColor.Black.CGColor;
                Control.TintColor = UIColor.White;
                Control.Layer.BorderWidth = 2;
                Control.Layer.CornerRadius = 10;
                Control.TextColor = UIColor.White;

            }
        }
    }
}