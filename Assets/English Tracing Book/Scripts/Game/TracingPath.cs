using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using IndieStudio.EnglishTracingBook.Utility;
using UnityEngine.Events;

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
    [RequireComponent(typeof(LineAnimation))]
    public class TracingPath : MonoBehaviour
    {
        /// <summary>
        /// Line Animation component used to animate the line of tracing paths e.g on user hint or help
        /// </summary>
        private LineAnimation lineAnimation;

        /// <summary>
        /// Whether the path is completed or not.
        /// </summary>
        [HideInInspector]
        public bool completed;

        /// <summary>
        /// The fill method (Radial or Linear or Point).
        /// </summary>
        public FillMethod fillMethod;

        /// <summary>
        /// The complete offset (The fill amount offset).
        /// </summary>
        public float completeOffset = 0.85f;

        /// <summary>
        /// The first number reference.
        /// </summary>
        public Transform firstNumber;

        /// <summary>
        /// The second number reference.
        /// </summary>
        public Transform secondNumber;

        /// <summary>
        /// The shape reference.
        /// </summary>
        [HideInInspector]
        public Shape shape;

        /// <summary>
        /// The curve of the tracing path
        /// </summary>
        [HideInInspector]
        public Curve curve;

        /// <summary>
        /// The fill image
        /// </summary>
        [HideInInspector]
        public Image fillImage;

        /// <summary>
        /// The count of the traced points
        /// </summary>
        [HideInInspector]
        public int tracedPoints;

        /// <summary>
        /// The line of the tracing path
        /// </summary>
        [HideInInspector]
        public Line line;

        /// <summary>
        /// From : First Number Value, To : Second Number Value
        /// </summary>
        [HideInInspector]
        public int from, to;

        void Awake()
        {
            lineAnimation = GetComponent<LineAnimation>();

            //Requires LineAnimation component
            if (lineAnimation == null)
            {
                lineAnimation = gameObject.AddComponent<LineAnimation>();
            }

            string[] slices = gameObject.name.Split('-');
            from = int.Parse(slices[1]);
            to = int.Parse(slices[2]);

            shape = GetComponentInParent<Shape>();
            curve = GetComponentInChildren<Curve>();
            fillImage = CommonUtil.FindChildByTag(transform, "Fill").GetComponent<Image>();
            fillImage.fillAmount = 0;
            tracedPoints = 0;

            if (SceneManager.GetActiveScene().name == "Game" && GameManager.instance == null)
                GameManager.instance = FindFirstObjectByType<GameManager>();

            //create new line
            CreateNewLine();
        }

        void Start()
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (GameManager.instance.animateNumbersOnStart && GameManager.instance.compoundShape == null)
                {
                    //Animate the numbers of the Path
                    firstNumber.transform.position = secondNumber.transform.position = Vector3.zero;
                    TransformFollow2D firstNTF = firstNumber.gameObject.AddComponent<TransformFollow2D>();
                    TransformFollow2D secondtNTF = secondNumber.gameObject.AddComponent<TransformFollow2D>();

                    firstNTF.target = curve.GetFirstPoint();
                    secondtNTF.target = curve.GetLastPoint();

                    firstNTF.targetMode = secondtNTF.targetMode = TransformFollow2D.TargetMode.TRANSFORM;
                    firstNTF.speed = secondtNTF.speed = 6;
                    firstNTF.transform.position = secondtNTF.transform.position = Vector3.zero;
                    firstNTF.isRunning = secondtNTF.isRunning = true;
                }
            }

            //Change numbers parent and sibling to be visible on the top
            RectTransform firstNumberRectTransfrom = firstNumber.GetComponent<RectTransform>();
            RectTransform secondNumberRectTransfrom = secondNumber.GetComponent<RectTransform>();

            firstNumberRectTransfrom.GetComponent<RectTransform>().SetParent(transform.parent);
            secondNumberRectTransfrom.GetComponent<RectTransform>().SetParent(transform.parent);

            firstNumberRectTransfrom.SetAsLastSibling();
            secondNumberRectTransfrom.SetAsLastSibling();
        }


        /// <summary>
        /// Create new line
        /// </summary>
        private void CreateNewLine()
        {
            if (ShapesManager.GetCurrentShapesManager().tracingMode != ShapesManager.TracingMode.LINE)
            {
                //skip if tracing mode not LINE
                return;
            }

            //disable fill image
            fillImage.enabled = false;

            GameObject linePrefab = null;
            if (SceneManager.GetActiveScene().name == "Game")
            {
                linePrefab = GameManager.instance.linePrefab;
            }
            else if (SceneManager.GetActiveScene().name == "Album")
            {
                linePrefab = ShapesTable.instance.linePrefab;
            }

            if (linePrefab != null)
            {
                GameObject lineGO = Instantiate(linePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                lineGO.transform.SetParent(transform);
                Vector3 temp = lineGO.transform.localPosition;
                temp.z = -500;
                lineGO.transform.localPosition = temp;
                lineGO.transform.localScale = Vector3.one;
                line = lineGO.GetComponent<Line>();

                SetUpLineWidth();
            }
        }

        public void SetUpLineWidth()
        {
            if (line == null)
                return;

            CompoundShape cs = GetComponentInParent<CompoundShape>();

            //Change line width ratio as you want from below in Game,Album scenes
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (cs == null && UserTraceInput.instance == null)
                {
                    //shape line width
                    line.SetWidth(shape.content.localScale.magnitude * ShapesManager.GetCurrentShapesManager().GameShapeLineWidth);
                }
                else
                {
                    //compound shape line width

                    if (cs == null)
                    {
                        line.SetWidth(transform.GetComponentInParent<Shape>().transform.localScale.magnitude * ShapesManager.GetCurrentShapesManager().GameCompoundShapeLineWidth);
                    }
                    else
                    {
                        if (cs.shapes.Count != 0)
                            line.SetWidth(cs.shapes[0].transform.localScale.magnitude * ShapesManager.GetCurrentShapesManager().GameCompoundShapeLineWidth);
                    }

                }
            }
            else if (SceneManager.GetActiveScene().name == "Album")
            {
                if (cs == null)
                {
                    //shape line width
                    line.SetWidth(shape.content.localScale.magnitude * ShapesManager.GetCurrentShapesManager().AlbumShapeLineWidth);
                }
                else
                {
                    //compound shape line width
                    if (cs.shapes.Count != 0)
                        line.SetWidth(cs.shapes[0].transform.localScale.magnitude * ShapesManager.GetCurrentShapesManager().AlbumCompoundShapeLineWidth);
                }
            }

        }

        /// <summary>
        /// Auto fill.
        /// </summary>
        public void AutoFill()
        {
            if (Mathf.Approximately(fillImage.fillAmount, 1))
            {
                return;
            }

            StartCoroutine("AutoFillCoroutine");
        }


        /// <summary>
        /// Auto fill coroutine.
        /// </summary>
        /// <returns>The fill coroutine.</returns>
        private IEnumerator AutoFillCoroutine()
        {
            while (fillImage.fillAmount < 1)
            {
                fillImage.fillAmount += 0.02f;
                yield return new WaitForSeconds(0.001f);
            }

        }

        /// <summary>
        /// Set the status of the numbers.
        /// </summary>
        /// <param name="status">the status value.</param>
        public void SetNumbersStatus(bool status)
        {
            StartCoroutine(SetNumbersStatusCoroutine(status));
        }

        private IEnumerator SetNumbersStatusCoroutine(bool status)
        {
            yield return 0;

            Transform[] numbers = new Transform[] { firstNumber, secondNumber };

            Color tempColor = Color.white;
            Animator animator = null;
            foreach (Transform number in numbers)
            {
                if (number == null)
                    continue;

                animator = number.GetComponent<Animator>();
                yield return new WaitUntil(() => animator.isInitialized);

                if (status == true)
                {
                    animator.SetBool("Select", true);
                    tempColor.a = 1;
                }
                else
                {
                    if (shape.enablePriorityOrder)
                    {
                        animator.SetBool("Select", false);
                        tempColor.a = 0.3f;
                    }
                }

                number.GetComponent<Image>().color = tempColor;
            }
        }

        /// <summary>
        /// Set the visibility of the  numbers.
        /// </summary>
        /// <param name="visible">visibility value.</param>
        public void SetNumbersVisibility(bool visible)
        {
            if (firstNumber != null)
            {
                firstNumber.gameObject.SetActive(visible);
            }
            if (secondNumber != null)
            {
                if (fillMethod == FillMethod.Point)
                {
                    secondNumber.gameObject.SetActive(false);
                }
                else
                {
                    secondNumber.gameObject.SetActive(visible);
                }
            }
        }

        /// <summary>
        /// Disable the current point
        /// </summary>
        public void DisableCurrentPoint()
        {
            curve.SetPointActiveValue(false, tracedPoints);
        }

        /// <summary>
        /// Enable the current point
        /// </summary>
        public void EnableCurrentPoint()
        {
            curve.SetPointActiveValue(true, tracedPoints);
        }

        /// <summary>
        /// Reset the path.
        /// </summary>
        public void Reset()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (line != null)
            {
                line.Reset();
            }

            tracedPoints = 0;
            curve.DisablePoints();
            EnableCurrentPoint();

            SetNumbersVisibility(true);
            completed = false;

            if (!shape.enablePriorityOrder)
            {
                SetNumbersStatus(true);
            }
            StartCoroutine("ReleaseFillCoroutine");
        }


        /// <summary>
        /// Release Fill coroutine.
        /// </summary>
        /// <returns>The coroutine.</returns>
        private IEnumerator ReleaseFillCoroutine()
        {
            while (fillImage.fillAmount > 0)
            {
                fillImage.fillAmount -= 0.02f;
                yield return new WaitForSeconds(0.005f);
            }
        }

        /// <summary>
        /// On complete the tracing path
        /// </summary>
        public void OnComplete()
        {
            SetNumbersVisibility(false);
        }

        /// <summary>
        /// Auto line drawing or complete for the path
        /// </summary>
        public void AutoLineComplete(bool animated = false, UnityEvent animationDoneEvent = null)
        {
            if (line == null)
            {
                return;
            }

            line.Reset();
            Vector3 inversePoint;
            List<Vector3> curvePoints = new List<Vector3>();

            foreach (Transform point in curve.points)
            {
                inversePoint = line.transform.InverseTransformPoint(point.position);
                curvePoints.Add(inversePoint);

                if (fillMethod == FillMethod.Point && curve.points.Count == 1)
                {
                    curvePoints.Add(inversePoint);
                }
            }

            if (!animated)
            {
                //without animation / instantly
                foreach (Vector3 point in curvePoints)
                {
                    line.AddPoint(point);
                }

                line.BezierInterpolate(curve.smoothness);
            }
            else
            {
                //with animation / delay
                Line.bezier.Interpolate(curvePoints, curve.smoothness);
                lineAnimation.Run(Line.bezier.GetDrawingPoints2(), line, animationDoneEvent);
            }
        }

        public enum FillMethod
        {
            Radial,
            Linear,
            Point
        }
    }
}