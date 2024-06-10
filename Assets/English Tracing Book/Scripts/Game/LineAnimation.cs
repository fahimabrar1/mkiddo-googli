using System.Collections.Generic;
using UnityEngine;
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
    public class LineAnimation : MonoBehaviour
    {
        /// <summary>
        /// List of points of the line to animate line through them
        /// </summary>
        private List<Vector3> points;

        /// <summary>
        /// Whether to animate the line or not
        /// </summary>
        private bool animateLine = false;

        /// <summary>
        /// An index used as a pointer on the points in list
        /// </summary>
        private int currentPointIndex;

        /// <summary>
        /// Current Point (where we are) , Traget Point (move to this point)
        /// </summary>
        private Vector3 currentPoint, targetPoint;

        /// <summary>
        /// Line Reference (contains Line Renderer, Line Z Poition ..etc )
        /// </summary>
        private Line line;

        /// <summary>
        /// Animation Speed
        /// </summary>
        private float speed = 500;

        /// <summary>
        /// An unity event invoked when the animation is done or finished
        /// </summary>
        private UnityEvent animationDoneEvent;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!animateLine)
            {
                return;
            }

            if (currentPointIndex < points.Count - 1)
            {
                if (Vector2.Distance(currentPoint, targetPoint) > 0.1f)
                {
                    //move until you reach target point
                    currentPoint = Vector2.MoveTowards(currentPoint, targetPoint, Time.deltaTime * speed);
                    currentPoint.z = line.pointZPosition;

                    //update point position in line renderer
                    line.lineRenderer.SetPosition(currentPointIndex + 1, currentPoint);
                }
                else
                {
                    //set last point in the line renderer as the target point position
                    line.lineRenderer.SetPosition(currentPointIndex + 1, targetPoint);

                    //move to next point

                    currentPoint = targetPoint;
                    currentPointIndex++;

                    if (currentPointIndex + 1 < points.Count)
                    {
                        line.AddPoint(targetPoint);
                        targetPoint = points[currentPointIndex + 1];
                        targetPoint.z = line.pointZPosition;
                    }

                }

            }
            else
            {
                //stop animation (already we are done)
                animateLine = false;

                if(animationDoneEvent!=null)
                    animationDoneEvent.Invoke();
            }
        }

        public void Run(List<Vector3> points, Line line,UnityEvent animationDoneEvent = null)
        {
            // init values , and start line animation 

            this.points = points;
            this.line = line;
            this.animationDoneEvent = animationDoneEvent;

            line.AddPoint(points[0]);
            line.AddPoint(points[0]);

            currentPoint = points[0];
            targetPoint = points[1];

            currentPoint.z = targetPoint.z = line.pointZPosition;

            currentPointIndex = 0;

            animateLine = true;
        }
    }
}