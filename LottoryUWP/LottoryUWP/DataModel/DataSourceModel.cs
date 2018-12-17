using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LottoryUWP.DataModel
{
    public class DataSourceModel
    {
        public string FilePath { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public string Prefix { get; set; } = string.Empty;

        public bool IsNumberSource { get { return string.IsNullOrEmpty(this.FilePath); } }

        public async Task<DataSourceResult> GenerateDataItems()
        {
         
            if (this.IsNumberSource)
            {
                DataSourceResult r = new DataSourceResult();
                string format = (End < 10 ? "{0:0}" : End < 100 ? "{0:00}" : "{0:000}");

                for (int i = Start; i <= End; i++)
                    r.ItemList.Add(new DrawItem() { MajorColumnValue = Prefix + String.Format(format, i) });

                r.ColumnTitles[0] = "ID";
                return r;
            }
            else
            {
                StorageFolder folder = ApplicationData.Current.TemporaryFolder;

                var file = await folder.GetFileAsync(this.FilePath);

                if (file != null)
                    return await ReadFileForDrawItem(file);
            }

            return null;
        }

        public static async Task<DataSourceResult> ReadFileForDrawItem(StorageFile file)
        {
            DataSourceResult r = new DataSourceResult();

            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                string line1 = await streamReader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line1))
                    return null;

                var titles = line1.Split(new char[] { ',' });

                r.ColumnTitles[0] = titles.FirstOrDefault();
                r.ColumnTitles[1] = titles.ElementAtOrDefault(1);

                while (!streamReader.EndOfStream)
                {
                    String line = streamReader.ReadLine();

                    var patten = line.Split(new[] { ',' });

                    DrawItem drawItem = new DrawItem()
                    {
                        MajorColumnValue = patten.FirstOrDefault(),
                        SecondaryColumnValue = patten.ElementAtOrDefault(1)
                    };

                    if (String.IsNullOrEmpty(drawItem.MajorColumnValue) && String.IsNullOrEmpty(drawItem.SecondaryColumnValue))
                        continue;
                    else
                        r.ItemList.Add(drawItem);
                    
                }

            }

            return r;
        }

        public class DataSourceResult
        {
            public List<DrawItem> ItemList = new List<DrawItem>();
            public String[] ColumnTitles =  new String[2];
        }
    }
}
