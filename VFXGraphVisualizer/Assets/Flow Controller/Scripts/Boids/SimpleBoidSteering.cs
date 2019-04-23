/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.Boids
{
    using UnityEngine;

    /// <summary>
    /// Not really boids at all. Naively moves game objects along the vectors dictated
    /// by the flow control region.
    /// </summary>
    public class SimpleBoidSteering : MonoBehaviour
    {
        private Boid m_boidSelf;

        public float Speed = 2f;

        void Start()
        {
            m_boidSelf = GetComponent<Boid>();
        }

        void Update()
        {
            FlowControlRegion control = m_boidSelf.controlRegion;

            if (control != null)
            {
                Vector3 dir;
                control.SampleWorldCoord(transform.position, out dir);
                dir *= Time.deltaTime * Speed;

                transform.position = transform.position + dir;
            }

        }
    }
}
