/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControlEditor
{
    using UnityEngine;
    using UnityEditor;
    using FlowControl;

    [CustomEditor(typeof(FlowControlRegion))]
    public class FlowControlRegionEditor : Editor
    {
        private FlowControlRegion m_field;
        private SerializedProperty resolution;
        private SerializedProperty showForces;
        private SerializedProperty showSplines;
        private SerializedProperty showTestPoints;
        private SerializedProperty forceDisplayCutoff;
        private SerializedProperty approach;
        private SerializedProperty outerEntryVector;
        private SerializedProperty outerExitVector;
        private SerializedProperty entryBehaviour;
        private SerializedProperty exitBehaviour;
        private SerializedProperty outerEntryTransform;
        private SerializedProperty outerExitTransform;

        void OnEnable()
        {
            m_field = (FlowControlRegion)target;

            showForces = serializedObject.FindProperty("ShowForces");
            showSplines = serializedObject.FindProperty("ShowSplines");
            showTestPoints = serializedObject.FindProperty("ShowTestPoints");
            forceDisplayCutoff = serializedObject.FindProperty("forceDisplayCutoff");
            outerEntryVector = serializedObject.FindProperty("outerEntryVector");
            outerExitVector = serializedObject.FindProperty("outerExitVector");
            resolution = serializedObject.FindProperty("m_resolution");
            approach = serializedObject.FindProperty("m_approach");
            entryBehaviour = serializedObject.FindProperty("entryBehaviour");
            exitBehaviour = serializedObject.FindProperty("exitBehaviour");
            outerEntryTransform = serializedObject.FindProperty("outerEntryTransform");
            outerExitTransform = serializedObject.FindProperty("outerExitTransform");
        }

        [MenuItem("GameObject/Create Other/Flow control region")]
        public static void CreateRegion(MenuCommand command)
        {
            GameObject region = new GameObject("Flow control region");
            region.AddComponent<FlowControlRegion>();
        }

        public override void OnInspectorGUI()
        {
            resolution.intValue = EditorGUILayout.IntField("Resolution:", resolution.intValue);
            approach.floatValue = EditorGUILayout.FloatField("Approach:", approach.floatValue);

            EditorGUILayout.Separator();


            FlowControlRegion.OuterBehaviour selectedEntry = m_field.entryBehaviour;

            selectedEntry = (FlowControlRegion.OuterBehaviour)EditorGUILayout.EnumPopup("Entry behaviour", selectedEntry);
            entryBehaviour.enumValueIndex = (int)selectedEntry;

            if (selectedEntry == FlowControlRegion.OuterBehaviour.ApplyVector)
            {
                outerEntryVector.vector3Value = EditorGUILayout.Vector3Field("Applied vector:", m_field.outerEntryVector);
            }

            if (selectedEntry == FlowControlRegion.OuterBehaviour.MoveToTransform)
            {
                outerEntryTransform.objectReferenceValue = EditorGUILayout.ObjectField("Target:", m_field.outerEntryTransform, typeof(Transform), true) as Transform;
            }


            FlowControlRegion.OuterBehaviour selectedExit = m_field.exitBehaviour;

            selectedExit = (FlowControlRegion.OuterBehaviour)EditorGUILayout.EnumPopup("Exit behaviour", selectedExit);
            exitBehaviour.enumValueIndex = (int)selectedExit;

            if (selectedExit == FlowControlRegion.OuterBehaviour.ApplyVector)
            {
                outerExitVector.vector3Value = EditorGUILayout.Vector3Field("Applied vector:", m_field.outerExitVector);
            }

            if (selectedExit == FlowControlRegion.OuterBehaviour.MoveToTransform)
            {
                outerExitTransform.objectReferenceValue = EditorGUILayout.ObjectField("Target:", m_field.outerExitTransform, typeof(Transform), true) as Transform;
            }


            EditorGUILayout.Separator();

            if (GUILayout.Button("Add Spline"))
            {
                m_field.AddSpline();
            }

            EditorGUILayout.Separator();

            showForces.boolValue = EditorGUILayout.Toggle("Show forces", showForces.boolValue);
            if (showForces.boolValue)
            {
                forceDisplayCutoff.floatValue = EditorGUILayout.Slider("Force display cutoff", forceDisplayCutoff.floatValue, 0f, 0.9f);
            }
            showSplines.boolValue = EditorGUILayout.Toggle("Show splines", showSplines.boolValue);
            showTestPoints.boolValue = EditorGUILayout.Toggle("Show test points", showTestPoints.boolValue);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

        }

    }
}
