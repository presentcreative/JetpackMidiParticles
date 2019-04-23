using System.Collections;
using System.Collections.Generic;
using System.IO;
using JangaFX;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

[CustomEditor(typeof(VectorField))]
public class VectorFieldEditor: Editor
{
	// Vector field
	SerializedProperty Intensity;
	SerializedProperty Bounds;
	SerializedProperty PerAxisBounds;
	SerializedProperty BoundsX;
	SerializedProperty BoundsY;
	SerializedProperty BoundsZ;

	SerializedProperty ClosedBoundRamp;
	
	// Animation
	SerializedProperty Animate;
	SerializedProperty AnimationType;
	SerializedProperty Curve;
	SerializedProperty Duration;

	// GPU
	SerializedProperty GenerateGPUTexture;

	// Size
	SerializedProperty SizeFromFile;
	SerializedProperty SizeX;
	SerializedProperty SizeY;
	SerializedProperty SizeZ;
	
	// Gizmo
	SerializedProperty ShowGizmos;
	SerializedProperty ShowVectorFieldAs;
	SerializedProperty GizmoScale;
	SerializedProperty VectorFieldColor;
	SerializedProperty AlwaysDisplayVF;

	private GUISkin headerSkin = null;
    private int currentTab = 0;
	private Texture jangaFxLogo = null;
	private Texture headerLogo = null;
	private GUIStyle styleLeft;
	private GUIStyle styleRight;
	private string[] leafnames = null;
	private int selectedSource = 0;
	
	void OnEnable()
	{
		Intensity = serializedObject.FindProperty("Intensity");
		Bounds = serializedObject.FindProperty("Bounds");
		PerAxisBounds = serializedObject.FindProperty("PerAxisBounds");
		BoundsX = serializedObject.FindProperty("BoundsX");
		BoundsY = serializedObject.FindProperty("BoundsY");
		BoundsZ = serializedObject.FindProperty("BoundsZ");
		ClosedBoundRamp = serializedObject.FindProperty("ClosedBoundRamp");

		Animate = serializedObject.FindProperty("Animate");
		AnimationType = serializedObject.FindProperty("AnimationType");
		Curve = serializedObject.FindProperty("Curve");
		Duration = serializedObject.FindProperty("Duration");

		SizeFromFile = serializedObject.FindProperty("SizeFromFile");
		SizeX = serializedObject.FindProperty("SizeX");
		SizeY = serializedObject.FindProperty("SizeY");
		SizeZ = serializedObject.FindProperty("SizeZ");
		
		GenerateGPUTexture = serializedObject.FindProperty("GenerateGPUTexture");

		// Gizmo
		ShowGizmos = serializedObject.FindProperty("ShowGizmos");
		ShowVectorFieldAs = serializedObject.FindProperty("ShowVectorFieldAs");
		GizmoScale = serializedObject.FindProperty("GizmoScale");
		VectorFieldColor = serializedObject.FindProperty("VectorFieldColor");
		AlwaysDisplayVF = serializedObject.FindProperty("AlwaysDisplayVF");
		
		if (jangaFxLogo == null)
			jangaFxLogo = (Texture)Resources.Load("Textures/jangafx-small",typeof(Texture));
		if (headerLogo == null)
			headerLogo = (Texture)Resources.Load("Textures/vectorfield-small",typeof(Texture));
		if (headerSkin == null)
			headerSkin = (GUISkin)Resources.Load("Styles/HeaderSkin",typeof(GUISkin));	
		styleLeft = new GUIStyle();
		styleLeft.alignment = TextAnchor.MiddleLeft;
		styleRight = new GUIStyle();
		styleRight.alignment = TextAnchor.MiddleRight;
		
		RefreshFileList();
		VectorField editedObj = target as VectorField;
		
		for (int i = 0; i<leafnames.Length; i++)
		{
			if (leafnames[i] == editedObj.VFFilename)
				selectedSource = i;
		}
	}

	private void RefreshFileList()
	{
		string basicPath = Application.dataPath;
		string[] files = Directory.GetFiles(basicPath, "*.fga", SearchOption.AllDirectories);
		leafnames = new string[files.Length+1];
		leafnames[0] = "None";
		for (int i = 0; i<files.Length; i++) { leafnames[i+1] = Path.GetFileNameWithoutExtension(files[i]); }
	}

	private string GetFullName(string leafname)
	{
		string basicPath = Application.dataPath;
		string[] files = Directory.GetFiles(basicPath, "*.fga", SearchOption.AllDirectories);

		for (int i = 0; i<files.Length; i++)
		{
			if (leafname == Path.GetFileNameWithoutExtension(files[i]))
				return files[i];
		}

		return "";
	}

