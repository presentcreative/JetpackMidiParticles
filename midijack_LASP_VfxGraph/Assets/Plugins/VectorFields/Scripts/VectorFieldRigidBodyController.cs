using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JangaFX
{
	[AddComponentMenu("Vector Field/Rigidbody Controller")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Rigidbody))]
	public class VectorFieldRigidBodyController: MonoBehaviour
	{
		public enum ForceBlendingMode
		{
			ReplaceVelocity,
			AddForce
		}

		[Range(0f, 1f)]
		public float Tightness=1.0f;
		private const float MinimalInfluence = 0.05f;
		public float Multiplier = 1.0f;
		public bool AffectedByAllVF = true;
		public List<VectorField> VFRestrictedList = new List<VectorField>();

		Rigidbody rb;

		private void Awake()
		{
			rb = this.GetComponent<Rigidbody>();
		}

		private float hlslSmoothstep(float min, float max, float value)
		{
			float t = Mathf.Clamp01((value - min) / (max - min));
			return t * t * (3.0f - 2.0f * t);
		}
	
		void Update ()
		{
			Vector3 force;

			if (AffectedByAllVF)
				force = VectorField.GetCombinedVectors(transform.position)*Multiplier;
			else
				force = VectorField.GetCombinedVectorsRestricted(transform.position, VFRestrictedList)*Multiplier;

			float intensity = force.magnitude;
			float blendIntensity = hlslSmoothstep(0.0f, MinimalInfluence+0.0001f, intensity);

			float SQTightness = Tightness*Tightness;
			
			rb.velocity = Vector3.Lerp(rb.velocity, force, SQTightness*blendIntensity);
			rb.AddForce(force*(1.0f - SQTightness)*blendIntensity, ForceMode.Force);
		}
	}
}