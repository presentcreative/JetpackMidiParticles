using System.Collections;
using System.Collections.Generic;
using JangaFX;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VectorFieldParticleEmitter))]
public class VectorFieldParticleEmitterEditor : Editor 
{
	SerializedProperty VectorFieldSource;
	SerializedProperty EmissionType;
	SerializedProperty Coverage;

	private GUISkin headerSkin = null;
	private Texture jangaFxLogo = null;
	private Texture headerLogo = null;
	private GUIStyle styleLeft;
	private GUIStyle styleRight;
	
	void OnEnable()
	{
		VectorFieldSource = serializedObject.FindProperty("VectorFieldSource");
		EmissionType = serializedObject.FindProperty("EmissionType");
		Coverage = serializedObject.FindProperty("Coverage");
		
		if (jangaFxLogo == null)
			jangaFxLogo = (Texture)Resources.Load("Textures/jangafx-small",typeof(Texture));
		if (headerLogo == null)
			headerLogo = (Texture)Resources.Load("Textures/particleEmitter-small",typeof(Texture));
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
		EditorGUILayout.PropertyField(VectorFieldSource);
		EditorGUILayout.PropertyField(EmissionType);
		EditorGUILayout.PropertyField(Coverage);
		serializedObject.ApplyModifiedProperties();
	}
}
