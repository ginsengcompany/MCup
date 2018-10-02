using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MCup.CostomControls;
using MCup.iOS.CostomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageCell), typeof(ImageCellListRenderer))]
namespace MCup.iOS.CostomRenderers
{
    public class ImageCellListRenderer : ImageCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as CustomImageCellListRenderer;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
            };
            return cell;
        }
    }
}