/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControlEditor
{
    using FlowControl;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(SplinePoint))]
    public class SplinePointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SplinePoint splinePoint = (SplinePoint)target;

            splinePoint.HandleDepart = EditorGUILayout.Vector3Field("Depart from", splinePoint.HandleDepart);
            splinePoint.HandleApproach = EditorGUILayout.Vector3Field("Approach from", splinePoint.HandleApproach);

            SceneView.RepaintAll();
        }

        void OnSceneGUI()
        {
            SplinePoint splinePoint = (SplinePoint)target;

            Handles.color = Color.yellow;

            Transform t = splinePoint.gameObject.transform;

            splinePoint.HandleDepart = t.InverseTransformPoint(
                    Handles.FreeMoveHandle(
                            t.TransformPoint(
                                splinePoint.HandleDepart),
                            Quaternion.identity,
                            HandleUtility.GetHandleSize(splinePoint.HandleDepart) * 0.15f,
                            Vector3.zero,
                            Handles.CircleHandleCap));

            splinePoint.HandleApproach = t.InverseTransformPoint(
                    Handles.FreeMoveHandle(
                            t.TransformPoint(
                                splinePoint.HandleApproach),
                            Quaternion.identity,
                            HandleUtility.GetHandleSize(splinePoint.HandleApproach) * 0.15f,
                            Vector3.zero,
                            Handles.CircleHandleCap));

            Handles.DrawLine(t.TransformPoint(splinePoint.HandleDepart), t.TransformPoint(splinePoint.HandleApproach));
        }
    }
}
