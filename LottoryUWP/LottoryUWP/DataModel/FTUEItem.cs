using LottoryUWP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class FTUEItem
    {
        public string ImagePath { get; set; }
        public string ExtImagePath { get; set; }
        public string Description { get; set; }
        public VersionLevel RequiredVersionLevel { get; set; }
        public bool IsUpgradeOnly { get; set; }

        public Action ActiveAnimation { get; set; }

        public static IEnumerable<FTUEItem> Items {
            get
            {
                var Items = new List<FTUEItem>();

                Items.Add(new FTUEItem() { Description = LottoryUWP.Strings.Resources.FTUE_Intro_Menu, ImagePath = @"ms-appx:///Assets/FTUE/FTUE1.jpg", ExtImagePath = @"ms-appx:///Assets/FTUE/FTUE2.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2, IsUpgradeOnly = false });
                Items.Add(new FTUEItem() { Description = LottoryUWP.Strings.Resources.FTUE_Intro_Img, ImagePath = @"ms-appx:///Assets/FTUE/FTUE3.jpg", ExtImagePath = @"ms-appx:///Assets/FTUE/FTUE4.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2, IsUpgradeOnly = false });
                Items.Add(new FTUEItem() { Description = LottoryUWP.Strings.Resources.FTUE_Intro_Round, ImagePath = @"ms-appx:///Assets/FTUE/FTUE5.jpg", ExtImagePath = @"ms-appx:///Assets/FTUE/FTUE6.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2, IsUpgradeOnly = false });
                Items.Add(new FTUEItem() { Description = LottoryUWP.Strings.Resources.FTUE_Intro_Report, ImagePath = @"ms-appx:///Assets/FTUE/FTUE7.jpg", ExtImagePath = @"ms-appx:///Assets/FTUE/FTUE8.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2, IsUpgradeOnly = false });

                return Items;
            }
        }
    }
    
}
