using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System.Linq;

///Developed by Indie Studio
///https://assetstore.unity.com/publishers/9268
///www.indiestd.com
///info@indiestd.com

namespace IndieStudio.EnglishTracingBook.Editors
{
    public class PostProcessor : AssetPostprocessor
    {
        private static readonly string googleMobileAdsPath = Application.dataPath + "/GoogleMobileAds";
        private static readonly string chartBoostAdsPath = Application.dataPath + "/Chartboost";
        private static readonly string googleMobileAdsDefine = "GOOGLE_MOBILE_ADS";
        private static readonly string chartBoosteAdsDefine = "CHARTBOOST_ADS";
        private static ListRequest Request;

        static void HandleReuqest()
        {
            if (Request.IsCompleted)
            {

                var androidCurrentDefineSymbols = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android);
                var iosCurrentDefineSymbols = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.iOS);

                var androidDefines = string.Empty;
                var iosDefines = string.Empty;

                //Unity Package Manager Client Search
                //if (Request.Result != null && Request.Result.ToList().Where(x => x.name == "com.unity.ads").FirstOrDefault() != null)
                //{
                //}

                if (System.IO.Directory.Exists(googleMobileAdsPath))
                {
                    if (!androidCurrentDefineSymbols.Contains(googleMobileAdsDefine))
                        androidDefines += googleMobileAdsDefine + ";";

                    if (!iosCurrentDefineSymbols.Contains(googleMobileAdsDefine))
                        iosDefines += googleMobileAdsDefine + ";";
                }

                if (System.IO.Directory.Exists(chartBoostAdsPath))
                {
                    if (!androidCurrentDefineSymbols.Contains(chartBoosteAdsDefine))
                        androidDefines += chartBoosteAdsDefine + ";";

                    if (!iosCurrentDefineSymbols.Contains(chartBoosteAdsDefine))
                        iosDefines += chartBoosteAdsDefine + ";";
                }

                if (!string.IsNullOrEmpty(androidDefines))
                    PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, androidCurrentDefineSymbols + ";" + androidDefines);

                if (!string.IsNullOrEmpty(iosDefines))
                    PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.iOS, iosCurrentDefineSymbols + ";" + iosDefines);

                EditorApplication.update -= HandleReuqest;
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            Request = Client.List();

            EditorApplication.update -= HandleReuqest;
            EditorApplication.update += HandleReuqest;
        }
    }
}