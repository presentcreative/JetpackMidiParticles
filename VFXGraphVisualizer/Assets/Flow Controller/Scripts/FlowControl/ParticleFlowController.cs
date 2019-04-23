/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Add this component to a particle system GameObject to have it dictate the movement
    /// of particles according to a flow control region.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleFlowController : MonoBehaviour
    {
        private ParticleSystem particleSys;
        private ParticleSystem.Particle[] particles;
#if UNITY_5_6_OR_NEWER
        private ParticleSystem.CustomDataModule customData;
#endif
        private List<Vector4> _data;

        public FlowControlRegion flowControlRegion;

        [Range(0f, 1f)]
        public float DirectionalInertia = 0f;
        public float Speed = 3f;

        private void Start()
        {
            particleSys = GetComponent<ParticleSystem>();
#if UNITY_5_6_OR_NEWER
            if (!particleSys.customData.enabled)
            {
                customData = particleSys.customData;
                customData.enabled = true;
                customData.SetMode(ParticleSystemCustomData.Custom1, ParticleSystemCustomDataMode.Disabled);
            }
#endif
            _data = new List<Vector4>();
        }

        void Update()
        {
            if (particles == null)
            {
                particleSys = GetComponent<ParticleSystem>();
                particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
            }

            if (_data == null)
            {
                _data = new List<Vector4>();
            }

            int numParticles = particleSys.GetParticles(this.particles);
            particleSys.GetCustomParticleData(_data, ParticleSystemCustomData.Custom1);

            float scaledInertia = 0.9f + DirectionalInertia / 10f;

            for (int i = 0; i < numParticles; i++)
            {
                ParticleSystem.Particle p = particles[i];
                Vector4 d = _data[i];

                if (d.x < p.remainingLifetime)
                {
                    d.w = 0;
                }

                Vector3 force;
                bool inRegion = flowControlRegion.SampleWorldCoord(p.position, out force, d.w < 2);
                if (force == Vector3.zero)
                {
                    force = p.velocity.normalized;
                }
                else
                {
                    force.Normalize();
                }

                Vector3 velocity;

                if (p.velocity == Vector3.zero)
                {
                    velocity = (force * Speed);
                }
                else
                {
                    velocity = ((p.velocity * scaledInertia) + (force * Speed * (1f - scaledInertia)));
                }

                particles[i].velocity = velocity;

                velocity.Normalize();

                if (inRegion)
                {
                    if (d.w == 0) /* 0 == entering */
                    {
                        _data[i] = new Vector4(p.remainingLifetime, 0, 0, 1);
                    }
                }
                else
                {
                    if (d.w == 1) /* 1 == in region */
                    {
                        _data[i] = new Vector4(p.remainingLifetime, 0, 0, 2); /* 2 == exiting */
                    }
                }
            }

            particleSys.SetCustomParticleData(_data, ParticleSystemCustomData.Custom1);
            particleSys.GetCustomParticleData(_data, ParticleSystemCustomData.Custom1);
            particleSys.SetParticles(particles, numParticles);
        }
    }
}
