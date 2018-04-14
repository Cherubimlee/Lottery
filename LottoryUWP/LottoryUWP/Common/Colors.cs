using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.Common
{
    public class Color
    {
        public string Name { get; set; }
        public Windows.UI.Color ColorObj { get; set; }

        public static IEnumerable<Color> colors;
        public static IEnumerable<Color> Colors
        {
            get
            {
                if(colors != null)
                {
                    return colors;
                }

                var colorlist = new List<Windows.UI.Color>();

                colorlist.Add(Windows.UI.Colors.Black);
                colorlist.Add(Windows.UI.Colors.White);
                colorlist.Add(Windows.UI.Colors.Red);
                colorlist.Add(Windows.UI.Colors.Pink);
                colorlist.Add(Windows.UI.Colors.Orange);
                colorlist.Add(Windows.UI.Colors.RosyBrown);
                colorlist.Add(Windows.UI.Colors.Yellow);
                colorlist.Add(Windows.UI.Colors.YellowGreen);
                colorlist.Add(Windows.UI.Colors.Green);
                colorlist.Add(Windows.UI.Colors.LightCyan);
                colorlist.Add(Windows.UI.Colors.Blue);
                colorlist.Add(Windows.UI.Colors.Violet);
                colorlist.Add(Windows.UI.Colors.DarkViolet);
                colorlist.Add(Windows.UI.Colors.Gray);


                colors = colorlist.Select(color => new Color() { Name = Windows.UI.ColorHelper.ToDisplayName(color), ColorObj = color });
                return colors;
                
            }
        }
    }
    
}
