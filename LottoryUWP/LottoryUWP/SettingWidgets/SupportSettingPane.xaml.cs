using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using LottoryUWP.DataModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.SettingWidgets
{
    public sealed partial class SupportSettingPane : UserControl
    {
        public SupportSettingPane()
        {
            this.InitializeComponent();
            this.VersionText.Text = String.Format("{0} {1}.{2}.{3}", Package.Current.DisplayName,  Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build);

        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.Title = this.Support_Link_Reset.Content;
            dialog.Content = LottoryUWP.Strings.Resources.Restart_App_Reset;
            dialog.PrimaryButtonText = LottoryUWP.Strings.Resources.ContentDialog_ResetAll_NO;
            dialog.SecondaryButtonText = LottoryUWP.Strings.Resources.ContentDialog_ResetAll_OK;

            if(await dialog.ShowAsync() == ContentDialogResult.Secondary)
                await SettingData.Instance.ResetAllSettingAndReboot();
        }

        private async void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = this.VersionText.Text;

            var email = new Windows.ApplicationModel.Contacts.ContactEmail() {Address="yifeng.li@cherubimlee.com" };
            if (email != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email.Address);
                emailMessage.To.Add(emailRecipient);
                emailMessage.Subject = string.Format("{0} {1}", Support_Link_Email.Content.ToString(), DateTime.Now.ToString());
            }

            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);

        }
    }
}
