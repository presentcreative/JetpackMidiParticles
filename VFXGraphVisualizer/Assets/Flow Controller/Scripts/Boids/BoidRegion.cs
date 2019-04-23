/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.Boids
{
    using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    /// <summary>
    /// A boid region contains Boid component objects.
    /// </summary>
    public class BoidRegion : MonoBehaviour
    {
        private List<Boid> m_boids;

        void Start()
        {
            m_boids = null;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, Vector3.Scale(transform.lossyScale, new Vector3(2, 2, 2)));
        }
#endif

        internal void Register(Boid boid)
        {
            if (m_boids == null)
            {
                m_boids = new List<Boid>();
            }
            m_boids.Add(boid);
        }

        public List<Boid> GetBoids()
        {
            return m_boids;
        }
    }
}
