using System.Collections;
using System.Collections.Generic;
using JangaFX;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(VectorFieldRigidBodyController))]
public class VectorFieldRigidBodyControllerEditor : Editor 
{
	SerializedProperty Tightness;
	SerializedProperty Multiplier;
	SerializedProperty AffectedByAllVF;
	SerializedProperty VFRestrictedList;

	private GUISkin headerSkin = null;
	private Texture jangaFxLogo = null;
	private Texture headerLogo = null;
	private GUIStyle styleLeft;
	private GUIStyle styleRight;
	
	void OnEnable()
	{
		Tightness = serializedObject.FindProperty("Tightness");
		Multiplier = serializedObject.FindProperty("Multiplier");
		AffectedByAllVF = serializedObject.FindProperty("AffectedByAllVF");
		VFRestrictedList = serializedObject.FindProperty("VFRestrictedList");
		
		if (jangaFxLogo == null)
			jangaFxLogo = (Texture)Resources.Load("Textures/jangafx-small",typeof(Texture));
		if (headerLogo == null)
			headerLogo = (Texture)Resources.Load("Textures/rbController-small",typeof(Texture));
		if (headerSkin == null)
			headerSkin = (GUISkin)Resources.Load("Styles/HeaderSkin",typeof(GUISkin));	
		styleLeft = new GUIStyle();
		styleLeft.alignment = TextAnchor.MiddleLeft;
		styleRight = new GUIStyle();
		styleRight.alignment = TextAnchor.MiddleRight;
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		GUISkin originalSkin = GUI.skin;
		GUI.skin = headerSkin;

		GUILayout.BeginHorizontal("box");
		GUILayout.Box(headerLogo, styleLeft);
		GUILayout.Box(jangaFxLogo, styleRight);
		GUILayout.EndHorizontal();
		GUI.skin = originalSkin;

		GUILayout.Space(5);
		EditorGUILayout.PropertyField(Tightness);
		EditorGUILayout.PropertyField(Multiplier);
		EditorGUILayout.PropertyField(AffectedByAllVF);
		if (!AffectedByAllVF.boolValue)
			EditorGUILayout.PropertyField(VFRestrictedList, true);
		serializedObject.ApplyModifiedProperties();
	}

	
}
