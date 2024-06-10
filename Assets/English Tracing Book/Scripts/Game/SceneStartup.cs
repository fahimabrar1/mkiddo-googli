﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using IndieStudio.EnglishTracingBook.Utility;

/*
 * English Tracing Book Package
 *
 * @license		    Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @author		    Indie Studio - Baraa Nasser
 * @Website		    https://indiestd.com
 * @Asset Store     https://assetstore.unity.com/publishers/9268
 * @Unity Connect   https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @email		    info@indiestd.com
 *
 */

namespace IndieStudio.EnglishTracingBook.Game
{
    [DisallowMultipleComponent]
    public class SceneStartup : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            ShowAd();
        }

        public void ShowAd()
        {
            if (SceneManager.GetActiveScene().name == "Logo")
            {
                AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_LOAD_LOGO_SCENE);
            }
            else if (SceneManager.GetActiveScene().name == "Main")
            {
                AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_LOAD_MAIN_SCENE);
            }
            else if (SceneManager.GetActiveScene().name == "Album")
            {
                AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_LOAD_ALBUM_SCENE);
            }
            else if (SceneManager.GetActiveScene().name == "Game")
            {
                AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_LOAD_GAME_SCENE);
            }
        }

        void OnDestroy()
        {
            AdsManager.instance.HideAdvertisment();
        }
    }
}