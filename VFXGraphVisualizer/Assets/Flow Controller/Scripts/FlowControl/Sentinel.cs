/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

using System;
using UnityEngine;

namespace com.kupio.FlowControl
{
    /// <summary>
    /// A sentinel value detects changes in a scene by boiling GameObject values down to a single number.
    /// </summary>
    struct Sentinel
    {
        public float checksum;

        internal void Add(Vector3 v)
        {
            checksum = checksum + v.x + v.y + v.z;
        }

        internal void Add(Quaternion q)
        {
            checksum = checksum + q.w + q.x + q.y + q.z;
        }

        internal void Add(Transform transform)
        {
            Add(transform.lossyScale);
            Add(transform.position);
        }

        internal void Add(int i)
        {
            checksum += i;
        }

        internal void Add(float f)
        {
            checksum += f;
        }

        internal void Add(bool b)
        {
            checksum += (b?1:0);
        }
    }
}
