using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JangaFX
{
	[AddComponentMenu("Vector Field/Particle Controller")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ParticleSystem))]
	public class VectorFieldParticleController: MonoBehaviour
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

		public bool AnimateTightness = false;
		public AnimationCurve TightnessOverTime = new AnimationCurve();
		public bool AnimateMultiplier = false;
		public AnimationCurve MultiplierOverTime = new AnimationCurve();
		
		ParticleSystem.Particle[] particles=null;
		ParticleSystem ps;

		private void Awake()
		{
			ps = this.GetComponent<ParticleSystem>();
			ps.simulationSpace = ParticleSystemSimulationSpace.World;
		}


		private float hlslSmoothstep(float min, float max, float value)
		{
			float t = Mathf.Clamp01((value - min) / (max - min));
			return t * t * (3.0f - 2.0f * t);
		}
	
		void Update ()
		{
			if ((particles == null) || (ps.maxParticles != particles.Length))
				particles = new ParticleSystem.Particle[ps.maxParticles];
			
			int numParticles = ps.GetParticles(particles);

			for (int i = 0; i < numParticles; i++)
			{
				Vector3 force;

				float relativeLife = 1.0f-particles[i].remainingLifetime/particles[i].startLifetime;
				
				if (AffectedByAllVF)
					force = VectorField.GetCombinedVectors(particles[i].position)*Multiplier;
				else
					force = VectorField.GetCombinedVectorsRestricted(particles[i].position, VFRestrictedList)*Multiplier;
					
				float intensity = force.magnitude;
				float blendIntensity = hlslSmoothstep(-0.0001f, MinimalInfluence, intensity);
				
				float finalTightness = Tightness*Tightness;
				if (AnimateTightness)
					finalTightness *=
						TightnessOverTime.Evaluate(relativeLife);
				if (AnimateMultiplier)
					force *= MultiplierOverTime.Evaluate(relativeLife);
				
				particles[i].velocity = Vector3.Lerp(particles[i].velocity + force * Time.smoothDeltaTime *blendIntensity, force, finalTightness*blendIntensity);
			}

			ps.SetParticles(particles, numParticles);
		}
	}
}