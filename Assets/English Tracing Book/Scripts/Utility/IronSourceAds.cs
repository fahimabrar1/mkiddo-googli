using UnityEngine;
using UnityEngine.Events;


namespace IndieStudio.EnglishTracingBook.Utility
{
    [DisallowMultipleComponent]
    public class IronSourceAds : MonoBehaviour
    {

        #if LEVELPLAY_DEPENDENCIES_INSTALLED

        public string androidAppKey = "";
        public string iosAppKey = "";
        private UnityEvent onShowAdsEvent;
        public bool isChildDirected = false;
        public bool setConsent = false;

        void Start()
        {
            string appKey = string.Empty;

            if (Application.platform == RuntimePlatform.Android)
            {
                appKey = androidAppKey;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                appKey = iosAppKey;

            }

            IronSource.Agent.setConsent(setConsent);

            if (isChildDirected)
            {
                IronSource.Agent.setMetaData("is_deviceid_optout", "true");
                IronSource.Agent.setMetaData("is_child_directed", "true");
                IronSource.Agent.setMetaData("Google_Family_Self_Certified_SDKS", "true");
            }

            //Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            //Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            //Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(appKey);

        }

        void OnEnable()
        {
            //Add Init Event
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

            //Add ImpressionSuccess Event
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

            //Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdReadyEvent += RewardedVideoOnAdReadyEvent;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;

            //Add AdInfo Interstitial Events
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

            //Add AdInfo Banner Events
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
        }

        void OnApplicationPause(bool isPaused)
        {
            //Debug.Log("unity-script: OnApplicationPause = " + isPaused);
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void ShowRewardedVideo(UnityEvent onShowAdsEvent)
        {
            this.onShowAdsEvent = onShowAdsEvent;

            IronSource.Agent.loadRewardedVideo();
        }

        public void ShowInterstitial(UnityEvent onShowAdsEvent)
        {
            this.onShowAdsEvent = onShowAdsEvent;

            IronSource.Agent.loadInterstitial();
        }

        public void LoadBanner(IronSourceBannerPosition position)
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, position);
        }

        public void DestroyBanner()
        {
            IronSource.Agent.destroyBanner();
        }


        #region Init callback handlers

        void SdkInitializationCompletedEvent()
        {
            //Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
        }

        #endregion

        #region AdInfo Rewarded Video
        void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got ReardedVideoOnAdOpenedEvent With AdInfo " + adInfo.ToString());
        }

        void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got ReardedVideoOnAdClosedEvent With AdInfo " + adInfo.ToString());
        }

        void RewardedVideoOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
                onShowAdsEvent?.Invoke();
            }
            else
            {
                //Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
            }

            //Debug.Log("unity-script: I got ReardedVideoOnAdAvailable With AdInfo " + adInfo.ToString());
        }

        void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got RewardedVideoOnAdAvailable");
        }

        void RewardedVideoOnAdUnavailable()
        {
            //Debug.Log("unity-script: I got ReardedVideoOnAdUnavailable");
        }

        void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError.ToString() + "And AdInfo " + adInfo.ToString());
        }

        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got ReardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
        }

        void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
        }

        #endregion


        #region AdInfo Interstitial

        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
                onShowAdsEvent?.Invoke();
            }
            else
            {
                //Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
            }

            //Debug.Log("unity-script: I got InterstitialOnAdReadyEvent With AdInfo " + adInfo.ToString());
        }

        void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdLoadFailed With Error " + ironSourceError.ToString());
        }

        void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdOpenedEvent With AdInfo " + adInfo.ToString());
        }

        void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo.ToString());
        }

        void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdShowSucceededEvent With AdInfo " + adInfo.ToString());
        }

        void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdShowFailedEvent With Error " + ironSourceError.ToString() + " And AdInfo " + adInfo.ToString());
        }

        void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo.ToString());
        }


        #endregion

        #region Banner AdInfo

        void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo.ToString());
        }

        void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
        {
            //Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError.ToString());
        }

        void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo.ToString());
        }

        void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got BannerOnAdScreenPresentedEvent With AdInfo " + adInfo.ToString());
        }

        void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got BannerOnAdScreenDismissedEvent With AdInfo " + adInfo.ToString());
        }

        void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
        {
            //Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo.ToString());
        }

        #endregion

        #region ImpressionSuccess callback handler

        void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            //Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
            //Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
        }

        #endregion

#endif
    }
}
