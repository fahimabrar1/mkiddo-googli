using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
#if GOOGLE_MOBILE_ADS
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;
#endif
using UnityEngine.Events;

///Developed By Indie Studio
///https://assetstore.unity.com/publishers/9268
///www.indiestd.com
///info@indiestd.com

namespace IndieStudio.EnglishTracingBook.Utility
{

    [DisallowMultipleComponent]
    public class AdMob : MonoBehaviour
    {
#if GOOGLE_MOBILE_ADS
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardBasedVideoAd;
        public string androidBannerAdUnitID;
        public string androidInterstitialAdUnitID;
        public string androidRewardBasedVideoAdUnitID;
        public string IOSBannerAdUnitID;
        public string IOSInterstitialAdUnitID;
        public string IOSRewardBasedVideoAdUnitID;
        public TagForChildDirectedTreatment tagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified;
        public TagForUnderAgeOfConsent tagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False;
        public List<string> testDeviceIDS = new List<string>() { AdRequest.TestDeviceSimulator };
        public bool EnableHandlersDebugMessages = true;


        void Start()
        {
            MobileAds.SetiOSAppPauseOnBackground(true);

            //testDeviceIDS.Add(AdRequest.TestDeviceSimulator);

            // Configure TagForChildDirectedTreatment and test device IDs.
            var requestConfiguration = new RequestConfiguration()
            {
                TagForChildDirectedTreatment = tagForChildDirectedTreatment,
                TagForUnderAgeOfConsent = tagForUnderAgeOfConsent,
                TestDeviceIds = testDeviceIDS,
            };

            MobileAds.SetRequestConfiguration(requestConfiguration);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(HandleInitCompleteAction);

            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            // Callbacks from GoogleMobileAds are not guaranteed to be called on
            // main thread.
            // In this example we use MobileAdsEventExecutor to schedule these calls on
            // the next Update() loop.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                PrintMessage("Initialization complete", EnableHandlersDebugMessages);

                RequestInterstitialAd();
                RequestRewardBasedVideoAd();
            });
        }

        public void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            //UnityEngine.PrintMessage("App State is " + state , EnableHandlersDebugMessages);

            // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
            //MobileAdsEventExecutor.ExecuteInUpdate(() =>
            //{
            //	if (state == AppState.Foreground)
            //	{

            //	}
            //});
        }

        public void RequestBannerAd(AdPosition adPostion)
        {
            PrintMessage("Banner Requested", EnableHandlersDebugMessages);

#if UNITY_ANDROID
            string adUnitId = androidBannerAdUnitID;
#elif UNITY_IPHONE
			string adUnitId = IOSBannerAdUnitID;
#else
			string adUnitId = "unexpected_platform";
#endif

            adUnitId = adUnitId.Trim();

            if (string.IsNullOrEmpty(adUnitId))
            {
                return;
            }

            //Destroy current banner ad, if exists
            DestroyBannerAd();

            // Create a banner
            bannerView = new BannerView(adUnitId, AdSize.Banner, adPostion);

            // Register for ad events.
            bannerView.OnBannerAdLoaded += HandleBannerLoaded;
            bannerView.OnBannerAdLoadFailed += HandleBannerFailedToLoad;
            bannerView.OnAdImpressionRecorded += HandleBannerImpressionRecorded;
            bannerView.OnAdClicked += HandleBannerOnAdClicked;
            bannerView.OnAdFullScreenContentOpened += HandleBannerOpened;
            bannerView.OnAdFullScreenContentClosed += HandleBannerClosed;
            bannerView.OnAdPaid += HandleBannerAdPaid;

            // Create an ad request.
            AdRequest request = CreateAdRequest();

            // Load the banner with the request.
            bannerView.LoadAd(request);
        }

        private void RequestInterstitialAd()
        {
            PrintMessage("interstitialAd Requested", EnableHandlersDebugMessages);

#if UNITY_ANDROID
            string adUnitId = androidInterstitialAdUnitID;
#elif UNITY_IPHONE
					string adUnitId = IOSInterstitialAdUnitID;
#else
					string adUnitId = "unexpected_platform";
#endif

            adUnitId = adUnitId.Trim();

            if (string.IsNullOrEmpty(adUnitId))
            {
                return;
            }

            //Destroy current Interstitial ad, if exists
            DestroyInterstitialAd();

            // Create an ad request.
            AdRequest request = CreateAdRequest();

            InterstitialAd.Load(adUnitId, request,
         (InterstitialAd ad, LoadAdError loadError) =>
         {
             if (loadError != null)
             {
                 PrintMessage("Interstitial ad failed to load with error: " +
                     loadError.GetMessage(), EnableHandlersDebugMessages);
                 return;
             }
             else if (ad == null)
             {
                 PrintMessage("Interstitial ad failed to load.", EnableHandlersDebugMessages);
                 return;
             }

             PrintMessage("Interstitial ad loaded.", EnableHandlersDebugMessages);

             interstitialAd = ad;

             // Register for ad events.
             interstitialAd.OnAdFullScreenContentFailed += HandleInterstitialFailedToLoad;
             interstitialAd.OnAdImpressionRecorded += HandleInterstitialImpressionRecorded;
             interstitialAd.OnAdClicked += HandleInterstitialAdClicked;
             interstitialAd.OnAdFullScreenContentOpened += HandleInterstitialOpened;
             interstitialAd.OnAdFullScreenContentClosed += HandleInterstitialClosed;
             interstitialAd.OnAdPaid += HandleInterstitialAdPaid;

             HandleInterstitialLoaded();
         });

        }

        private void RequestRewardBasedVideoAd()
        {
#if UNITY_ANDROID
            string adUnitId = androidRewardBasedVideoAdUnitID;
#elif UNITY_IPHONE
					string adUnitId = IOSRewardBasedVideoAdUnitID;
#else
					string adUnitId = "unexpected_platform";
#endif

            adUnitId = adUnitId.Trim();

            if (string.IsNullOrEmpty(adUnitId))
            {
                return;
            }

            //Destroy current RewardBasedVideo ad, if exists
            DestroyRewardBasedVideoAd();

            // Create an ad request.
            AdRequest request = CreateAdRequest();

            // create new rewarded ad instance
            RewardedAd.Load(adUnitId, request,
                (RewardedAd ad, LoadAdError loadError) =>
                {
                    if (loadError != null)
                    {
                        PrintMessage("Rewarded ad failed to load with error: " +
                                    loadError.GetMessage(), EnableHandlersDebugMessages);
                        return;
                    }
                    else if (ad == null)
                    {
                        PrintMessage("Rewarded ad failed to load.", EnableHandlersDebugMessages);
                        return;
                    }

                    PrintMessage("Rewarded ad loaded.", EnableHandlersDebugMessages);

                    rewardBasedVideoAd = ad;

                    // Register for ad events.

                    rewardBasedVideoAd.OnAdFullScreenContentFailed += HandleRewardBasedVideoFailedToLoad;
                    rewardBasedVideoAd.OnAdImpressionRecorded += HandleRewardBasedVideoImpressionRecorded;
                    rewardBasedVideoAd.OnAdClicked += HandleRewardBasedVideoAdClicked;
                    rewardBasedVideoAd.OnAdFullScreenContentOpened += HandleRewardBasedVideoOpened;
                    rewardBasedVideoAd.OnAdFullScreenContentClosed += HandleRewardBasedVideoClosed;
                    rewardBasedVideoAd.OnAdPaid += HandleRewardBasedVideoAdPaid;

                    HandleRewardBasedVideoLoaded();
                });
        }

        // Returns an empty ad request.
        private AdRequest CreateAdRequest()
        {
            return new AdRequest();
        }

        private void ShowBannerAd()
        {
            if (bannerView == null)
            {
                return;
            }

            PrintMessage("Show BannerView", EnableHandlersDebugMessages);

            bannerView.Show();
        }

        public void ShowInterstitialAd(UnityEvent onShowAdsEvent)
        {
            if (interstitialAd == null)
            {
                return;
            }

            if (interstitialAd.CanShowAd())
            {
                PrintMessage("Show InterstitialAd", EnableHandlersDebugMessages);

                if (onShowAdsEvent != null)
                    onShowAdsEvent.Invoke();
                interstitialAd.Show();
            }
        }

        public void ShowRewardBasedVideoAd(UnityEvent onShowAdsEvent)
        {
            if (rewardBasedVideoAd == null)
            {
                return;
            }

            if (rewardBasedVideoAd.CanShowAd())
            {
                PrintMessage("Show RewardBasedVideoAd", EnableHandlersDebugMessages);

                if (onShowAdsEvent != null)
                    onShowAdsEvent.Invoke();

                rewardBasedVideoAd.Show((Reward reward) =>
                {
                    //Do custom actions here when receive a reward

                    PrintMessage("Rewarded ad granted a reward: " + reward.Amount, EnableHandlersDebugMessages);
                });
            }
        }

        public void DestroyAllAds()
        {
            DestroyBannerAd();
            DestroyInterstitialAd();
            DestroyRewardBasedVideoAd();
        }

        public void DestroyBannerAd()
        {
            if (bannerView == null)
            {
                return;
            }

            PrintMessage("Banner ad destroyed.", EnableHandlersDebugMessages);

            bannerView.Destroy();
        }

        private void DestroyInterstitialAd()
        {
            if (interstitialAd == null)
            {
                return;
            }

            PrintMessage("InterstitialAd ad destroyed.", EnableHandlersDebugMessages);

            interstitialAd.Destroy();
        }

        private void DestroyRewardBasedVideoAd()
        {
            if (rewardBasedVideoAd == null)
            {
                return;
            }

            PrintMessage("RewardBasedVideoAd ad destroyed.", EnableHandlersDebugMessages);

            rewardBasedVideoAd.Destroy();
        }

        #region Banner callback handlers

        private void HandleBannerLoaded()
        {
            PrintMessage("Banner ad loaded.", EnableHandlersDebugMessages);

            ShowBannerAd();
        }

        private void HandleBannerFailedToLoad(LoadAdError error)
        {
            PrintMessage("Banner ad failed to load with error: " + error.GetMessage(), EnableHandlersDebugMessages);
        }

        private void HandleBannerImpressionRecorded()
        {
            PrintMessage("Banner ad recorded an impression.", EnableHandlersDebugMessages);
        }

        private void HandleBannerOnAdClicked()
        {
            PrintMessage("Banner ad recorded a click.", EnableHandlersDebugMessages);
        }

        private void HandleBannerOpened()
        {
            PrintMessage("Banner ad opening.", EnableHandlersDebugMessages);
        }

        private void HandleBannerClosed()
        {
            PrintMessage("Banner ad closed.", EnableHandlersDebugMessages);
        }

        private void HandleBannerAdPaid(AdValue adValue)
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                             "Rewarded intersitial ad received a paid event.",
                                             adValue.CurrencyCode,
                                             adValue.Value);
            PrintMessage(msg, EnableHandlersDebugMessages);
        }

        #endregion

        #region Interstitial callback handlers

        private void HandleInterstitialLoaded()
        {
            PrintMessage("HandleInterstitialLoaded event received", EnableHandlersDebugMessages);
        }

        private void HandleInterstitialFailedToLoad(AdError error)
        {
            PrintMessage("Interstitial ad failed to show with error: " +
                                error.GetMessage(), EnableHandlersDebugMessages);
        }

        private void HandleInterstitialOpened()
        {
            PrintMessage("Interstitial ad opening.", EnableHandlersDebugMessages);
        }

        private void HandleInterstitialClosed()
        {
            PrintMessage("Interstitial ad closed.", EnableHandlersDebugMessages);

            RequestInterstitialAd();
        }

        private void HandleInterstitialImpressionRecorded()
        {
            PrintMessage("Interstitial ad recorded an impression.", EnableHandlersDebugMessages);
        }

        private void HandleInterstitialAdClicked()
        {
            PrintMessage("Interstitial ad recorded a click.", EnableHandlersDebugMessages);
        }

        private void HandleInterstitialAdPaid(AdValue adValue)
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                              "Interstitial ad received a paid event.",
                                              adValue.CurrencyCode,
                                              adValue.Value);
            PrintMessage(msg, EnableHandlersDebugMessages);
        }

        #endregion

        #region RewardBasedVideo callback handlers

        private void HandleRewardBasedVideoLoaded()
        {
            PrintMessage("HandleRewardBasedVideoLoaded event received", EnableHandlersDebugMessages);
        }

        private void HandleRewardBasedVideoFailedToLoad(AdError error)
        {
            PrintMessage("Rewarded ad failed to show with error: " +
                            error.GetMessage(), EnableHandlersDebugMessages);
        }

        private void HandleRewardBasedVideoOpened()
        {
            PrintMessage("Rewarded ad opening.", EnableHandlersDebugMessages);
        }

        private void HandleRewardBasedVideoClosed()
        {
            PrintMessage("Rewarded ad closed.", EnableHandlersDebugMessages);

            RequestRewardBasedVideoAd();
        }

        private void HandleRewardBasedVideoImpressionRecorded()
        {
            PrintMessage("Rewarded ad recorded an impression.", EnableHandlersDebugMessages);
        }

        private void HandleRewardBasedVideoAdClicked()
        {
            PrintMessage("Rewarded ad recorded a click.", EnableHandlersDebugMessages);
        }

        private void HandleRewardBasedVideoAdPaid(AdValue adValue)
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                   "Rewarded ad received a paid event.",
                                                   adValue.CurrencyCode,
                                                   adValue.Value);
            PrintMessage(msg, EnableHandlersDebugMessages);
        }

        #endregion


        public void PrintMessage(string message, bool isEnabled = true)
        {
            if (isEnabled)
                Debug.Log(message);
        }
#endif
    }

}



