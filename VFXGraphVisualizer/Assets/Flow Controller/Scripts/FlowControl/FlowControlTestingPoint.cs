/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl
{
    using UnityEngine;
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    /// <summary>
    /// Add this component to a GameObject and make it a child of a flow control region to have
    /// an in-editor way of quickly checking the behaviour of a flow control region.
    /// </summary>
    [ExecuteInEditMode]
    [Serializable]
    public class FlowControlTestingPoint : MonoBehaviour
    {
        public float inertia = 0.1f;
        public float step = 0.1f;
        public int iterations = 80;
        public bool Smooth = false;

        private Vector3 momentum;

        void OnValidate()
        {
            step = Math.Max(0.01f, step);
            step = Math.Min(1, step);
            iterations = Math.Max(1, iterations);
            iterations = Math.Min(200, iterations);
            inertia = Math.Max(0, inertia);
            inertia = Math.Min(1, inertia);
        }

        public bool ShowTestingPoints
        {
            get
            {
                FlowControlRegion ffr = GetComponentInParent<FlowControlRegion>();
                return (ffr != null && ffr.ShowTestPoints);
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            FlowControlRegion ffr = GetComponentInParent<FlowControlRegion>();

            if (ffr == null || !ffr.ShowTestPoints)
            {
                return;
            }


            momentum = Vector3.zero;

            Gizmos.color = Color.green;

            Vector3 p = transform.position;

            float midsize = HandleUtility.GetHandleSize(p) / 10f;
            Gizmos.DrawSphere(p, midsize);

            bool exiting = false;

            for (int i = 0; i < iterations; i++)
            {
                Vector3 dir;
                if (Smooth)
                {
                    exiting = ffr.SampleWorldCoordSmooth(p, out dir, !exiting) || exiting;
                }
                else
                {
                    exiting = ffr.SampleWorldCoord(p, out dir, !exiting) || exiting;
                }

                Vector3 impetus = dir * step + momentum;

                momentum = impetus * inertia;

                Vector3 to = p + impetus;

                Gizmos.DrawLine(p, to);

                p = to;
            }

        }
#endif


    }
}
