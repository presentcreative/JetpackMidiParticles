using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace JangaFX
{
	[AddComponentMenu("Vector Field/Particle Emitter")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ParticleSystem))]
	public class VectorFieldParticleEmitter : MonoBehaviour
	{
		public enum Emission
		{
			VectorOrigins,
			InsideField,
			Corners,
			Edges,
			Surface, 
			FaceXPositive,
			FaceXNegative,
			FaceYPositive,
			FaceYNegative,
			FaceZPositive,
			FaceZNegative
		};

		public VectorField VectorFieldSource;
		public Emission EmissionType;
		[Range(0f, 1f)]
		public float Coverage = 1.0f;
		
		ParticleSystem ps;
		ParticleSystem.EmitParams emitParams;
		float emissionRate = 0;
		float timer = 0;

		private void Awake()
		{
			ps = this.GetComponent<ParticleSystem>();
			ps.playOnAwake = false;
			ps.Stop();
			emitParams = new ParticleSystem.EmitParams();
			emitParams.startColor = ps.startColor;
			emitParams.startSize = ps.startSize;
			emitParams.startLifetime = ps.startLifetime;
			emissionRate = 1f / ps.emission.rateOverTime.constant;
		}

		void FixedUpdate()
		{
			if (VectorFieldSource == null)
				return;
			
			if (timer > emissionRate)
			{
				int numToEmit = (int) (timer / emissionRate);

				float invCoverage = 1.0f-Coverage;
				
				for (int i = 0; i < numToEmit; i++)
				{

					switch (EmissionType)
					{
						case Emission.VectorOrigins:
							{
								Vector3 pos;
								Vector3 dir;
								VectorFieldSource.GetRandomVector(out pos, out dir);
								
								emitParams.position = pos;
								emitParams.velocity = dir;
							}
							break;
						case Emission.InsideField:
							emitParams.position = VectorFieldSource.GetPointInField(invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.Corners:
							emitParams.position = VectorFieldSource.GetRandomCorner();
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.Edges:
							emitParams.position = VectorFieldSource.GetPointOnEdge(invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.Surface:
							emitParams.position = VectorFieldSource.GetPointOnVolume(invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceXPositive:
							emitParams.position = VectorFieldSource.GetPointOnFace(5,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceXNegative:
							emitParams.position = VectorFieldSource.GetPointOnFace(3,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceYPositive:
							emitParams.position = VectorFieldSource.GetPointOnFace(4,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceYNegative:
							emitParams.position = VectorFieldSource.GetPointOnFace(2,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceZPositive:
							emitParams.position = VectorFieldSource.GetPointOnFace(1,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						case Emission.FaceZNegative:
							emitParams.position = VectorFieldSource.GetPointOnFace(0,invCoverage);
							emitParams.velocity = VectorFieldSource.GetVector(emitParams.position);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					ps.Emit(emitParams, 1);
				}
				timer = 0;
			}
			timer += Time.deltaTime;
		}
	}
}