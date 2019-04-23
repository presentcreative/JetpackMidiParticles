/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl
{
    using System;
    using UnityEngine;
    using UtilityBelt;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [ExecuteInEditMode]
    [Serializable]
    public class Spline : MonoBehaviour
    {
        private int m_resolution = 20; /* Not currently editable */

        [SerializeField]
        private bool m_isClosed;
        public bool IsClosed
        {
            get
            {
                return m_isClosed;
            }
            set
            {
                if (m_isClosed != value)
                {
                    m_isClosed = value;
                }
            }
        }

        public void AddPoint(Vector3? pos = null, Vector3? departTowards = null)
        {
            if(!pos.HasValue || !departTowards.HasValue)
            {
                pos = new Vector3(1, 1, 1);
                departTowards = new Vector3(2, 2, 2);
                /* TODO: Take this from the last point, extended. */
            }

            GameObject go = new GameObject();
            go.transform.position = pos.Value;
            go.name = GameObjectHelper.GenerateNextName(typeof(SplinePoint), "Point", gameObject);
            SplinePoint sp = go.AddComponent<SplinePoint>();
            sp.HandleDepart = departTowards.Value;
            go.transform.parent = transform;
        }


        /// <summary>
        /// Find a point between two spline points in world-space
        /// </summary>
        /// <param name="t">0..1</param>
        public Vector3 GetPointOnCurve(SplinePoint from, SplinePoint to, float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 b = from.transform.TransformPoint(from.HandleDepart);
            Vector3 c = to.transform.TransformPoint(to.HandleApproach);

            return Vector3.Lerp(
                Vector3.Lerp(
                    Vector3.Lerp(from.transform.position, b, t),
                    Vector3.Lerp(b, c, t),
                    t),
                Vector3.Lerp(
                    Vector3.Lerp(b, c, t),
                    Vector3.Lerp(c, to.transform.position, t),
                    t),
                t
                );
        }

        /// <summary>
        /// Call a function along points on the curve in world-splace
        /// </summary>
        public void CrawlSpline(Action<Vector3, Vector3> cb, int resolution)
        {
            SplinePoint from, to;
            Vector3 lastPoint;
            int max = resolution + 1;

            SplinePoint[] points = gameObject.GetComponentsInChildren<SplinePoint>();

            for (int i = 0; i < points.Length - 1; i++)
            {
                from = points[i];
                to = points[i + 1];
                if (from == null || to == null)
                {
                    return;
                }

                lastPoint = from.transform.position;

                for (int j = 1; j < max; j++)
                {
                    Vector3 currentPoint = GetPointOnCurve(from, to, j / (float)resolution);
                    cb(lastPoint, currentPoint);
                    lastPoint = currentPoint;
                }
            }

            if(m_isClosed)
            {
                from = points[points.Length - 1];
                to = points[0];
                if (from == null || to == null)
                {
                    return;
                }

                lastPoint = from.transform.position;

                for (int j = 1; j < max; j++)
                {
                    Vector3 currentPoint = GetPointOnCurve(from, to, j / (float)resolution);
                    cb(lastPoint, currentPoint);
                    lastPoint = currentPoint;
                }
            }
        }

        public bool ShowSplines
        {
            get
            {
                FlowControlRegion fcr = GetComponentInParent<FlowControlRegion>();
                return (fcr == null || fcr.ShowSplines);
            }
        }

        public SplinePoint[] Points
        {
            get
            {
                return gameObject.GetComponentsInChildren<SplinePoint>();
            }
        }


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                SplinePoint[] points = gameObject.GetComponentsInChildren<SplinePoint>();

                if (points.Length < 2)
                {
                    AddPoint(new Vector3(0f, 0f, 0f), new Vector3(0.2f, 0f, 0f));
                    AddPoint(new Vector3(1f, 1f, 0f), new Vector3(1f, 1.2f, 0f));
                }
            }

            if (ShowSplines)
            {
                Gizmos.color = Selection.activeGameObject == gameObject ? Color.yellow : Color.magenta;
                CrawlSpline((a, b) => {
                    Gizmos.DrawLine(a, b);
                }, m_resolution);
            }
        }

        internal Sentinel AddToSentinel(Sentinel sentinel)
        {
            sentinel.Add(transform);
            sentinel.Add(m_isClosed);
            SplinePoint[] points = gameObject.GetComponentsInChildren<SplinePoint>();
            for (int i = 0; i < points.Length; i++)
            {
                sentinel = points[i].AddToSentinel(sentinel);
            }
            return sentinel;
        }

#endif
    }
}
