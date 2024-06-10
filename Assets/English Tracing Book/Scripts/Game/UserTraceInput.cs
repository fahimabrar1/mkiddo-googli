using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class UserTraceInput : MonoBehaviour
    {
        [HideInInspector]
        public string text;
        public static UserTraceInput instance;
        public string shapesManagerReference = "SShapesManager";

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyReference();
            }
        }


        public void DestroyReference()
        {
            Destroy(gameObject);
        }

    }
}
