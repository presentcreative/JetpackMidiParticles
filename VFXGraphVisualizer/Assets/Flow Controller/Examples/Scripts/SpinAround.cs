/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.examples
{
    using UnityEngine;

    public class SpinAround : MonoBehaviour
    {
        void Update()
        {
            this.transform.Rotate(new Vector3(90 * Time.deltaTime, 0, 0));
        }
    }
}
