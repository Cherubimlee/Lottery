using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LottoryUWP.DataModel
{
    public class BrushModel
    {
        public Color SolidBrushColor { get; set; }

        public string URIString { get; set; }

        public Brush ToBrush()
        {
            if (String.IsNullOrWhiteSpace(URIString))
            {
                return new SolidColorBrush(SolidBrushColor);
            }
            else
            {
                if (Uri.IsWellFormedUriString(URIString, UriKind.RelativeOrAbsolute))
                    return new ImageBrush()
                    {
                        ImageSource = new BitmapImage(new Uri(URIString)),
                        Stretch = Stretch.UniformToFill
                    };
                else
                    return new SolidColorBrush(SolidBrushColor);
            }
        }
    }
}
