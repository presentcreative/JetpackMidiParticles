/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControlEditor
{
    using FlowControl;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Spline))]
    public class SplineEditor : Editor
    {
        private Spline m_spline;
        private SerializedProperty closed;

        void OnEnable()
        {
            m_spline = (Spline)target;
            closed = serializedObject.FindProperty("m_isClosed");
        }

        void OnSceneGUI()
        {
            SplinePoint[] points = m_spline.Points;

            for (int i = 0; i < points.Length; i++)
            {
                SplinePoint point = points[i];
                Handles.Label(point.Position + new Vector3(0, HandleUtility.GetHandleSize(point.Position) * 0.3f, 0), point.gameObject.name);
            }
        }

        public override void OnInspectorGUI()
        {
            bool bNewVal = EditorGUILayout.Toggle("Closed:", closed.boolValue);
            m_spline.IsClosed = bNewVal;

            if (GUILayout.Button("Add Point"))
            {
                m_spline.AddPoint();
            }

            SceneView.RepaintAll();
        }
    }
}
