using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace JangaFX
{
	[AddComponentMenu("Vector Field/Vector Field")]
	[ExecuteInEditMode]
	public class VectorField: MonoBehaviour
	{
		public enum AutoAnimationType
		{
			Forward,
			Backward,
			PingPong
		}
		public enum GizmosShape
		{
			ColoredArrows,
			ScaledArrows,
			ColoredLines,
			ScaledLines,
			None
		}
		public enum BoundsType
		{
			Border,
			Repeat,
			Closed,
			Mirror
		}
		
		public bool ShowGizmos = true;
		public GizmosShape ShowVectorFieldAs = GizmosShape.ScaledArrows;
		[Range(0.1f, 10.0f)]
		public float GizmoScale = 1.0f;
		public Color VectorFieldColor = new Color(1f, 1f, 0f, 0.5f);
		public bool AlwaysDisplayVF = false;

		public bool SizeFromFile = true;
		public float SizeX;
		public float SizeY;
		public float SizeZ;
		
		public float Intensity = 1.0f;
		public bool Animate = false;
		public AutoAnimationType AnimationType = AutoAnimationType.Forward;
		public AnimationCurve Curve = new AnimationCurve();
		[Range(0.0f, 10.0f)]
		public float Duration = 1.0f;
		public BoundsType Bounds = BoundsType.Closed;
		public bool PerAxisBounds = false;
		public BoundsType BoundsX = BoundsType.Closed;
		public BoundsType BoundsY = BoundsType.Closed;
		public BoundsType BoundsZ = BoundsType.Closed;
		[Range(0.0f, 1.0f)]
		public float ClosedBoundRamp = 0.0f;
		public string VFFilename;
		
		public bool GenerateGPUTexture = false;

		[SerializeField, HideInInspector]
		private Texture3D GPUTexture = null;
		public Texture3D GetGPUTexture
		{
			get { return GPUTexture; }
		}
		
		[SerializeField, HideInInspector]
		private int vectorSizeX;
		[SerializeField, HideInInspector]
		private int vectorSizeY;
		[SerializeField, HideInInspector]
		private int vectorSizeZ;

		[SerializeField, HideInInspector]
		private Vector3 minCorner=Vector3.zero;
		[SerializeField, HideInInspector]
		private Vector3 maxCorner=Vector3.zero;
		[SerializeField, HideInInspector]
		private Vector3 boxSize=Vector3.zero;

		[SerializeField, HideInInspector]
		private Vector3[] vectorData;

		[SerializeField, HideInInspector]
		private Vector3[] worldBounds;
		[SerializeField, HideInInspector]
		private Vector3[] localBounds;


		public Vector3 BoxSize
		{
			get { return boxSize; }
		}

		private static List<VectorField> vectorFieldsList = new List<VectorField>();
		
		private float AnimIntensity = 1.0f;
		private Mesh arrowMesh=null;
		private const float globalScale = 0.01f; // from m to cm

		public void ClearVF()
		{
			vectorData = null;
			minCorner = maxCorner = boxSize = Vector3.zero;
			vectorSizeX = vectorSizeY = vectorSizeZ = 0;
			worldBounds = null;
			localBounds = null;
		}

		public void ResizeBox()
		{
			boxSize.x = SizeX;
			boxSize.y = SizeY;
			boxSize.z = SizeZ;

			minCorner = -boxSize*0.5f;
			maxCorner = boxSize*0.5f;

			ComputeBounds();
		}
		
		private void ComputeBounds()
		{
			worldBounds = new Vector3[8];
			localBounds = new Vector3[8];

			localBounds[0] = new Vector3(minCorner.x, minCorner.y, minCorner.z);
			localBounds[1] = new Vector3(maxCorner.x, minCorner.y, minCorner.z);
			localBounds[2] = new Vector3(minCorner.x, maxCorner.y, minCorner.z);
			localBounds[3] = new Vector3(maxCorner.x, maxCorner.y, minCorner.z);
			localBounds[4] = new Vector3(minCorner.x, minCorner.y, maxCorner.z);
			localBounds[5] = new Vector3(maxCorner.x, minCorner.y, maxCorner.z);
			localBounds[6] = new Vector3(minCorner.x, maxCorner.y, maxCorner.z);
			localBounds[7] = new Vector3(maxCorner.x, maxCorner.y, maxCorner.z);

			ComputeWorldBounds();
		}	
		
		public void ReadFile(string filename)
        {
	        if (!File.Exists(filename))
            {
                Debug.LogError("Missing vectorfiles "+filename);
                return;
            }

	        string fileLine;
	        string[] fileWords;
	        StreamReader reader = new StreamReader(filename);

	        try
	        {
		        // Header - Get size
		        fileLine = reader.ReadLine();
				fileWords = fileLine.Split(',');
		        vectorSizeX = (int)(float.Parse(fileWords[0].Trim(), System.Globalization.CultureInfo.InvariantCulture));
		        vectorSizeY = (int)(float.Parse(fileWords[1].Trim(), System.Globalization.CultureInfo.InvariantCulture));
		        vectorSizeZ = (int)(float.Parse(fileWords[2].Trim(), System.Globalization.CultureInfo.InvariantCulture));
		        
		        // Header - Get min corner
		        fileLine = reader.ReadLine();
		        fileWords = fileLine.Split(',');
		        minCorner.x = float.Parse(fileWords[0].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;
		        minCorner.y = float.Parse(fileWords[1].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;
		        minCorner.z = float.Parse(fileWords[2].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;
		        
		        // Header - Get max corner
		        fileLine = reader.ReadLine();
		        fileWords = fileLine.Split(',');
		        maxCorner.x = float.Parse(fileWords[0].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;
		        maxCorner.y = float.Parse(fileWords[1].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;
		        maxCorner.z = float.Parse(fileWords[2].Trim(), System.Globalization.CultureInfo.InvariantCulture)*globalScale;

		        boxSize = maxCorner - minCorner;

		        if (SizeFromFile)
		        {
			        SizeX = boxSize.x;
			        SizeY = boxSize.y;
			        SizeZ = boxSize.z;
		        }
		        
		        vectorData = new Vector3[vectorSizeX * vectorSizeY * vectorSizeZ];

		        for (int y = 0; y<vectorSizeY; y++)
		        {
			        for (int x = 0; x<vectorSizeX; x++)
			        {
				        for (int z = 0; z<vectorSizeZ; z++)
				        {
					        Vector3 currentDirection;
					        fileLine = reader.ReadLine();
					        fileWords = fileLine.Split(',');
					        currentDirection.z = float.Parse(fileWords[0].Trim(), System.Globalization.CultureInfo.InvariantCulture);// X in unreal & jangaFX
					        currentDirection.x = float.Parse(fileWords[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);// Y in unreal & jangaFX
					        currentDirection.y = float.Parse(fileWords[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);// Z in unreal & jangaFX

					        vectorData[x + y*vectorSizeX + z*vectorSizeX * vectorSizeY] = currentDirection;
				        }
			        }
		        }
	        }
	        catch (Exception e)
	        {
				Debug.LogError("Error while loading "+filename);		        
		        Debug.LogError(e.Message);
	        }
	        
	        reader.Close();

	        if (SizeFromFile)
		        ComputeBounds();
	        else
		        ResizeBox();
        }

		public void SaveVFAsTexture(string path)
		{
			GenerateTexture(true);
			if (path.StartsWith(Application.dataPath)) 
			{
         		path= "Assets" + path.Substring(Application.dataPath.Length);
			}
			else
			{
				EditorUtility.DisplayDialog("Error while saving 3D texture",
											"You have to save the asset in the Asset directory of your project.", "OK");
				return;
			}

			Debug.Log("Save as "+path);
			var newGPUTexture = Instantiate(GPUTexture);
			AssetDatabase.CreateAsset(newGPUTexture, path);
			// DestroyImmediate(newGPUTexture);
		}

		public Vector3 GetPointInField(float invcoverage)
		{
			if (worldBounds==null)
				return Vector3.zero;

			float x = Random.Range(minCorner.x + boxSize.x*0.5f*invcoverage, maxCorner.x - boxSize.x*0.5f*invcoverage);
			float y = Random.Range(minCorner.y + boxSize.y*0.5f*invcoverage, maxCorner.y - boxSize.y*0.5f*invcoverage);
			float z = Random.Range(minCorner.z + boxSize.z*0.5f*invcoverage, maxCorner.z - boxSize.z*0.5f*invcoverage);

			return transform.TransformPoint(new Vector3(x, y, z));
		}

		public void GetRandomVector(out Vector3 position, out Vector3 direction)
		{
			if (vectorData == null)
				position = direction = Vector3.zero;
			
			int x = Random.Range(0, vectorSizeX);
			int y = Random.Range(0, vectorSizeY);
			int z = Random.Range(0, vectorSizeZ);

			position = new Vector3();
			direction = new Vector3();
			
			float rY = (float)(y+0.5f)/(float)vectorSizeY;
			position.y = Mathf.Lerp(minCorner.y, maxCorner.y, rY);
			float rX = (float)(x+0.5f)/(float)vectorSizeX;
			position.x = Mathf.Lerp(minCorner.x, maxCorner.x, rX);
			float rZ = (float)(z+0.5f)/(float)vectorSizeZ;
			position.z = Mathf.Lerp(minCorner.z, maxCorner.z, rZ);
			position = transform.TransformPoint(position);

			direction = vectorData[x+y*vectorSizeX+z*vectorSizeX*vectorSizeY]*Intensity*AnimIntensity;
			direction = transform.TransformDirection(direction);
		}

		
		public Vector3 GetRandomCorner()
		{
			if (worldBounds==null)
				return Vector3.zero;

			return worldBounds[Random.Range(0, worldBounds.Length)];
		}

		public Vector3 GetPointOnEdge(float invcoverage)
		{
			if (worldBounds==null)
				return Vector3.zero;

			int edgeID = Random.Range(0, 12);
			float relativePosition = Random.Range(0.5f*invcoverage, 1.0f-0.5f*invcoverage);

			switch (edgeID)
			{
				case 0:
					return Vector3.Lerp(worldBounds[0], worldBounds[1], relativePosition);
				case 1:
					return Vector3.Lerp(worldBounds[1], worldBounds[3], relativePosition);
				case 2:
					return Vector3.Lerp(worldBounds[3], worldBounds[2], relativePosition);
				case 3:
					return Vector3.Lerp(worldBounds[2], worldBounds[0], relativePosition);
				case 4:
					return Vector3.Lerp(worldBounds[4], worldBounds[5], relativePosition);
				case 5:
					return Vector3.Lerp(worldBounds[5], worldBounds[7], relativePosition);
				case 6:
					return Vector3.Lerp(worldBounds[7], worldBounds[6], relativePosition);
				case 7:
					return Vector3.Lerp(worldBounds[6], worldBounds[4], relativePosition);
				case 8:
					return Vector3.Lerp(worldBounds[0], worldBounds[4], relativePosition);
				case 9:
					return Vector3.Lerp(worldBounds[1], worldBounds[5], relativePosition);
				case 10:
					return Vector3.Lerp(worldBounds[2], worldBounds[6], relativePosition);
				case 11:
					return Vector3.Lerp(worldBounds[3], worldBounds[7], relativePosition);
			}
			return Vector3.zero;
		}		

		public Vector3 GetPointOnFace(int faceID, float invcoverage)
		{
			if (worldBounds==null)
				return Vector3.zero;

			float relativePositionX = Random.Range(0.5f*invcoverage, 1.0f-0.5f*invcoverage);
			float relativePositionY = Random.Range(0.5f*invcoverage, 1.0f-0.5f*invcoverage);
			
			switch (faceID)
			{
				case 0:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[0], worldBounds[1], relativePositionX),
						Vector3.Lerp(worldBounds[2], worldBounds[3], relativePositionX), relativePositionY); 
				case 1:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[4], worldBounds[5], relativePositionX),
						Vector3.Lerp(worldBounds[6], worldBounds[7], relativePositionX), relativePositionY); 
				case 2:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[0], worldBounds[1], relativePositionX),
						Vector3.Lerp(worldBounds[4], worldBounds[5], relativePositionX), relativePositionY); 
				case 3:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[2], worldBounds[0], relativePositionX),
						Vector3.Lerp(worldBounds[6], worldBounds[4], relativePositionX), relativePositionY); 
				case 4:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[3], worldBounds[2], relativePositionX),
						Vector3.Lerp(worldBounds[7], worldBounds[6], relativePositionX), relativePositionY); 
				case 5:
					return Vector3.Lerp(
						Vector3.Lerp(worldBounds[1], worldBounds[3], relativePositionX),
						Vector3.Lerp(worldBounds[5], worldBounds[7], relativePositionX), relativePositionY); 
			}
			
			return Vector3.zero;
		}		

		public Vector3 GetPointOnVolume(float invcoverage)
		{
			int faceID = Random.Range(0, 6);
			return GetPointOnFace(faceID, invcoverage);
		}		

		public static Vector3 GetCombinedVectors(Vector3 worldPosition)
		{
			if (vectorFieldsList.Count == 0)
				return Vector3.zero;

			Vector3 combined=Vector3.zero;

			for (int i = 0; i<vectorFieldsList.Count; i++)
				combined += vectorFieldsList[i].GetVector(worldPosition);

			return combined;
		}

		public static Vector3 GetCombinedVectorsRestricted(Vector3 worldPosition, List<VectorField> vfList)
		{
			if (vfList.Count == 0)
				return Vector3.zero;

			Vector3 combined=Vector3.zero;

			for (int i = 0; i<vfList.Count; i++)
			{
				if (vfList[i]!=null)
					combined += vfList[i].GetVector(worldPosition);
			}

			return combined;
		}

		
		public Vector3 GetVector(Vector3 worldPosition)
		{
			if (vectorData==null)
				return Vector3.zero;
			
			Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
			Vector3 value = Vector3.zero;
			Vector3 relativeLocalPosition = (localPosition-minCorner);
			relativeLocalPosition.x /= boxSize.x;
			relativeLocalPosition.y /= boxSize.y;
			relativeLocalPosition.z /= boxSize.z;

			float borderIntensity = 1.0f;
			Vector3 clampedBorderIntensity = Vector3.one;
			
			if (ClosedBoundRamp > 0.0f)
			{
				clampedBorderIntensity.x = Mathf.Min(clampedBorderIntensity.x, 0.5f - Mathf.Abs(0.5f - relativeLocalPosition.x));
				clampedBorderIntensity.y = Mathf.Min(clampedBorderIntensity.y, 0.5f - Mathf.Abs(0.5f - relativeLocalPosition.y));
				clampedBorderIntensity.z = Mathf.Min(clampedBorderIntensity.z, 0.5f - Mathf.Abs(0.5f - relativeLocalPosition.z));

				clampedBorderIntensity = 2.0f * clampedBorderIntensity;

				clampedBorderIntensity.x = Mathf.Clamp01(clampedBorderIntensity.x / ClosedBoundRamp);
				clampedBorderIntensity.y = Mathf.Clamp01(clampedBorderIntensity.y / ClosedBoundRamp);
				clampedBorderIntensity.z = Mathf.Clamp01(clampedBorderIntensity.z / ClosedBoundRamp);
			}

			int cX, cY, cZ;

			if (!PerAxisBounds) 
				BoundsX = BoundsY = BoundsZ = Bounds;
			
			switch (BoundsX)
			{
				case BoundsType.Closed:
					if ((relativeLocalPosition.x < -0.001f) || (relativeLocalPosition.x > 1.001f))
						return value;
					borderIntensity *= clampedBorderIntensity.x;
					break;
				case BoundsType.Repeat:
					relativeLocalPosition.x -= Mathf.Floor(relativeLocalPosition.x);
					break;
				case BoundsType.Border:
					relativeLocalPosition.x = Mathf.Clamp(relativeLocalPosition.x, 0.0f, 0.9999f);
					break;
				case BoundsType.Mirror:
					relativeLocalPosition.x = (relativeLocalPosition.x*.5f - Mathf.Floor(relativeLocalPosition.x*.5f))*2.0f;
					if (relativeLocalPosition.x > 1.0f)
						relativeLocalPosition.x = 2f - relativeLocalPosition.x;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			switch (BoundsY)
			{
				case BoundsType.Closed:
					if ((relativeLocalPosition.y < -0.001f) || (relativeLocalPosition.y > 1.001f))
						return value;
					borderIntensity *= clampedBorderIntensity.y;
					break;
				case BoundsType.Repeat:
					relativeLocalPosition.y -= Mathf.Floor(relativeLocalPosition.y);
					break;
				case BoundsType.Border:
					relativeLocalPosition.y = Mathf.Clamp(relativeLocalPosition.y, 0.0f, 0.9999f);
					break;
				case BoundsType.Mirror:
					relativeLocalPosition.y = (relativeLocalPosition.y*0.5f - Mathf.Floor(relativeLocalPosition.y*0.5f))*2.0f;
					if (relativeLocalPosition.y > 1.0f)
						relativeLocalPosition.y = 2f - relativeLocalPosition.y;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			switch (BoundsZ)
			{
				case BoundsType.Closed:
					if ((relativeLocalPosition.z < -0.001f) || (relativeLocalPosition.z > 1.001f))
						return value;
					borderIntensity *= clampedBorderIntensity.z;
					break;
				case BoundsType.Repeat:
					relativeLocalPosition.z -= Mathf.Floor(relativeLocalPosition.z);
					break;
				case BoundsType.Border:
					relativeLocalPosition.z = Mathf.Clamp(relativeLocalPosition.z, 0.0f, 0.9999f);
					break;
				case BoundsType.Mirror:
					relativeLocalPosition.z = (relativeLocalPosition.z*0.5f - Mathf.Floor(relativeLocalPosition.z*0.5f))*2.0f;
					if (relativeLocalPosition.z > 1.0f)
						relativeLocalPosition.z = 2f - relativeLocalPosition.z;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			cX = Mathf.FloorToInt(relativeLocalPosition.x * vectorSizeX);
			cY = Mathf.FloorToInt(relativeLocalPosition.y * vectorSizeY);
			cZ = Mathf.FloorToInt(relativeLocalPosition.z * vectorSizeZ);
			cX = Mathf.Clamp(cX, 0, vectorSizeX - 1);
			cY = Mathf.Clamp(cY, 0, vectorSizeY - 1);
			cZ = Mathf.Clamp(cZ, 0, vectorSizeZ - 1);
			value = vectorData[cX + cY * vectorSizeX + cZ * vectorSizeX * vectorSizeY] * Intensity * AnimIntensity * borderIntensity * borderIntensity * borderIntensity;
			value = transform.TransformDirection(value);

			return value;
		}

		private void OnEnable()
		{
#if UNITY_EDITOR
            EditorApplication.update += Update;
#endif
            if (!vectorFieldsList.Contains(this))
			{
				vectorFieldsList.Add(this);
			}
		}

		private void OnDisable()
		{
#if UNITY_EDITOR
            EditorApplication.update -= Update;
#endif
            if (vectorFieldsList.Contains(this))
				vectorFieldsList.Remove(this);
		}

		private void GenerateTexture(bool force)
		{
			if ((force) && (GPUTexture!=null))
			{
				DestroyImmediate(GPUTexture, true);
				GPUTexture = null;
			}

			if (GPUTexture == null)
			{
				Color[] colorArray = new Color[vectorSizeX * vectorSizeY * vectorSizeZ];

				GPUTexture = new Texture3D(vectorSizeX, vectorSizeY, vectorSizeZ, TextureFormat.RGBAHalf, true);
				for (int x = 0; x < vectorSizeX; x++)
				{
					for (int y = 0; y < vectorSizeY; y++)
					{
						for (int z = 0; z < vectorSizeZ; z++)
						{
							Vector3 localDirection = vectorData[x + y * vectorSizeX + z * vectorSizeX * vectorSizeY];

							Color c = new Color(localDirection.x, localDirection.y, localDirection.z, 1.0f);
							colorArray[x + (y * vectorSizeX) + (z * vectorSizeX * vectorSizeY)] = c;
						}
					}
				}
				GPUTexture.SetPixels(colorArray);
				GPUTexture.Apply();
			}
//				Shader.SetGlobalTexture("VFTexture"+ vfID, GPUTexture);
//				Shader.SetGlobalMatrix("VFMatrix" + vfID, transform.localToWorldMatrix);
//				Shader.SetGlobalMatrix("VFInvMatrix" + vfID, transform.worldToLocalMatrix);
//				Shader.SetGlobalInt("VFCount", vectorFieldsList.Count);
		}

		private void Update()
		{
			if (vectorData == null)
				return;
			
			if (GenerateGPUTexture)
				GenerateTexture(false);

			if (transform.hasChanged)
			{
				ComputeWorldBounds();
				transform.hasChanged = false;
			}

			float localTime;

#if UNITY_EDITOR
            localTime = (float)(EditorApplication.timeSinceStartup%10000.0);
#else
			localTime = Time.time;
#endif
			if (Animate)
			{
				switch (AnimationType)
				{
					case AutoAnimationType.Forward:
					{
						float relativeTimePos = (localTime%Duration) / Duration;
						AnimIntensity = Curve.Evaluate(relativeTimePos);
					}
						break;
					case AutoAnimationType.Backward:
					{
						float relativeTimePos = (localTime%Duration) / Duration;
						AnimIntensity = Curve.Evaluate(1.0f - relativeTimePos);
					}
						break;
					case AutoAnimationType.PingPong:
					{
						float relativeTimePos = (localTime%(Duration*2)) / Duration;
						if (relativeTimePos>1.0f)
							relativeTimePos = 2.0f-relativeTimePos;
						
						AnimIntensity = Curve.Evaluate(relativeTimePos);
					}
						break;
				}
			}
			else
			{
				AnimIntensity = 1.0f;
			}
		}

		private void ComputeWorldBounds()
		{
			if (worldBounds == null)
				return;

			for (int i = 0; i<8; i++)
				worldBounds[i] = transform.TransformPoint(localBounds[i]);
		}

		private void DisplayVF()
		{
			if (vectorData == null)
				return;

			Vector3 position = Vector3.zero;
			Vector3 worldPosition = Vector3.zero;
			Vector3 worldDirection = Vector3.zero;

			if (arrowMesh == null)
				arrowMesh = (Mesh)Resources.Load("Models/VF_Arrow",typeof(Mesh));
			
			Color gColor = VectorFieldColor;
			Gizmos.color = gColor;

			float baseScale = Mathf.Max(Mathf.Min(Mathf.Min(
						Vector3.Distance(worldBounds[0], worldBounds[1])/vectorSizeX,
						Vector3.Distance(worldBounds[0], worldBounds[2])/vectorSizeY),
						Vector3.Distance(worldBounds[0], worldBounds[4])/vectorSizeZ), 10.0f);

			baseScale *= globalScale*GizmoScale;
			
			for (int y = 0; y<vectorSizeY; y++)
			{
				float rY = (float)(y+0.5f)/(float)vectorSizeY;
				position.y = Mathf.Lerp(minCorner.y, maxCorner.y, rY);
				for (int x = 0; x<vectorSizeX; x++)
				{
					float rX = (float)(x+0.5f)/(float)vectorSizeX;
					position.x = Mathf.Lerp(minCorner.x, maxCorner.x, rX);
					for (int z = 0; z<vectorSizeZ; z++)
					{
						Color gColorFixed = gColor; 
						float rZ = (float)(z+0.5f)/(float)vectorSizeZ;
						position.z = Mathf.Lerp(minCorner.z, maxCorner.z, rZ);
						Vector3 localDirection = vectorData[x+y*vectorSizeX+z*vectorSizeX*vectorSizeY]*Intensity*AnimIntensity;
						
						worldPosition = transform.TransformPoint(position);
						worldDirection = transform.TransformDirection(localDirection * baseScale );

						if (worldDirection.magnitude>0.001f)
						{
							switch (ShowVectorFieldAs)
							{
								case GizmosShape.ColoredArrows:
									gColorFixed.r = Mathf.Clamp01(0.5f + localDirection.x*.5f);
									gColorFixed.g = Mathf.Clamp01(0.5f + localDirection.y*.5f);
									gColorFixed.b = Mathf.Clamp01(0.5f + localDirection.z*.5f);
									Gizmos.color = gColorFixed;
									Gizmos.DrawMesh(arrowMesh, worldPosition, Quaternion.LookRotation(worldDirection),
										Vector3.one*baseScale);
									break;
								case GizmosShape.ScaledArrows:
									Gizmos.DrawMesh(arrowMesh, worldPosition, Quaternion.LookRotation(worldDirection),
										Vector3.one*worldDirection.magnitude);
									break;
								case GizmosShape.ColoredLines:
									gColorFixed.r = Mathf.Clamp01(0.5f + localDirection.x*.5f);
									gColorFixed.g = Mathf.Clamp01(0.5f + localDirection.y*.5f);
									gColorFixed.b = Mathf.Clamp01(0.5f + localDirection.z*.5f);
									Gizmos.color = gColorFixed;
									Gizmos.DrawRay(worldPosition, worldDirection.normalized*baseScale*3.0f);

									break;
								case GizmosShape.ScaledLines:
									Gizmos.color = gColorFixed;
									Gizmos.DrawRay(worldPosition, worldDirection*3.0f);
									break;
							}
						}
					}
				}
			}
		}

		void DisplayBBox(Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(worldBounds[0], worldBounds[1]);
			Gizmos.DrawLine(worldBounds[1], worldBounds[3]);
			Gizmos.DrawLine(worldBounds[3], worldBounds[2]);
			Gizmos.DrawLine(worldBounds[2], worldBounds[0]);

			Gizmos.DrawLine(worldBounds[4], worldBounds[5]);
			Gizmos.DrawLine(worldBounds[5], worldBounds[7]);
			Gizmos.DrawLine(worldBounds[7], worldBounds[6]);
			Gizmos.DrawLine(worldBounds[6], worldBounds[4]);

			Gizmos.DrawLine(worldBounds[0], worldBounds[4]);
			Gizmos.DrawLine(worldBounds[1], worldBounds[5]);
			Gizmos.DrawLine(worldBounds[2], worldBounds[6]);
			Gizmos.DrawLine(worldBounds[3], worldBounds[7]);
			
		}
		
		private void OnDrawGizmos()
		{
			if (vectorData == null)
				return;

			if (!ShowGizmos)
				return;
			
			DisplayBBox(Color.gray);

			if (AlwaysDisplayVF)
			{
				DisplayVF();
			}
		}
		
		private void OnDrawGizmosSelected()
		{
			if (vectorData == null)
				return;

			if (!ShowGizmos)
				return;

			if (ShowVectorFieldAs == GizmosShape.None)
				return;
			
			if (!AlwaysDisplayVF)
				DisplayVF();

			DisplayBBox(Color.white);
		}

#if UNITY_EDITOR
        [MenuItem("GameObject/Effects/Vector Field", false, 10)]
		static void CreateVectorField(MenuCommand menuCommand)
		{
			GameObject go = new GameObject("VectorField");
			go.AddComponent<VectorField>();
			GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
			Selection.activeObject = go;
		}
#endif
    }
}