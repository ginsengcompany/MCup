﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MCup.CostomControls;
using MCup.Droid.CostomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPickerForms), typeof(CustomPicker))]
namespace MCup.Droid.CostomRenderers
{
    public class CustomPicker : PickerRenderer
    {
        public CustomPicker(Context context) : base(context)
        {
            AutoPackage = false;
        }

        

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetSingleLine(false);
                Control.SoundEffectsEnabled = true;
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#003964"));
            }
        }
    }
}