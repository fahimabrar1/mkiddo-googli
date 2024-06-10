using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
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
    public class UIEvents : MonoBehaviour
    {
        /// <summary>
        /// Static instance of this class.
        /// </summary>
        public static UIEvents instance;

        //Set of dialogs
        private Dialog renewHelpBoosterDialog;

        /// <summary>
        /// A Unity event.
        /// </summary>
        private UnityEvent unityEvent = new UnityEvent();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            GameObject temp;

            temp = GameObject.Find("RenewHelpBoosterDialog");
            if (temp != null)
            {
                renewHelpBoosterDialog = temp.GetComponent<Dialog>();
            }
        }

        public void AlbumShapeEvent(TableShape tableShape)
        {
            if (tableShape == null)
            {
                return;
            }

            ShapesManager shapesManager = ShapesManager.GetCurrentShapesManager();

            if (shapesManager == null)
            {
                return;
            }

            if (shapesManager.shapes[tableShape.ID].isLocked && !shapesManager.testMode)
            {
                return;
            }

            ShapesManager.Shape.selectedShapeID = tableShape.ID;
            LoadGameScene();
        }

        public void PointerButtonEvent(Pointer pointer)
        {
            if (pointer == null)
            {
                return;
            }
            if (pointer.group != null)
            {
                if (ScrollSlider.instance != null)
                {
                    ScrollSlider.instance.DisableCurrentPointer();
                    ScrollSlider.instance.currentGroupIndex = pointer.group.Index;
                    ScrollSlider.instance.GoToCurrentGroup();
                }
            }
        }

        public void LoadMainScene()
        {
            DestroyUserTraceInput();

            StartCoroutine(SceneLoader.instance.LoadScene("Main"));
        }

        public void LoadGameScene()
        {
            StartCoroutine(SceneLoader.instance.LoadScene("Game"));
        }

        public void LoadSettingsScene()
        {
            StartCoroutine(SceneLoader.instance.LoadScene("Settings"));
        }

        public void LoadAlbumScene()
        {
            if (UserTraceInput.instance != null)
            {
                LoadUserInputScene();
            }
            //Load Album scene based on current shapesManagerReference
            else if (ShapesManager.GetCurrentShapesManager() != null)
            {
                StartCoroutine(SceneLoader.instance.LoadScene(ShapesManager.GetCurrentShapesManager().sceneName));
            }
        }

        public void LoadAlbumScene(string shapesManagerReference)
        {
            //Load Album scene based on given shapesManagerReference
            if (string.IsNullOrEmpty(shapesManagerReference))
            {
                Debug.LogError("Empty Shapes Manager Reference in the Button Component");
                return;
            }
            ShapesManager shapesManager = ShapesManager.shapesManagers[shapesManagerReference];
            ShapesManager.shapesManagerReference = shapesManagerReference;
            StartCoroutine(SceneLoader.instance.LoadScene(shapesManager.sceneName));
        }

        public void LoadUserInputScene()
        {
            StartCoroutine(SceneLoader.instance.LoadScene("UserInput"));
        }

        private void DestroyUserTraceInput()
        {
            if (UserTraceInput.instance != null)
            {
                UserTraceInput.instance.DestroyReference();
            }
        }

        public void NextClickEvent()
        {
            GameManager.instance.NextShape();
        }

        public void PreviousClickEvent()
        {
            GameManager.instance.PreviousShape();
        }

        public void SpeechClickEvent()
        {
            GameManager.instance.Spell();
        }

        public void ResetShape()
        {
            if (!GameManager.instance.shape.completed)
            {
                GameManager.instance.DisableGameManager();
                GameObject.Find("ResetShapeConfirmDialog").GetComponent<Dialog>().Show(false);
            }
            else
            {
                GameManager.instance.ResetShape();
            }
        }

        public void PencilClickEvent(Pencil pencil)
        {
            if (pencil == null)
            {
                return;
            }
            if (GameManager.instance == null)
            {
                return;
            }
            if (GameManager.instance.currentPencil != null)
            {
                GameManager.instance.currentPencil.DisableSelection();
                GameManager.instance.currentPencil = pencil;
            }
            GameManager.instance.SetShapeOrderColor();
            pencil.EnableSelection();
        }

        public void HelpUserBoosterClick(Booster booster)
        {
            if (booster == null || !GameManager.instance.isRunning)
            {
                return;
            }
           
            if (booster.GetValue() == 0)
            {
                AudioSources.instance.PlayLockedSFX();
                ShowRenewHelpBoosterDialog();
                return;
            }

            booster.DecreaseValue();

            GameManager.instance.HelpUser();
        }

        public void RenewHelpBooster(Booster booster)
        {
            if (booster == null)
            {
                return;
            }

            GameManager.instance.Resume();
            renewHelpBoosterDialog.Hide(true);

            //Set yes button of the dialog as interactable false
            renewHelpBoosterDialog.transform.Find("YesButton").GetComponent<Button>().interactable = false;

            //Renew booster value when the Ad is sucessfully appeared
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(() => booster.ResetValue());
            unityEvent.AddListener(() => renewHelpBoosterDialog.transform.Find("YesButton").GetComponent<Button>().interactable = true);

            AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_RENEW_HELP_COUNT, unityEvent);
        }

        public void ResetShapeClickEvent()
        {
            GameManager.instance.ResetShape();
            Progress.instance.ResetDropFlags();
        }

        public void ShowResetGameDialog()
        {
            GameObject.Find("ResetGameConfirmDialog").GetComponent<Dialog>().Show(true);
        }

        public void ShowRenewHelpBoosterDialog()
        {
            GameManager.instance.Pause();
            renewHelpBoosterDialog.Show(true);
        }

        public void ResetGame()
        {
            DataManager.ResetGame();
        }

        public void LeaveApp()
        {
            Application.Quit();
        }
    }
}