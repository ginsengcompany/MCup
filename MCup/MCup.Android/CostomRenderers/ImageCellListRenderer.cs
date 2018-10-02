using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MCup.CostomControls;
using MCup.Droid.CostomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ImageCell), typeof(ImageCellListRenderer))]
namespace MCup.Droid.CostomRenderers
{
    public class ImageCellListRenderer : ImageCellRenderer
    {
        private Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);

            // Save original background to roll-back to it when not selected,
            // we're assuming that no cells will be selected on creation.
            _selected = false;
            _unselectedBackground = _cellCore.Background;

            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                // Had to create a property to track the selection because cellCore.Selected is always false.
                _selected = !_selected;
                if (_selected)
                {
                    var extendedViewCell = sender as CustomImageCellListRenderer;
                    _cellCore.SetBackgroundColor(extendedViewCell.SelectedBackgroundColor.ToAndroid());
                }
                else
                    _cellCore.SetBackground(_unselectedBackground);
            }
        }
    }
}