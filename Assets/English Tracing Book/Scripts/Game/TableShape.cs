﻿using UnityEngine;
using System.Collections;
using System;

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
    public class TableShape : MonoBehaviour
    {
        /// <summary>
        /// Table Shape ID.
        /// </summary>
        public int ID = -1;

        // Use this for initialization
        void Start()
        {
            ///Setting up the ID for Table Shape
            if (ID == -1)
            {
                string[] tokens = gameObject.name.Split('-');
                if (tokens != null)
                {
                    ID = int.Parse(tokens[1]);
                }
            }
        }
    }
}