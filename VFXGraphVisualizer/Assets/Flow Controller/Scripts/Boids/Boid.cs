/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.Boids
{
    using UnityEngine;

    /// <summary>
    /// Boid marker component for example purposes.
    /// </summary>
    public class Boid : MonoBehaviour
    {
        public BoidRegion boidRegion;
        public FlowControlRegion controlRegion;
        private Vector3 velocity;

        void Start()
        {
            velocity = Vector3.zero;
        }


        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }
    }
}
