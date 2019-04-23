/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.examples
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// This switches a particle controller's flow region once every few seconds to show that
    /// multiple flows can exist in a scene and can be switched between.
    /// </summary>
    public class SwitchFlows : MonoBehaviour
    {
        public FlowControlRegion[] regions;

        private int next = 0;

        void Start()
        {
            StartCoroutine(SwitchFlowFn());
        }

        private IEnumerator SwitchFlowFn()
        {
            WaitForSeconds wfs = new WaitForSeconds(3f);

            for (;;)
            {
                next++;
                if (next >= regions.Length)
                {
                    next = 0;
                }
                GetComponent<ParticleFlowController>().flowControlRegion = regions[next];
                yield return wfs;
            }
        }
    }
}
