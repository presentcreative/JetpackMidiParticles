/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.examples
{
    using Boids;
    using FlowControl;
    using UnityEngine;

    /// <summary>
    /// This creates a fixed number of randomly distrubuted boids for example purposes.
    /// </summary>
    public class BoidRandom : MonoBehaviour
    {
        public int BoidCount = 10;
        public GameObject BoidPrefab;
        public FlowControlRegion ControlRegion;

        private BoidRegion m_boidRegion;

        void Start()
        {
            m_boidRegion = GetComponent<BoidRegion>();

            for (int i = 0; i < BoidCount; i++)
            {
                GameObject newBoid = Instantiate(BoidPrefab);
                newBoid.GetComponent<Boid>().boidRegion = m_boidRegion;
                newBoid.GetComponent<Boid>().controlRegion = ControlRegion;
                newBoid.transform.parent = gameObject.transform;
                Vector3 pos = m_boidRegion.transform.position;
                Vector3 s = m_boidRegion.transform.lossyScale;
                newBoid.transform.position = new Vector3(
                        Random.Range(pos.x - s.x, pos.x + s.x),
                        Random.Range(pos.y - s.y, pos.y + s.y),
                        Random.Range(pos.z - s.z, pos.z + s.z)
                    );
            }
        }
    }

}
