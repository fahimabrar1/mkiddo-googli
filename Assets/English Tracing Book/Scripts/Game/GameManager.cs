using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using IndieStudio.EnglishTracingBook.Utility;
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
    [DisallowMultipleComponent]
    [RequireComponent(typeof(FullScreenGameMode))]
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Whether the script is running or not.
        /// </summary>
        public bool isRunning = true;

        /// <summary>
        /// Whether to hide shape's image on complete or not
        /// </summary>
        public bool hideShapeImageOnComplete;

        /// <summary>
        /// Whether to move/animate numbers of the paths on Start or not
        /// </summary>
        public bool animateNumbersOnStart = true;

        /// <summary>
        /// Whether to enable Fill Qurater Angle Restriction or not
        /// </summary>
        private bool quarterRestriction = true;

        /// <summary>
        /// The sentence's prefab (used to create custom user input)
        /// </summary>
        public GameObject sentencePrefab;

        /// <summary>
        /// The line's prefab
        /// </summary>
        public GameObject linePrefab;

        /// <summary>
        /// The PathObjectMove of the Hand
        /// </summary>
        public PathObjectMove handPOM;

        /// <summary>
        /// The reset confirm dialog
        /// </summary>
        public Dialog resetConfirmDialog;

        /// <summary>
        /// The next/previous button transform
        /// </summary>
        public Transform nextButton, previousButton;

        /// <summary>
        /// The current pencil.
        /// </summary>
        public Pencil currentPencil;

        /// <summary>
        /// The shape order.
        /// </summary>
        public Text shapeOrder;

        /// <summary>
        /// The write shape name text.
        /// </summary>
        public Text writeText;

        /// <summary>
        /// The tracing path that the user selected (currently pressed).
        /// </summary>
        private TracingPath selectedTracingPath;

        /// <summary>
        /// The shape parent.
        /// </summary>
        public Transform shapeParent;

        /// <summary>
        /// The shape reference.
        /// </summary>
        [HideInInspector]
        public Shape shape;

        /// <summary>
        /// The click postion.
        /// </summary>
        private Vector3 clickPostion;

        /// <summary>
        /// The direction between click and shape.
        /// </summary>
        private Vector2 direction;

        /// <summary>
        /// The current angle , angleOffset and fill amount.
        /// </summary>
        private float angle, angleOffset, fillAmount;

        /// <summary>
        /// The clock wise sign.
        /// </summary>
        private float clockWiseSign;

        /// <summary>
        /// The hand reference.
        /// </summary>
        public Transform hand;

        /// <summary>
        /// The default size of the cursor.
        /// </summary>
        private Vector3 cursorDefaultSize;

        /// <summary>
        /// The click size of the cursor.
        /// </summary>
        private Vector3 cursorClickSize;

        /// <summary>
        /// The target quarter of the radial fill.
        /// </summary>
        private float targetQuarter;

        /// <summary>
        /// The complete effect.
        /// </summary>
        public ParticleSystem winEffect;

        /// <summary>
        /// UI Parent Gameobject that contains the Timer
        /// </summary>
        public Transform timerPanel;

        /// <summary>
        /// The window dialog reference.
        /// </summary>
        public WinDialog winDialog;

        /// <summary>
        /// The shape picture image reference (used to show the picture image  of the selected shape).
        /// </summary>
        public Image shapePicture;

        /// <summary>
        /// The hit2d reference.
        /// </summary>
        private RaycastHit2D hit2d;

        /// <summary>
        /// The compound shape reference.
        /// </summary>
        [HideInInspector]
        public CompoundShape compoundShape;

        /// <summary>
        /// Hand Sparkes effect while moving or tracing
        /// </summary>
        public ParticleSystem handSparkles;

        /// <summary>
        /// Temp Emission Module for the ParticleSystem
        /// </summary>
        private ParticleSystem.EmissionModule tempEmission;

        /// <summary>
        /// Static instance of this class.
        /// </summary>
        public static GameManager instance;

        void Awake()
        {
            //Initiate GameManager instance 
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            Init();
        }

        /// <summary>
        /// Init references/values
        /// </summary>
        private void Init()
        {
            //Initiate values and setup the references
            cursorDefaultSize = hand.transform.localScale;
            cursorClickSize = cursorDefaultSize / 1.2f;

            if (handSparkles != null)
            {
                tempEmission = handSparkles.emission;
                tempEmission.enabled = false;
            }

            if (string.IsNullOrEmpty(ShapesManager.shapesManagerReference) && UserTraceInput.instance == null)
            {
                Debug.LogErrorFormat("You have to start the game from the <b>Main</b> scene");
                return;
            }

            if (handPOM == null)
            {
                handPOM = GameObject.Find("TracingHand").GetComponent<PathObjectMove>();
            }

            if (currentPencil != null)
            {
                currentPencil.EnableSelection();
            }

            winEffect.gameObject.SetActive(false);

            ResetTargetQuarter();
            SetShapeOrderColor();
            CreateShape();
        }

        // Update is called once per frame
        void Update()
        {
            //Game Logic is here

            if (!isRunning)
            {
                return;
            }

            HandleInput();
        }

        /// <summary>
        /// Handle user's input
        /// </summary>
        private void HandleInput()
        {
            DrawHand(GetCurrentPlatformClickPosition(Camera.main));
            DrawBrightEffect(GetCurrentPlatformClickPosition(Camera.main));

            if (shape == null)
            {
                return;
            }

            if (shape.completed)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                //if (!shape.completed)
                //brightEffect.gameObject.SetActive(false);

                hit2d = Physics2D.Raycast(GetCurrentPlatformClickPosition(Camera.main), Vector2.zero);
                if (hit2d.collider != null)
                {
                    if (handSparkles != null)
                        tempEmission.enabled = true;

                    if (hit2d.transform.tag == "Point")
                    {
                        OnPointHitCollider(hit2d);

                        if (Input.GetMouseButtonDown(0))
                        {
                            shape.CancelInvoke();
                            DisableHandTracing();
                            ShowUserTouchHand();
                        }
                    }
                    else if (hit2d.transform.tag == "Collider")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            DisableHandTracing();
                            ShowUserTouchHand();
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (handSparkles != null)
                    tempEmission.enabled = false;

                //brightEffect.gameObject.SetActive(false);
                HideUserTouchHand();
                StartAutoTracing(shape, 1);
                ResetPath();
            }

            if (selectedTracingPath == null)
            {
                return;
            }

            if (selectedTracingPath.completed)
            {
                return;
            }

            if (ShapesManager.GetCurrentShapesManager().enableTracingLimit)
            {
                hit2d = Physics2D.Raycast(GetCurrentPlatformClickPosition(Camera.main), Vector2.zero);
                if (hit2d.collider == null)
                {
                    AudioSources.instance.PlayWrongSFX();
                    ResetPath();
                    return;
                }
            }

            TracePath();
        }

        /// <summary>
        /// On Point collider hit.
        /// </summary>
        /// <param name="hit2d">Hit2d.</param>
        private void OnPointHitCollider(RaycastHit2D hit2d)
        {
            selectedTracingPath = hit2d.transform.GetComponentInParent<TracingPath>();

            if (selectedTracingPath == null)
            {
                return;
            }

            selectedTracingPath.DisableCurrentPoint();
            selectedTracingPath.tracedPoints++;
            selectedTracingPath.EnableCurrentPoint();

            if (selectedTracingPath.completed || !shape.IsCurrentPath(selectedTracingPath))
            {
                ReleasePath();
            }
            else
            {
                selectedTracingPath.StopAllCoroutines();
                selectedTracingPath.fillImage.color = currentPencil.color.colorKeys[0].color;
            }

            selectedTracingPath.curve.Init();

            if (!selectedTracingPath.shape.enablePriorityOrder)
            {
                shape = selectedTracingPath.shape;
            }
        }

        /// <summary>
        /// Get the current platform click position in the world space.
        /// </summary>
        /// <returns>The current platform click position.</returns>
        private Vector3 GetCurrentPlatformClickPosition(Camera camera)
        {
            Vector3 clickPosition = Vector3.zero;

            if (Application.isMobilePlatform)
            {//current platform is mobile
                if (Input.touchCount != 0)
                {
                    Touch touch = Input.GetTouch(0);
                    clickPosition = touch.position;
                }
            }
            else
            {//others
                clickPosition = Input.mousePosition;
            }

            clickPosition = camera.ScreenToWorldPoint(clickPosition);//get click position in the world space
            clickPosition.z = 0;
            return clickPosition;
        }

        /// <summary>
        /// Create new shape.
        /// </summary>
        private void CreateShape()
        {
            if (UserTraceInput.instance == null)
            {
                // Timer.instance.Reset();
            }

            winEffect.gameObject.SetActive(false);
            resetConfirmDialog.Hide(true);
            BlackArea.instance.Hide();
            winDialog.Hide();
            //nextButton.GetComponent<Animator>().SetBool("Select", false);

            CompoundShape currentCompoundShape = FindFirstObjectByType<CompoundShape>();
            if (currentCompoundShape != null)
            {
                DestroyImmediate(currentCompoundShape.gameObject);
            }
            else
            {
                Shape shapeComponent = FindFirstObjectByType<Shape>();
                if (shapeComponent != null)
                {
                    DestroyImmediate(shapeComponent.gameObject);
                }
            }

            try
            {
                GameObject shapeGameObject = null;
                if (UserTraceInput.instance != null)
                {
                    shapeGameObject = Instantiate(sentencePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    shapeGameObject.transform.SetParent(shapeParent);
                    shapeGameObject.transform.localPosition = sentencePrefab.transform.localPosition;

                    var cs = shapeGameObject.GetComponent<CompoundShape>();
                    cs.text = UserTraceInput.instance.text;
                    cs.Generate();
                    shapeOrder.gameObject.SetActive(false);
                    shapePicture.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    previousButton.gameObject.SetActive(false);
                }
                else
                {
                    GameObject shapePrefab = ShapesManager.GetCurrentShapesManager().GetCurrentShape().prefab;
                    shapeGameObject = Instantiate(shapePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    shapeGameObject.transform.SetParent(shapeParent);
                    shapeGameObject.transform.localPosition = shapePrefab.transform.localPosition;
                    shapeGameObject.name = shapePrefab.name;

                    shapeOrder.text = (ShapesManager.Shape.selectedShapeID + 1) + "/" + ShapesManager.GetCurrentShapesManager().shapes.Count;
                    ShapesManager.GetCurrentShapesManager().lastSelectedGroup = ShapesManager.Shape.selectedShapeID;
                }

                compoundShape = FindFirstObjectByType<CompoundShape>();

                if (compoundShape != null)
                {//Scentence
                    shapeGameObject.transform.localScale = Vector3.one * compoundShape.scaleFactor * CommonUtil.ShapeGameAspectRatio();
                    shape = compoundShape.shapes[0];
                }
                else
                {//Shape
                    shape = FindFirstObjectByType<Shape>();
                }

                StartAutoTracing(shape, 0.5f);
                Spell();
            }
            catch
            {
                //Catch the exception or display an alert
            }

            if (shape == null)
            {
                return;
            }

            //Set up write text/label and
            //Setup rest message in the Rest Confirm Dialog
            if (UserTraceInput.instance == null)
            {
                CommonUtil.FindChildByTag(resetConfirmDialog.transform, "Message").GetComponent<Text>().text = "Reset " + ShapesManager.GetCurrentShapesManager().shapeLabel + " " + shape.GetTitle() + " ?";
                writeText.text = "Write the " + ShapesManager.GetCurrentShapesManager().shapeLabel.ToLower() + " '" + shape.GetTitle() + "'";

                //Set up shape's picture
                shapePicture.sprite = ShapesManager.GetCurrentShapesManager().GetCurrentShape().picture;
                if (shapePicture.sprite == null)
                {
                    shapePicture.enabled = false;
                }
                else
                {
                    shapePicture.enabled = true;
                }
            }
            else
            {
                CommonUtil.FindChildByTag(resetConfirmDialog.transform, "Message").GetComponent<Text>().text = "Reset " + shape.GetTitle() + " ?";
                writeText.text = "Write the text '" + shape.GetTitle() + "'";
            }

            EnableGameManager();
        }

        /// <summary>
        /// Go to the Next shape.
        /// </summary>
        public void NextShape()
        {
            if (ShapesManager.Shape.selectedShapeID >= 0 && ShapesManager.Shape.selectedShapeID < ShapesManager.GetCurrentShapesManager().shapes.Count - 1)
            {
                //Get the next shape and check if it's locked , then do not load the next shape
                if (ShapesManager.Shape.selectedShapeID + 1 < ShapesManager.GetCurrentShapesManager().shapes.Count)
                {

                    if (DataManager.IsShapeLocked(ShapesManager.Shape.selectedShapeID + 1, ShapesManager.GetCurrentShapesManager()) && !ShapesManager.GetCurrentShapesManager().testMode)
                    {
                        //Play lock sound effectd
                        AudioSources.instance.PlayLockedSFX();
                        //Skip the next
                        return;
                    }
                }

                ShapesManager.Shape.selectedShapeID++;
                CreateShape();//Create new shape
            }
            else
            {
                if (ShapesManager.Shape.selectedShapeID == ShapesManager.GetCurrentShapesManager().shapes.Count - 1)
                {
                    UIEvents.instance.LoadAlbumScene();
                }
                else
                {
                    //Play lock sound effectd
                    AudioSources.instance.PlayLockedSFX();
                }
            }
        }

        /// <summary>
        /// Go to the previous shape.
        /// </summary>
        public void PreviousShape()
        {
            if (ShapesManager.Shape.selectedShapeID > 0 && ShapesManager.Shape.selectedShapeID < ShapesManager.GetCurrentShapesManager().shapes.Count)
            {
                ShapesManager.Shape.selectedShapeID--;
                CreateShape();
            }
            else
            {
                //Play lock sound effectd
                AudioSources.instance.PlayLockedSFX();
            }
        }

        /// <summary>
        /// Trace the current path
        /// </summary>
        private void TracePath()
        {
            if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.FILL)
            {
                if (selectedTracingPath.fillMethod == TracingPath.FillMethod.Radial)
                {
                    RadialFill();
                }
                else if (selectedTracingPath.fillMethod == TracingPath.FillMethod.Linear)
                {
                    LinearFill();
                }
                else if (selectedTracingPath.fillMethod == TracingPath.FillMethod.Point)
                {
                    PointFill();
                }
            }
            else if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.LINE)
            {
                DrawLine();
            }

            CheckPathComplete();
        }

        /// <summary>
        /// Radial fill tracing method.
        /// </summary>
        private void RadialFill()
        {
            clickPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = clickPostion - selectedTracingPath.curve.centroid;

            angleOffset = 0;
            clockWiseSign = (selectedTracingPath.fillImage.fillClockwise ? 1 : -1);

            if (selectedTracingPath.fillImage.fillMethod == Image.FillMethod.Radial360)
            {
                if (selectedTracingPath.fillImage.fillOrigin == 0)
                {//Bottom
                    angleOffset = 0;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 1)
                {//Right
                    angleOffset = clockWiseSign * 90;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 2)
                {//Top
                    angleOffset = -180;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 3)
                {//left
                    angleOffset = -clockWiseSign * 90;
                }
            }
            else if (selectedTracingPath.fillImage.fillMethod == Image.FillMethod.Radial90)
            {
                if (selectedTracingPath.fillImage.fillOrigin == 0)
                {//Bottom Left like path in 'a'
                    angleOffset = selectedTracingPath.fillImage.fillClockwise ? -90 : 0;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 3)
                {//Bottom Right like path in 'a' horinoal flipped
                    angleOffset = selectedTracingPath.fillImage.fillClockwise ? 0 : -90;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 2)
                {//Top Right like path in 'a' vertial flipped
                    angleOffset = selectedTracingPath.fillImage.fillClockwise ? 90 : -180;
                }
                else if (selectedTracingPath.fillImage.fillOrigin == 1)
                {//Top Left like path in 'a' vertial and horizontal flipped
                    angleOffset = selectedTracingPath.fillImage.fillClockwise ? -180 : 90;
                }
            }

            angle = Mathf.Atan2(-clockWiseSign * direction.x, -direction.y) * Mathf.Rad2Deg + angleOffset;

            if (angle < 0)
                angle += 360;

            angle = Mathf.Clamp(angle, 0, 360);

            if (quarterRestriction)
            {
                if (!(angle >= 0 && angle <= targetQuarter))
                {
                    fillAmount = selectedTracingPath.fillImage.fillAmount = 0;
                    return;
                }

                if (angle >= targetQuarter / 2)
                {
                    targetQuarter += 90;
                }
                else if (angle < 45)
                {
                    targetQuarter = 90;
                }

                targetQuarter = Mathf.Clamp(targetQuarter, 90, 360);
            }

            fillAmount = Mathf.Abs(angle / 360.0f);
            selectedTracingPath.fillImage.fillAmount = fillAmount;
        }

        /// <summary>
        /// Linear fill tracing method.
        /// </summary>
        private void LinearFill()
        {
            clickPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Rect rect = CommonUtil.RectTransformToWorldSpace(selectedTracingPath.GetComponent<RectTransform>());

            Vector3 pos1 = selectedTracingPath.curve.GetFirstPoint().position, pos2 = selectedTracingPath.curve.GetLastPoint().position;
            pos1.z = pos2.z = 0;

            clickPostion.x = Mathf.Clamp(clickPostion.x, Mathf.Min(pos1.x, pos2.x), Mathf.Max(pos1.x, pos2.x));
            clickPostion.y = Mathf.Clamp(clickPostion.y, Mathf.Min(pos1.y, pos2.y), Mathf.Max(pos1.y, pos2.y));
            clickPostion.z = 0;

            fillAmount = Vector2.Distance(clickPostion, pos1) / Vector2.Distance(pos1, pos2);
            selectedTracingPath.fillImage.fillAmount = fillAmount;
        }

        /// <summary>
        /// Point fill tracing method.
        /// </summary>
        private void PointFill()
        {
            fillAmount = 1;
            selectedTracingPath.fillImage.fillAmount = 1;
        }

        /// <summary>
        /// Draw line tracing method
        /// </summary>
        private void DrawLine()
        {
            //Get Line component
            if (selectedTracingPath.line != null)
            {
                selectedTracingPath.line.SetColor(currentPencil.color);
                //Add touch/click point into current line
                selectedTracingPath.line.AddPoint(selectedTracingPath.line.transform.InverseTransformPoint(GetCurrentPlatformClickPosition(Camera.main)));
                selectedTracingPath.line.BezierInterpolate(0.3f);
            }
        }

        /// <summary>
        /// Checks wehther path completed or not.
        /// </summary>
        private void CheckPathComplete()
        {
            if (selectedTracingPath.tracedPoints == selectedTracingPath.curve.points.Count)
            {
                if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.FILL)
                { //Fill Tracing Mode
                    if (fillAmount >= selectedTracingPath.completeOffset)
                    {
                        selectedTracingPath.completed = true;
                        selectedTracingPath.AutoFill();
                    }
                }
                else if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.LINE)
                {//Line Tracing Mode

                    if (Vector2.Distance(selectedTracingPath.curve.GetLastPoint().position, GetCurrentPlatformClickPosition(Camera.main)) < 0.5f)
                    {
                        selectedTracingPath.completed = true;
                    }
                }
            }

            if (selectedTracingPath.completed)
            {
                selectedTracingPath.AutoLineComplete();
                selectedTracingPath.OnComplete();

                ReleasePath();

                if (CheckShapeComplete())
                {
                    OnShapeComplete();
                }
                else
                {
                    AudioSources.instance.PlayCorrectSFX();
                }

                shape.ShowPathNumbers(shape.GetCurrentPathIndex());
            }
        }

        /// <summary>
        /// Check whether the shape completed or not.
        /// </summary>
        /// <returns><c>true</c>, if shape completed, <c>false</c> otherwise.</returns>
        private bool CheckShapeComplete()
        {
            bool shapeCompleted = true;
            foreach (TracingPath path in shape.tracingPaths)
            {
                if (!path.completed)
                {
                    shapeCompleted = false;
                    break;
                }
            }
            return shapeCompleted;
        }

        /// <summary>
        /// On shape completed event.
        /// </summary>
        private void OnShapeComplete()
        {
            shape.completed = true;

            bool allDone = true;

            List<Shape> shapes = new List<Shape>();

            if (compoundShape != null)
            {
                shapes = compoundShape.shapes;
                allDone = compoundShape.IsCompleted();

                if (!allDone)
                {
                    shape = compoundShape.shapes[compoundShape.GetCurrentShapeIndex()];
                    StartAutoTracing(shape, 1);
                }
            }
            else
            {
                shapes.Add(shape);
            }

            if (allDone)
            {
                if (handSparkles != null)
                    tempEmission.enabled = false;

                if (UserTraceInput.instance == null)
                {
                    SaveShapesData(shapes);
                }

                DisableHandTracing();
                HideUserTouchHand();
                //brightEffect.gameObject.SetActive(false);

                foreach (Shape s in shapes)
                {
                    if (hideShapeImageOnComplete)
                        s.content.GetComponent<Image>().enabled = false;
                    s.animator.SetTrigger("Completed");
                }

                Timer.instance.Stop();
                BlackArea.instance.Show();
                winDialog.Show();
                // nextButton.GetComponent<Animator>().SetTrigger("Select");
                winEffect.gameObject.SetActive(true);
                AudioSources.instance.PlayCompletedSFX();
                AdsManager.instance.HideAdvertisment();
                AdsManager.instance.ShowAdvertisment(AdPackage.AdEvent.Event.ON_SHOW_WIN_DIALOG, null);
            }
            else
            {
                AudioSources.instance.PlayCorrectSFX();
            }
        }

        /// <summary>
        /// Reset the shape.
        /// </summary>
        public void ResetShape()
        {
            List<Shape> shapes = new List<Shape>();
            if (compoundShape != null)
            {
                shapes = compoundShape.shapes;
            }
            else
            {
                shapes.Add(shape);
            }

            winEffect.gameObject.SetActive(false);
            // nextButton.GetComponent<Animator>().SetBool("Select", false);
            BlackArea.instance.Hide();
            winDialog.Hide();

            DisableHandTracing();

            foreach (Shape s in shapes)
            {
                if (s == null)
                    continue;

                s.completed = false;
                s.content.GetComponent<Image>().enabled = true;
                s.animator.SetBool("Completed", false);
                s.CancelInvoke();
                TracingPath[] paths = s.GetComponentsInChildren<TracingPath>();
                foreach (TracingPath path in paths)
                {
                    path.Reset();
                }

                if (compoundShape == null)
                {
                    StartAutoTracing(s, 2);
                }
                else if (compoundShape.GetShapeIndexByInstanceID(s.GetInstanceID()) == 0)
                {
                    shape = compoundShape.shapes[0];
                    StartAutoTracing(shape, 2);
                }
            }

            ReleasePath();

            Spell();

            if (UserTraceInput.instance == null)
                Timer.instance.Reset();
        }

        /// <summary>
        /// Save the data of the shapes such as (stars,path colors,unlock next shape...) .
        /// </summary>
        private void SaveShapesData(List<Shape> shapes)
        {
            if (shapes == null)
            {
                return;
            }

            if (shapes.Count == 0)
            {
                return;
            }

            //Save collected stars , stars of the shape
            ShapesManager.Shape.StarsNumber collectedStars = Progress.instance.starsNumber;
            DataManager.SaveShapeStars(ShapesManager.Shape.selectedShapeID, collectedStars, ShapesManager.GetCurrentShapesManager());

            int collectedStarsOffset = CommonUtil.ShapeStarsNumberEnumToIntNumber(collectedStars) - CommonUtil.ShapeStarsNumberEnumToIntNumber(ShapesManager.GetCurrentShapesManager().GetCurrentShape().starsNumber);
            ShapesManager.GetCurrentShapesManager().SetCollectedStars(collectedStarsOffset + ShapesManager.GetCurrentShapesManager().GetCollectedStars());

            ShapesManager.GetCurrentShapesManager().GetCurrentShape().starsNumber = collectedStars;

            //unlock the next shape
            if (ShapesManager.Shape.selectedShapeID + 1 < ShapesManager.GetCurrentShapesManager().shapes.Count)
            {
                DataManager.SaveShapeLockedStatus(ShapesManager.Shape.selectedShapeID + 1, false, ShapesManager.GetCurrentShapesManager());
                ShapesManager.GetCurrentShapesManager().shapes[ShapesManager.Shape.selectedShapeID + 1].isLocked = false;
            }

            Color tempColor = Colors.whiteColor;
            // save the colors of the paths
            int compundID = -1;
            foreach (Shape s in shapes)
            {
                if (compoundShape != null)
                {
                    //ID or Index of the shape in the compound shape
                    compundID = compoundShape.GetShapeIndexByInstanceID(s.GetInstanceID());
                }

                foreach (TracingPath p in s.tracingPaths)
                {
                    tempColor = ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.FILL ? p.fillImage.color : CommonUtil.GradientToColor(p.line.color);
                    DataManager.SaveShapePathColor(ShapesManager.Shape.selectedShapeID, compundID, p.from, p.to, tempColor, ShapesManager.GetCurrentShapesManager());
                }
            }
        }

        /// <summary>
        /// Starts the auto tracing for the current path.
        /// </summary>
        /// <param name="s">Shape Reference.</param>
        public void StartAutoTracing(Shape s, float traceDelay)
        {
            if (s == null)
            {
                return;
            }

            //Stop current movement
            DisableHandTracing();

            int currentPathIndex = s.GetCurrentPathIndex();

            if (currentPathIndex < 0 || currentPathIndex > s.tracingPaths.Count - 1)
                return;

            //Hide Numbers for other shapes , if we have compound shape
            if (compoundShape != null)
            {
                foreach (Shape ts in compoundShape.shapes)
                {
                    if (s.GetInstanceID() != ts.GetInstanceID())
                        ts.ShowPathNumbers(-1);
                }
            }

            if (s.tracingPaths.Count != 0)
            {
                //Set up the curve , and set position of Hand to the first point
                handPOM.curve = s.tracingPaths[currentPathIndex].curve;
                handPOM.transform.position = s.tracingPaths[currentPathIndex].curve.points[0].transform.position;
            }

            s.ShowPathNumbers(currentPathIndex);

            //Move the hand
            Invoke("EnableHandTracing", traceDelay);
        }

        /// <summary>
        /// Spell the shape.
        /// </summary>
        public void Spell()
        {
            if (ShapesManager.GetCurrentShapesManager().GetCurrentShape().clip == null || UserTraceInput.instance != null)
            {
                return;
            }

            AudioSources.instance.PlaySFXClip(ShapesManager.GetCurrentShapesManager().GetCurrentShape().clip, false);
        }

        /// <summary>
        /// Help the user.
        /// </summary>
        public void HelpUser()
        {
            int currentPathIndex = shape.GetCurrentPathIndex();

            if (currentPathIndex < 0 || currentPathIndex > shape.tracingPaths.Count - 1)
            {
                return;
            }

            selectedTracingPath = shape.tracingPaths[currentPathIndex];

            if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.FILL)
            {
                selectedTracingPath.fillImage.color = currentPencil.color.colorKeys[0].color;
                selectedTracingPath.AutoFill();
            }
            else if (ShapesManager.GetCurrentShapesManager().tracingMode == ShapesManager.TracingMode.LINE)
            {
                selectedTracingPath.line.SetColor(CommonUtil.ColorToGradient(currentPencil.color.colorKeys[0].color));
            }


            selectedTracingPath.completed = true;
            var shapeCompleted = CheckShapeComplete();

            if (shapeCompleted)
            {
                UnityEvent unityEvent = new UnityEvent();
                unityEvent.AddListener(() => OnShapeComplete());
                selectedTracingPath.AutoLineComplete(true, unityEvent);
            }
            else
            {
                selectedTracingPath.AutoLineComplete(true);
                selectedTracingPath.OnComplete();

                StartAutoTracing(shape, 1);
                AudioSources.instance.PlayCorrectSFX();
            }

            selectedTracingPath = null;
        }

        /// <summary>
        /// Reset the path.
        /// </summary>
        private void ResetPath()
        {
            if (selectedTracingPath != null)
                selectedTracingPath.Reset();
            ReleasePath();
        }

        /// <summary>
        /// Init The Curve of current tracing path
        /// (used using when the position of the shape is changed in the world space)
        /// </summary>
        public void ReInitTracingPathCurve()
        {
            if (handPOM != null && handPOM.curve.IsIinitialized())
            {
                handPOM.curve.ReInit();

                handPOM.Stop();
                handPOM.SetTarget(0);
                handPOM.Move();
            }
        }

        /// <summary>
        /// Release the path.
        /// </summary>
        private void ReleasePath()
        {
            selectedTracingPath = null;
            fillAmount = angleOffset = angle = 0;
            ResetTargetQuarter();
        }

        /// <summary>
        /// Reset the target quarter.
        /// </summary>
        private void ResetTargetQuarter()
        {
            targetQuarter = 90;
        }

        /// <summary>
        /// Enable the auto tracing of the hand.
        /// </summary>
        public void EnableHandTracing()
        {
            if (selectedTracingPath != null)
            {
                handPOM.curve = selectedTracingPath.curve;
            }

            if (handPOM.curve != null)
                handPOM.curve.Init();
            handPOM.MoveToStart();
        }

        /// <summary>
        /// Disable the auto tracing of the hand.
        /// </summary>
        public void DisableHandTracing()
        {
            CancelInvoke("EnableHandTracing");
            handPOM.Stop();
        }

        /// <summary>
        /// Show User's click/touch hand.
        /// </summary>
        public void ShowUserTouchHand()
        {
            hand.GetComponent<SpriteRenderer>().enabled = true;
        }

        /// <summary>
        /// Hide User's click/touch hand.
        /// </summary>
        public void HideUserTouchHand()
        {
            hand.GetComponent<SpriteRenderer>().enabled = false;
        }

        /// <summary>
        /// Draw the hand.
        /// </summary>
        /// <param name="clickPosition">Click position.</param>
        private void DrawHand(Vector3 clickPosition)
        {
            if (hand == null)
            {
                return;
            }

            hand.transform.position = clickPosition;
        }

        /// <summary>
        /// Set the size of the hand to default size.
        /// </summary>
        private void SetHandDefaultSize()
        {
            hand.transform.localScale = cursorDefaultSize;
        }

        /// <summary>
        /// Set the size of the hand to click size.
        /// </summary>
        private void SetHandClickSize()
        {
            hand.transform.localScale = cursorClickSize;
        }

        /// <summary>
        /// Draw the bright effect.
        /// </summary>
        /// <param name="clickPosition">Click position.</param>
        private void DrawBrightEffect(Vector3 clickPosition)
        {
            /*
            if (brightEffect == null) {
                return;
            }

            clickPosition.z = 0;
            brightEffect.transform.position = clickPosition;
            */
        }

        /// <summary>
        /// Hide/Disable Corner Timer Panel
        /// </summary>
        public void HideTimerPanel()
        {
            if (UserTraceInput.instance == null) return;

            Timer.instance.Stop();

            if (timerPanel != null)
                timerPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Set the color of the shape order.
        /// </summary>
        public void SetShapeOrderColor()
        {
            if (currentPencil == null)
            {
                return;
            }
            shapeOrder.color = currentPencil.color.colorKeys[0].color;
        }

        /// <summary>
        /// Disable the game manager.
        /// </summary>
        public void DisableGameManager()
        {
            isRunning = false;
        }

        /// <summary>
        /// Enable the game manager.
        /// </summary>
        public void EnableGameManager()
        {
            isRunning = true;
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        public void Pause()
        {
            if (!isRunning)
            {
                return;
            }

            if (Timer.instance != null)
                Timer.instance.Pause();
            DisableGameManager();
        }

        /// <summary>
        /// Resume the game.
        /// </summary>
        public void Resume()
        {
            if (Timer.instance != null && UserTraceInput.instance == null)
                Timer.instance.Resume();
            EnableGameManager();
        }
    }
}