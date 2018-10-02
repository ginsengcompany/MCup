using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.CostomControls
{
    public class CustomImageCellListRenderer : ImageCell
    {
        public static readonly BindableProperty SelectedBackgroundColorProperty =
                BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(CustomImageCellListRenderer), Color.Default);

        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
    }
}
