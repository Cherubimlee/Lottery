using LottoryUWP.DataModel;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace LottoryUWP.Utils
{
    public class ADUtils
    {

        public static ADUtils AdInstance = new ADUtils();

        InterstitialAd myInterstitialAd = null;
        string myAppId = "9nppsdm8nddl";//"d25517cb-12d4-4699-8bdc-52040c712cab";
        string myAdUnitId = "1100029586";//"test";
        string myAdUnitIdImg = "1100029590";

        AdType adType = AdType.Video;

        public ADUtils()
        {
            myInterstitialAd = new InterstitialAd();

            if (!SettingData.Instance.IsAdFree)
            {
                myInterstitialAd.Completed += MyInterstitialAd_Completed;
                myInterstitialAd.Cancelled += MyInterstitialAd_Completed;
                myInterstitialAd.ErrorOccurred += MyInterstitialAd_Completed;

                myInterstitialAd.RequestAd(adType, myAppId, adType == AdType.Video ? myAdUnitId : myAdUnitIdImg);
            }
        }

        public void RandomShowAd(double rate = 1.0)
        {
            var r = RandomUtil.Instance.RandomDouble();

            if (myInterstitialAd.State == InterstitialAdState.Ready && r <= rate)
            {
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = false;
                myInterstitialAd.Show();
            }
        }

        private async void MyInterstitialAd_Completed(object sender, object e)
        {
            if (SettingData.Instance.IsAdFree)
                return;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            await Task.Delay(TimeSpan.FromMinutes(5)); //No more Ad in 5 mins

            AdErrorEventArgs arg = e as AdErrorEventArgs;

            if (arg != null)
            {
                if (arg.ErrorCode == Microsoft.Advertising.ErrorCode.NoAdAvailable)
                    adType = adType == AdType.Video ? AdType.Display : AdType.Video;
            }

            myInterstitialAd.RequestAd(adType, myAppId, adType == AdType.Video ? myAdUnitId : myAdUnitIdImg);


        }
    }
}
