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

        public Action ActiveAnimation { get; set; }

        public static IEnumerable<FTUEItem> Items {
            get
            {
                var Items = new List<FTUEItem>();

                Items.Add(new FTUEItem() { Description = "Hello World 0", ImagePath = @"ms-appx:///Assets/Img/Img1.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2 });

                Items.Add(new FTUEItem() { Description = "Hello World 1", ImagePath = @"ms-appx:///Assets/Img/Img1.jpg", ExtImagePath = @"ms-appx:///Assets/Img/Img2.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2 });

                Items.Add(new FTUEItem() { Description = "Hello World 2", ImagePath = @"ms-appx:///Assets/Img/Img2.jpg", ExtImagePath = @"ms-appx:///Assets/Img/Img3.jpg", RequiredVersionLevel = VersionLevel.Ver_1_2 });

                return Items;
            }
        }
    }
    
}
