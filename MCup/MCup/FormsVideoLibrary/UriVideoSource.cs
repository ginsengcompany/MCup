using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.FormsVideoLibrary
{
        public class UriVideoSource : VideoSource
        {
            public static readonly BindableProperty UriProperty =
                BindableProperty.Create(nameof(Uri), typeof(string), typeof(UriVideoSource));

            public string Uri
            {
                set { SetValue(UriProperty, value); }
                get { return (string)GetValue(UriProperty); }
            }
        }
    }