	private void ReadVF(int index)
	{
		VectorField editedObj = target as VectorField;

		editedObj.VFFilename = leafnames[index];
		
		string fullname = GetFullName(editedObj.VFFilename);
		if (string.IsNullOrEmpty(fullname))
			editedObj.ClearVF();
		else
			editedObj.ReadFile(fullname);
	}
	
	private void SaveVFAsTexture()
	{
		VectorField editedObj = target as VectorField;
		
		string initialPath;

		if ( GetFullName(editedObj.VFFilename).StartsWith(Application.dataPath))
			initialPath = GetFullName(editedObj.VFFilename);
		else
			initialPath = Application.dataPath;

		Debug.Log("Initial path = "+initialPath);
		Debug.Log("Initial path = "+initialPath);

		Debug.Log("Leaf path = "+Path.GetFileNameWithoutExtension(editedObj.VFFilename)+".asset");


		var path = EditorUtility.SaveFilePanel(
			"Save texture as resource",
			initialPath,
			Path.GetFileNameWithoutExtension(editedObj.VFFilename)+".asset",
			"asset");
		// var path = EditorUtility.SaveFilePanel(
		// 	"Save texture as resource",
		// 	initialPath,
		// 	editedObj.VFFilename+".asset",
		// 	"asset");
        if (path.Length != 0)
        {
			editedObj.SaveVFAsTexture(path);
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		VectorField editedObj = target as VectorField;

		GUISkin originalSkin = GUI.skin;
		
		GUI.skin = headerSkin;

		GUILayout.BeginHorizontal("box");
		GUILayout.Box(headerLogo, styleLeft);
		GUILayout.Box(jangaFxLogo, styleRight);
		GUILayout.EndHorizontal();
		
		GUI.skin = originalSkin;

		int previousSelection = selectedSource;

		GUILayout.BeginHorizontal();
		GUILayout.Label("VF File");
		selectedSource = EditorGUILayout.Popup(selectedSource, leafnames);
		if (GUILayout.Button("Refresh"))
			RefreshFileList();
		GUILayout.EndHorizontal();
		GUILayout.Label("Vector field size : "+ editedObj.BoxSize.x.ToString("F2")+", "+editedObj.BoxSize.y.ToString("F2") + ", "+editedObj.BoxSize.z.ToString("F2"));


		if (selectedSource != previousSelection)
			ReadVF(selectedSource);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reload VF"))
			ReadVF(selectedSource);
		if (GUILayout.Button("Rename from file"))
		{
			editedObj.name = leafnames[selectedSource];
		}
		if (GUILayout.Button("Save 3D texture"))
			SaveVFAsTexture();
		GUILayout.EndHorizontal();

		GUILayout.BeginVertical("Box");
		currentTab = GUILayout.Toolbar(currentTab, new string[] {"Bounds", "Intensity", "Size", "Gizmo"});
		//currentTab = GUILayout.SelectionGrid(currentTab, new string[] { "Bounds", "Intensity", "Size", "Gizmo", "GPU" }, 3);
		GUILayout.EndVertical();

		GUILayout.BeginVertical(GUILayout.Height(100));
		GUILayout.Space(5);
		switch (currentTab)
		{
			case 0:
				EditorGUILayout.PropertyField(PerAxisBounds);
				if (PerAxisBounds.boolValue)
				{
					EditorGUILayout.PropertyField(BoundsX);
					EditorGUILayout.PropertyField(BoundsY);
					EditorGUILayout.PropertyField(BoundsZ);
				}
				else
					EditorGUILayout.PropertyField(Bounds);
				EditorGUILayout.PropertyField(ClosedBoundRamp);
				break;
			case 1:
				EditorGUILayout.PropertyField(Intensity);
				EditorGUILayout.PropertyField(Animate);
				if (Animate.boolValue)
				{
					EditorGUILayout.PropertyField(AnimationType);
					EditorGUILayout.PropertyField(Curve);
					EditorGUILayout.PropertyField(Duration);
				}
				break;
			case 2:
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(SizeFromFile);
				if (!SizeFromFile.boolValue)
				{
					EditorGUILayout.PropertyField(SizeX);
					EditorGUILayout.PropertyField(SizeY);
					EditorGUILayout.PropertyField(SizeZ);
				}
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					if (SizeFromFile.boolValue)
						ReadVF(selectedSource);
					else
						editedObj.ResizeBox();
				}
				break;
			case 3:
				EditorGUILayout.PropertyField(ShowGizmos);
				EditorGUILayout.PropertyField(ShowVectorFieldAs);
				EditorGUILayout.PropertyField(GizmoScale);
				EditorGUILayout.PropertyField(VectorFieldColor);
				EditorGUILayout.PropertyField(AlwaysDisplayVF);
				break;
			case 4:	// GPU ... not used yet
				EditorGUILayout.PropertyField(GenerateGPUTexture);
				break;
		}
		GUILayout.EndVertical();
		serializedObject.ApplyModifiedProperties();
	}

}