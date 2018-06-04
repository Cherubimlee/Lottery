using Newtonsoft.Json;
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

        public bool IsAllowDelete { get; set; } = true;

        [JsonIgnore]
        public Brush BrushObj
        {
            get
            {
                if (String.IsNullOrWhiteSpace(URIString))
                {
                    return new SolidColorBrush(SolidBrushColor);
                }
                else
                {
                      return new ImageBrush()
                        {
                            ImageSource = new BitmapImage(new Uri(URIString)),
                            Stretch = Stretch.UniformToFill
                        };
                   
                }
            }
        }

        public static IEnumerable<BrushModel> GetBuiltInModels() {

            List<BrushModel> backgroundBrushModels = new List<BrushModel>((LottoryUWP.Common.Color.Colors.Select(x => new BrushModel() { SolidBrushColor = x.ColorObj, IsAllowDelete = false })));

            backgroundBrushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/Img1.jpg", IsAllowDelete = false });
            backgroundBrushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/Img2.jpg", IsAllowDelete = false });
            backgroundBrushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/Img3.jpg", IsAllowDelete = false });
            backgroundBrushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/Img4.jpg", IsAllowDelete = false });

            return backgroundBrushModels;
        }

    }
}
