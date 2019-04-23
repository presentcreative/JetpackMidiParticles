/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControlEditor
{
    using UnityEngine;
    using UnityEditor;
    using FlowControl;
    using UtilityBelt;
    using FlowControl.Emitters;

    [CustomEditor(typeof(SplineEmitter))]
    public class SplineEmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Spline"))
            {
                GameObject go = new GameObject();
                go.name = GameObjectHelper.GenerateNextName(typeof(Spline), "Spline", ((SplineEmitter)target).gameObject);
                go.AddComponent<Spline>();
                go.transform.parent = ((SplineEmitter)target).transform;
            }
        }

    }
}
