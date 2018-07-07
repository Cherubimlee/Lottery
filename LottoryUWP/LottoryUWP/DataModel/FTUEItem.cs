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
    }
}
