/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl
{
#if UNITY_EDITOR
    using UnityEditor;
#endif

    using UnityEngine;
    using System;
    using System.Threading;
    using System.Collections.Generic;
    using uTween;
    using UtilityBelt;

    /// <summary>
    /// The Flow control region marks an AABB within which child spline curves can incluence
    /// a flow grid.
    /// </summary>
    [ExecuteInEditMode]
    [Serializable]
    public class FlowControlRegion : MonoBehaviour
    {
        private static bool enableMultithreaded = true;

        private class WorkGroup
        {
            [NonSerialized]
            public ManualResetEvent doneEvent;
            private int offsetX;
            private int span;
            private double maxMag2;
            private int m_resolution;
            private FieldCell[,,] field;
            private List<FieldCell> hasSplines;
            private float m_approach;
            private Vector3 m_min;
            private Vector3 tscale;

            public WorkGroup(int offsetX, int span, FieldCell[,,] field, int res, List<FieldCell> hasSplines, float approach, Vector3 min, Vector3 tscale)
            {
                if (span > 0)
                {
                    doneEvent = new ManualResetEvent(false);
                }
                else
                {
                    span = 1;
                }
                this.offsetX = offsetX;
                this.field = field;
                this.span = span;
                this.maxMag2 = (tscale.x * tscale.x * 4) + (tscale.y * tscale.y * 4) + (tscale.z * tscale.z * 4);
                this.hasSplines = hasSplines;
                this.m_resolution = res;
                this.m_approach = approach;

                this.m_min = min;

                this.tscale = tscale;
            }

            public void DistanceCallback(object o = null)
            {
                int x, y, z;

                float xmul = (tscale.x * 2 / m_resolution);
                float ymul = (tscale.y * 2 / m_resolution);
                float zmul = (tscale.z * 2 / m_resolution);

                float xmin = m_min.x + xmul / 2;
                float ymin = m_min.y + ymul / 2;
                float zmin = m_min.z + zmul / 2;

                for (x = offsetX; x < m_resolution; x += span)
                {
                    for (y = 0; y < m_resolution; y++)
                    {
                        for (z = 0; z < m_resolution; z++)
                        {

                            if (field[x, y, z].spline != null)
                            {
                                field[x, y, z].distance2ToSpline = 0;
                            }
                            else
                            {
                                Vector3 magD = new Vector3((xmin + x * xmul), (ymin + y * ymul), (zmin + z * zmul));
                                for (int i = 0; i < hasSplines.Count; i++)
                                {
                                    FieldCell cell = hasSplines[i];

                                    for (int j = 0; j < cell.Points.Count; j++)
                                    {
                                        Vector3 splinePoint = cell.Points[j];

                                        Vector3 mag = splinePoint - magD;

                                        float dist2 = (float)(mag.sqrMagnitude / maxMag2);

                                        if (dist2 < field[x, y, z].distance2ToSpline)
                                        {
                                            field[x, y, z].distance2ToSpline = dist2;
                                            float approach = Mathf.Min(1, m_approach * (1f - UTween.EaseOutCubic01(Mathf.Sqrt(dist2))));
                                            field[x, y, z].vecToSpline = Vector3.Lerp(mag.normalized, cell.Vector, approach);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (doneEvent != null)
                {
                    doneEvent.Set();
                }
            }

            public void VectorCallback(object o = null)
            {

                int x, y, z;

                for (x = offsetX; x < m_resolution; x += span)
                {
                    for (y = 0; y < m_resolution; y++)
                    {
                        for (z = 0; z < m_resolution; z++)
                        {
                            if (field[x, y, z].spline == null)
                            {
                                field[x, y, z].Vector = field[x, y, z].vecToSpline.normalized;
                            }
                        }
                    }
                }

                if (doneEvent != null)
                {
                    doneEvent.Set();
                }
            }
        } /* /Workgroup */

        private WorkGroup[] workGroups;
        private ManualResetEvent[] doneEvents;

        private class FieldCell
        {
            public Vector3 Vector;
            public Spline spline;
            public float distance2ToSpline;
            public Vector3 vecToSpline;
            public int x;
            public int y;
            public int z;
            public List<Vector3> Points = new List<Vector3>();

        }

        public enum OuterBehaviour
        {
            MoveToCenter = 0,
            ApplyVector = 1,
            AwayFromCenter = 2,
            MaintainDirection = 3,
            MoveToTransform = 4
        }

        public OuterBehaviour entryBehaviour;
        public OuterBehaviour exitBehaviour;
        public Vector3 outerEntryVector;
        public Vector3 outerExitVector;
        public Transform outerEntryTransform;
        public Transform outerExitTransform;

        public float forceDisplayCutoff = 0.6f;

        private List<FieldCell> hasSplines;

        [SerializeField]
        private int m_resolution = 10;
        public int Resolution
        {
            get { return m_resolution; }
            set
            {
                if (m_resolution == value)
                {
                    return;
                }
                m_resolution = value;
            }
        }

        [SerializeField]
        private float m_approach = 0.5f;
        public float Approach
        {
            get
            {
                return m_approach;
            }

            set
            {
                if (m_approach == value)
                {
                    return;
                }
                m_approach = value;
            }
        }

        [SerializeField]
        public bool ShowForces = true;

        [SerializeField]
        public bool ShowSplines = true;

        [SerializeField]
        public bool ShowTestPoints = true;

        [SerializeField]
        private Vector3[] m_field; /* This should be a multidimensional array, but Unity doesn't serialize those. Sigh. */
        private Color[] m_colours;

        [SerializeField]
        private Vector3 m_min;
        [SerializeField]
        private Vector3 m_max;

        private Vector3 VTWO = new Vector3(2, 2, 2);
        private float _lastSentinel;
        private bool _lockRegen;

        void OnValidate()
        {
            m_resolution = Math.Max(5, m_resolution);
            m_resolution = Math.Min(30, m_resolution);
            m_approach = Math.Max(0f, m_approach);
            m_approach = Math.Min(2.0f, m_approach);
        }

        public void AddSpline()
        {
            GameObject go = new GameObject();
            go.tag = "EditorOnly";
            go.name = GameObjectHelper.GenerateNextName(typeof(Spline), "Spline", gameObject);
            go.AddComponent<Spline>();
            go.transform.parent = transform;

        }


        private FieldCell World2Cell(FieldCell[,,] cells, Vector3 pos)
        {
            if (pos.x < m_min.x || pos.x > m_max.x || pos.y < m_min.y || pos.y > m_max.y || pos.z < m_min.z || pos.z > m_max.z)
            {
                return null;
            }

            float x = m_resolution * (pos.x - m_min.x) / (m_max.x - m_min.x);
            float y = m_resolution * (pos.y - m_min.y) / (m_max.y - m_min.y);
            float z = m_resolution * (pos.z - m_min.z) / (m_max.z - m_min.z);

            int maxRes = m_resolution - 1;
            return cells[
                Mathf.Clamp((int)x, 0, maxRes),
                Mathf.Clamp((int)y, 0, maxRes),
                Mathf.Clamp((int)z, 0, maxRes)];
        }

#if UNITY_EDITOR
        private void Regenerate()
        {
            int i, x, y, z;

            FieldCell[,,] field = new FieldCell[m_resolution, m_resolution, m_resolution];
            if(this.hasSplines == null)
            {
                this.hasSplines = new List<FieldCell>();
            }
            else
            {
                this.hasSplines.Clear();
            }

            for (x = 0; x < m_resolution; x++)
            {
                for (y = 0; y < m_resolution; y++)
                {
                    for (z = 0; z < m_resolution; z++)
                    {
                        field[x, y, z] = new FieldCell()
                        {
                            Vector = Vector3.zero,
                            spline = null,
                            distance2ToSpline = float.MaxValue,
                            x = x,
                            y = y,
                            z = z
                        };
                    }
                }
            }


            /* First, fill vectors that trace along the path of each spline that passes through the field. */
            Spline[] splines = GetComponentsInChildren<Spline>();
            if (splines.Length <= 0)
            {
                return;
            }

            int res3 = m_resolution * m_resolution * m_resolution;

            m_field = new Vector3[res3];
            m_colours = new Color[res3];

            /* Make sure our worldspace limits are up-to-date: */
            m_min = transform.position - transform.lossyScale;
            m_max = transform.position + transform.lossyScale;


            FieldCell lastHit = null;
            for (int j = 0; j < splines.Length; j++)
            {
                Spline spline = splines[j];

                spline.CrawlSpline((a, b) => {
                    FieldCell fc = World2Cell(field, a);
                    if (fc != null)
                    {
                        b -= a;
                        fc.Vector += b.normalized;
                        fc.spline = spline;
                        if (fc != lastHit)
                        {
                            this.hasSplines.Add(fc);
                            lastHit = fc;
                        }
                        lastHit.Points.Add(a);
                    }
                }, 60);
            }

            /* Secondly, calculate a crude path distance */

            if (enableMultithreaded)
            {
                int threadCount = Math.Max(1, SystemInfo.processorCount - 1);
                workGroups = new WorkGroup[threadCount];
                doneEvents = new ManualResetEvent[threadCount];

                int max1, max2;
                ThreadPool.GetMaxThreads(out max1, out max2);

                for (i = 0; i < threadCount; i++)
                {
                    workGroups[i] = new WorkGroup(i, threadCount, field, m_resolution, this.hasSplines, this.m_approach, m_min, transform.lossyScale);
                    doneEvents[i] = workGroups[i].doneEvent;
                    doneEvents[i].Reset();
                    ThreadPool.QueueUserWorkItem(workGroups[i].DistanceCallback);
                }
                WaitHandle.WaitAll(doneEvents);

                /* Thirdly, fill in the gaps */
                for (i = 0; i < threadCount; i++)
                {
                    doneEvents[i].Reset();
                    ThreadPool.QueueUserWorkItem(workGroups[i].VectorCallback);
                }
                WaitHandle.WaitAll(doneEvents);

            }
            else
            {
                WorkGroup wg = new WorkGroup(0, 0, field, m_resolution, this.hasSplines, this.m_approach, m_min, transform.lossyScale);
                wg.DistanceCallback();
                wg.VectorCallback();
            }

            /* Copy our new field into the serialized quaternion field */
            for (x = 0; x < m_resolution; x++)
            {
                for (y = 0; y < m_resolution; y++)
                {
                    for (z = 0; z < m_resolution; z++)
                    {
                        int idx = z + m_resolution * (y + m_resolution * x);
                        if (field[x, y, z].Vector == Vector3.zero)
                        {
                            m_field[idx] = Vector3.forward;
                            m_colours[idx] = Color.black;
                        }
                        else
                        {
                            m_field[idx] = field[x, y, z].Vector;
                            m_colours[idx] = Color.Lerp(Color.green, Color.red, 6 * Mathf.Sqrt(field[x, y, z].distance2ToSpline));
                        }
                    }
                }
            }
        }

        private void Start()
        {
            if (entryBehaviour == OuterBehaviour.MoveToTransform && outerEntryTransform == null)
            {
                Debug.LogWarning("Please assign a transform to the entry behaviour in MoveToTransform mode");
            }

            if (exitBehaviour == OuterBehaviour.MoveToTransform && outerExitTransform == null)
            {
                Debug.LogWarning("Please assign a transform to the exit behaviour in MoveToTransform mode");
            }
        }

        void OnDrawGizmos()
        {
            if (RegenerateRequired())
            {
                try
                {
                    if (_lockRegen)
                    {
                        _lastSentinel = 0;
                    }
                    else
                    {
                        _lockRegen = true;
                        Regenerate();
                    }
                }
                finally
                {
                    _lockRegen = false;
                }
            }

            Gizmos.color = Selection.activeGameObject == gameObject ? Color.yellow : Color.cyan;
            Gizmos.DrawWireCube(transform.position, Vector3.Scale(transform.lossyScale, VTWO));

            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.1f);
            Vector3 scal = transform.lossyScale;
            scal = new Vector3(scal.x - scal.x / m_resolution, scal.y - scal.y / m_resolution, scal.z - scal.z / m_resolution);
            Gizmos.DrawWireCube(transform.position, Vector3.Scale(scal, VTWO));

            if (!ShowForces || m_field == null)
            {
                return;
            }

            Vector3 min = m_min + (transform.lossyScale / m_resolution);

            Quaternion q = new Quaternion();

            float midsize = HandleUtility.GetHandleSize(this.gameObject.transform.position);

            for (int x = 0; x < m_resolution; x++)
            {
                for (int y = 0; y < m_resolution; y++)
                {
                    for (int z = 0; z < m_resolution; z++)
                    {
                        Vector3 pos = new Vector3(x,y,z);
                        pos.Scale(transform.lossyScale * 2 / m_resolution);
                        pos += min;

                        int idx = z + m_resolution * (y + m_resolution * x);

                        Handles.color = m_colours[idx];

                        if (m_colours[idx].r < forceDisplayCutoff || forceDisplayCutoff == 1f)
                        {
                            if (m_field[idx] != Vector3.zero)
                            {
                                q.SetLookRotation(m_field[idx]);
                            
                                Handles.ConeHandleCap(0, pos, q, midsize * 0.1f, EventType.Repaint); /* TODO: Draw these from back to front. */
                            }
                        }
                    }
                }
            }
        }

        private bool RegenerateRequired()
        {
            Sentinel sentinel = new Sentinel();

            sentinel.Add(gameObject.transform.lossyScale);
            sentinel.Add(m_resolution);
            sentinel.Add(m_approach);

            Spline[] splines = GetComponentsInChildren<Spline>();
            for (int i = 0; i < splines.Length; i++)
            {
                sentinel = splines[i].AddToSentinel(sentinel);
            }

            if(_lastSentinel != sentinel.checksum)
            {
                _lastSentinel = sentinel.checksum;
                return true;
            }

            return false;
        }
#endif

        /// <summary>
        /// For a given world position, calculate the force in the force control region. The force is
        /// quantized by the cell resolution, which is faster than an equivalent call to
        /// SampleWorldCoordSmooth
        /// </summary>
        /// <param name="pos">World position to sample</param>
        /// <param name="force">Output sampled value. A force of 0,0,0 means maintain velocity.</param>
        /// <param name="rot">A rotation from z forward to apply to an object transform.</param>
        /// <returns>true if the sample was inside the force control region.</returns>
        public bool SampleWorldCoord(Vector3 pos, out Vector3 force, bool isEntering = true)
        {
            Vector3 c = transform.position;
            Vector3 ext = transform.localScale;

            if (pos.x > (c.x + ext.x) || pos.x < m_min.x ||
                pos.y > (c.y + ext.y) || pos.y < m_min.y ||
                pos.z > (c.z + ext.z) || pos.z < m_min.z)
            {
                switch (isEntering?entryBehaviour:exitBehaviour)
                {
                    case OuterBehaviour.ApplyVector:
                        force = isEntering ? this.outerEntryVector : this.outerExitVector;
                        return false;

                    case OuterBehaviour.MoveToTransform:
                        force = (isEntering ? this.outerEntryTransform.position : this.outerExitTransform.position) - c;
                        return false;

                    case OuterBehaviour.AwayFromCenter:
                        pos = pos - c;
                        force = pos.normalized;
                        return false;

                    case OuterBehaviour.MaintainDirection:
                        force = Vector3.zero;
                        return false;

                    case OuterBehaviour.MoveToCenter:
                    default:
                        pos = c - pos;
                        force = pos.normalized;
                        return false;
                }
            }

            int maxr = m_resolution - 1;

            int ix = Mathf.Clamp((int)(((pos.x - m_min.x) / (2 * ext.x)) * m_resolution), 0, maxr);
            int iy = Mathf.Clamp((int)(((pos.y - m_min.y) / (2 * ext.y)) * m_resolution), 0, maxr);
            int iz = Mathf.Clamp((int)(((pos.z - m_min.z) / (2 * ext.z)) * m_resolution), 0, maxr);

        
            int idx = iz + m_resolution * (iy + m_resolution * ix);
#if UNITY_EDITOR
            if (idx < 0 || idx >= m_field.Length)
            {
                force = Vector3.zero;
                return true;
            }
#endif

            force = m_field[idx];
            return true;
        }

        /// <summary>
        /// For a given world position, calculate the force in the force control region. See also
        /// SampleWorldCoord which does the same but slightly faster, at the expense of the result
        /// being quantized.
        /// </summary>
        /// <param name="pos">World position to sample</param>
        /// <param name="force">Output sampled value. A force of 0,0,0 means maintain velocity.</param>
        /// <param name="rot">A rotation from z forward to apply to an object transform.</param>
        /// <returns>true if the sample was inside the force control region.</returns>
        public bool SampleWorldCoordSmooth(Vector3 pos, out Vector3 force, bool isEntering = true)
        {
            Vector3 cur = transform.position;
            Vector3 ext = transform.localScale;

            float xp = (((pos.x - m_min.x) / (2f * ext.x)) * (float)m_resolution) - 0.5f;
            float yp = (((pos.y - m_min.y) / (2f * ext.y)) * (float)m_resolution) - 0.5f;
            float zp = (((pos.z - m_min.z) / (2f * ext.z)) * (float)m_resolution) - 0.5f;

            int resMinus1 = m_resolution - 1;
            bool cantSmooth = (xp < 0 || xp >= resMinus1 || yp < 0 || yp >= resMinus1 || zp < 0 || zp >= resMinus1);

            if (cantSmooth ||
                pos.x > (cur.x + ext.x) || pos.x < m_min.x ||
                pos.y > (cur.y + ext.y) || pos.y < m_min.y ||
                pos.z > (cur.z + ext.z) || pos.z < m_min.z)
            {
                switch (isEntering ? entryBehaviour : exitBehaviour)
                {
                    case OuterBehaviour.ApplyVector:
                        force = isEntering ? this.outerEntryVector : this.outerExitVector;
                        return false;

                    case OuterBehaviour.MoveToTransform:
                        force = (isEntering ? this.outerEntryTransform.position : this.outerExitTransform.position) - cur;
                        return false;

                    case OuterBehaviour.AwayFromCenter:
                        pos = pos - cur;
                        force = pos.normalized;
                        return false;

                    case OuterBehaviour.MaintainDirection:
                        force = Vector3.zero;
                        return false;

                    case OuterBehaviour.MoveToCenter:
                    default:
                        pos = cur - pos;
                        force = pos.normalized;
                        return false;
                }
            }

            int maxr = m_resolution - 1;

            float xlerp = xp - Mathf.Floor(xp);
            float ylerp = yp - Mathf.Floor(yp);
            float zlerp = zp - Mathf.Floor(zp);

            int ix = Mathf.Clamp((int)xp, 0, maxr);
            int iy = Mathf.Clamp((int)yp, 0, maxr);
            int iz = Mathf.Clamp((int)zp, 0, maxr);


            /*
             *        F
             *       / \
             *      G   B
             *      |\ /|
             *      | H |
             *      |   |
             *      | E |
             *      |/ \|
             *      A   D
             *       \ /
             *        C
             *
             * ac; x +ve
             * cd; z +ve
             * ag; y +ve
             */

            int y1x1 = m_resolution * (iy + m_resolution * ix);
            int y1x2 = m_resolution * (iy + m_resolution * (ix + 1));
            int y2x1 = m_resolution * ((iy + 1) + m_resolution * ix);
            int y2x2 = m_resolution * ((iy + 1) + m_resolution * (ix + 1));

            int iz2 = iz + 1;

            Vector3 a = m_field[iz + y1x1];
            Vector3 c = m_field[iz + y1x2];
            Vector3 e = m_field[iz2 + y1x1];
            Vector3 d = m_field[iz2 + y1x2];
            Vector3 g = m_field[iz + y2x1];
            Vector3 h = m_field[iz + y2x2];
            Vector3 f = m_field[iz2 + y2x1];
            Vector3 b = m_field[iz2 + y2x2];

            Vector3 ac = Vector3.Lerp(a, c, xlerp);
            Vector3 ed = Vector3.Lerp(e, d, xlerp);
            Vector3 gh = Vector3.Lerp(g, h, xlerp);
            Vector3 fb = Vector3.Lerp(f, b, xlerp);

            Vector3 aced = Vector3.Lerp(ac, ed, zlerp);
            Vector3 ghfb = Vector3.Lerp(gh, fb, zlerp);

            force = Vector3.Lerp(aced, ghfb, ylerp);
            
            return true;
        }


        /// <param name="pos">World position to sample</param>
        /// <param name="force">Output sampled value. A force of 0,0,0 means maintain velocity.</param>
        /// <param name="rot">A rotation from z forward to apply to an object transform.</param>
        /// <returns>true if the sample was inside the force control region.</returns>
        public bool SampleWorldCoordWithRotation(Vector3 pos, out Vector3 force, out Quaternion rot, bool isEntering = true)
        {
            Vector3 c = transform.position;
            Vector3 ext = transform.localScale;

            if (pos.x > (c.x + ext.x) || pos.x < m_min.x ||
                pos.y > (c.y + ext.y) || pos.y < m_min.y ||
                pos.z > (c.z + ext.z) || pos.z < m_min.z)
            {
                switch (isEntering ? entryBehaviour : exitBehaviour)
                {
                    case OuterBehaviour.ApplyVector:
                        force = (isEntering ? this.outerEntryVector : this.outerExitVector).normalized;
                        rot = Quaternion.FromToRotation(Vector3.forward, force);
                        return false;

                    case OuterBehaviour.MoveToTransform:
                        force = ((isEntering ? this.outerEntryTransform.position : this.outerExitTransform.position) - c).normalized;
                        rot = Quaternion.FromToRotation(Vector3.forward, force);
                        return false;

                    case OuterBehaviour.AwayFromCenter:
                        force = (pos - c).normalized;
                        rot = Quaternion.FromToRotation(Vector3.forward, force);
                        return false;

                    case OuterBehaviour.MaintainDirection:
                        force = Vector3.zero;
                        rot = Quaternion.identity;
                        return false;

                    case OuterBehaviour.MoveToCenter:
                    default:
                        force = (c - pos).normalized;
                        rot = Quaternion.FromToRotation(Vector3.forward, force);
                        return false;
                }
            }

            int maxr = m_resolution - 1;

            int ix = Mathf.Clamp((int)(((pos.x - m_min.x) / (2 * ext.x)) * m_resolution), 0, maxr);
            int iy = Mathf.Clamp((int)(((pos.y - m_min.y) / (2 * ext.y)) * m_resolution), 0, maxr);
            int iz = Mathf.Clamp((int)(((pos.z - m_min.z) / (2 * ext.z)) * m_resolution), 0, maxr);


            int idx = iz + m_resolution * (iy + m_resolution * ix);
#if UNITY_EDITOR
            if (idx < 0 || idx >= m_field.Length)
            {
                force = Vector3.zero;
                rot = Quaternion.identity;
                return true;
            }
#endif

            force = m_field[idx];
            rot = Quaternion.FromToRotation(Vector3.forward, force);
            return true;
        }
    }
}
