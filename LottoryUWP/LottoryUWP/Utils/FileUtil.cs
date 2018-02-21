using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace LottoryUWP.Utils
{
    public static class FileUtil
    {
        public static async Task<StorageFile> OpenFileForSave()
        {
            FileSavePicker savePicker = new FileSavePicker();


            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("HTML Report", new List<string>() { ".html" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Report";

            return await savePicker.PickSaveFileAsync();
        }

        public static async Task<bool> WriteTextAsync(this StorageFile file, string text)
        {
            if (file != null)
            {
                try
                {
                    await FileIO.WriteTextAsync(file, text);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
