using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * English Tracing Book Package
 *
 * @License		      Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @Author		      Indie Studio - Baraa Nasser
 * @Website		      https://indiestd.com
 * @Asset Store       https://assetstore.unity.com/publishers/9268
 * @Unity Connect     https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @Email		      info@indiestd.com
 *
 */

namespace IndieStudio.EnglishTracingBook.Game
{
    public class FullScreenGameMode : MonoBehaviour
    {
        /// <summary>
        /// Where full screen enabled or not
        /// </summary>
        private static bool fullScreenEnabled = false;

        /// <summary>
        /// Objects which needs to show/hide for the full screen mode
        /// </summary>
        public Transform[] objectsActive,objectsNotActive;

        /// <summary>
        /// Objects which needs to be in the center (x-position) when full screen mode is enabled
        /// </summary>
        public Transform[] objectsCentered;

        /// <summary>
        /// Initial x-Position for each centered object
        /// </summary>
        private float[] objectsInitialXPosition;

        /// <summary>
        /// Events on full mode enabled/disabled
        /// </summary>
        public UnityEvent onFullModeEnabled, onFullModeDisabled;

        /// <summary>
        /// Event triggers On Start end
        /// </summary>
        public UnityEvent OnStartEndCallBack;

        // Use this for initialization
        void Start()
        {
            if(objectsCentered.Length > 0)
            objectsInitialXPosition = new float[objectsCentered.Length];

           for(int i = 0; i< objectsCentered.Length;i++)
            {
                if (objectsCentered[i] == null) continue;

                objectsInitialXPosition[i] = objectsCentered[i].position.x;
            }

            ApplyObjectsStatus();
            EventsCallBack();

            if (OnStartEndCallBack != null)
            {
                OnStartEndCallBack.Invoke();
            }
        }

        /// <summary>
        /// Hide all buttons /  menus and keep shape for tracing
        /// and show shape's controls buttons (move/resize/expand)
        /// </summary>
        public void ToggleFullScreenMode()
        {
            fullScreenEnabled = !fullScreenEnabled;
            ApplyObjectsStatus();

            EventsCallBack();
        }


        /// <summary>
        /// Full Mode events call back
        /// </summary>
        private void EventsCallBack()
        {
            if (fullScreenEnabled)
            {
                onFullModeEnabled.Invoke();
            }
            else
            {
                onFullModeDisabled.Invoke();
            }
        }

        /// <summary>
        /// Apply status (Active/ Not Active) for objects
        /// </summary>
        private void ApplyObjectsStatus()
        {
            foreach (Transform t in objectsActive)
            {
                if (t == null) continue;

                t.gameObject.SetActive(fullScreenEnabled);
            }

            foreach (Transform t in objectsNotActive)
            {
                if (t == null) continue;

                t.gameObject.SetActive(!fullScreenEnabled);
            }

            AppObjectsCenteredStatus();
        }

        /// <summary>
        /// Whether to center the objects (When mode is enabled) or return them to intial position (When mode is disabled) 
        /// </summary>
        private void AppObjectsCenteredStatus()
        {
            Vector3 position;
            for(int i = 0; i< objectsCentered.Length;i++)
            {
                if (objectsCentered[i] == null) continue;

                position = objectsCentered[i].position;

                if (fullScreenEnabled)
                {
                    position.x = 0;   
                }
                else
                {
                    position.x = objectsInitialXPosition[i];
                }

                objectsCentered[i].position = position;
            }
        }

    }
}