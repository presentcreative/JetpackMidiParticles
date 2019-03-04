using UnityEngine;
using System.Collections.Generic;
using MidiJack;

public class KnobIndicatorGroup : MonoBehaviour
{

    public GameObject prefab;

    public ParticleSystem midiParticleSystem;
    public List<KnobIndicator> indicators;

    //ben variables
    public float midiParticleStartSize;
    public float midiParticleStartSizePrev;

    public float midiParticleStartSpeed;
    public float midiParticleStartSpeedPrev;

    public float midiParticleEmissionRate;
    public float midiParticleEmissionRatePrev;

    public float midiParticleGravityModifier;
    public float midiParticleGravityModifierPrev;

    public float midiParticleStartLifetime;
    public float midiParticleStartLifetimePrev;

    public float midiParticleforceOverLifetime;
    public float midiParticleforceOverLifetimePrev;

    public float midiParticleDampen;
    public float midiParticleDampenPrev;

    public float midiParticleLifetime;
    public float midiParticleLifetimePrev;

    public List<int> midiWokeKnobs; // the midi CC number
   // public List<float> midiValuesList; // the midi CC number
    //public List<float> midiConnectors;  // for the actual midi flaot values

    void Start()
    {
        indicators = new List<KnobIndicator>();
    }

    void Update()
    {
        
        var channels = MidiMaster.GetKnobNumbers();

        // If a new chennel was added...
        if (indicators.Count != channels.Length)
        {
            // Instantiate the new indicator.
            var go = Instantiate<GameObject>(prefab);
            go.transform.position = Vector3.right * indicators.Count;

            // Initialize the indicator.
            var indicator = go.GetComponent<KnobIndicator>();
            indicator.knobNumber = channels[indicators.Count];

            // Add it to the indicator list.
            indicators.Add(indicator);
            //ben adds to his list
            midiWokeKnobs.Add(indicator.knobNumber);
            //midiConnectors.Add(midiWokeKnobs[indicator.Count]);
            //midiConnectors[indicators.Count] = midiWokeKnobs[indicators.Count];
        }

        //ben section
        //var s = MidiMaster.GetKnob(knobNumber);
        // MidiVal1 = MidiMaster.GetKnob(midiWokeKnobs[0]); // this version works for loading in the midiWokeKnobs as they get created
        //MidiVal0 = MidiMaster.GetKnob(0);
        /*if (midiWokeKnobs.Count > 0) {
            midiVal1 = Map(0, 20, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[1]));
        }
        */
        /*
        midiVal2 = Map(0, 300, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[2]));
        midiVal3 = Map(0, 200, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[3]));
        midiVal4 = Map(0, 200, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[4]));
        midiVal5 = Map(0, 10, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[5]));
        midiVal6 = Map(0, 255, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[6]));
        midiVal7 = Map(0, 255, 0, 1, MidiMaster.GetKnob(midiWokeKnobs[7]));
        */

        /*
        midiParticleSystem.startSize = midiVal1;
        midiParticleSystem.startSpeed = midiVal2;
        midiParticleSystem.emissionRate = midiVal3;
        midiParticleSystem.gravityModifier = midiVal4;
        midiParticleSystem.startLifetime = midiVal5;
        */

        //STARTSIZE
        midiParticleStartSize = Map(0, 300, 0, 1, MidiMaster.GetKnob(1));
        if (midiParticleStartSize != midiParticleStartSizePrev)
        {
            
            midiParticleSystem.startSize = midiParticleStartSize;
            midiParticleStartSizePrev = midiParticleStartSize;
        }

        //STARTSPEED
        midiParticleStartSpeed = Map(-400, 400, 0, 1, MidiMaster.GetKnob(2));
        if (midiParticleStartSpeed != midiParticleStartSpeedPrev)
        {
            
            midiParticleSystem.startSpeed = midiParticleStartSpeed;
            midiParticleStartSpeedPrev = midiParticleStartSpeed;
        }

        //EMISSION RATE
        midiParticleEmissionRate = Map(0, 500, 0, 1, MidiMaster.GetKnob(3));
        if (midiParticleEmissionRate != midiParticleEmissionRatePrev)
        {
            midiParticleSystem.emissionRate = midiParticleEmissionRate;
            midiParticleEmissionRatePrev = midiParticleEmissionRate;
        }

        //GRAVITY MODIFIER
        midiParticleGravityModifier = Map(0, 500, 0, 1, MidiMaster.GetKnob(4));
        if (midiParticleGravityModifier != midiParticleGravityModifierPrev)
        {
            midiParticleSystem.gravityModifier = midiParticleGravityModifier;
            midiParticleGravityModifierPrev = midiParticleGravityModifier;
        }

        //STARTLIFETIME
        midiParticleLifetime = Map(0, 20, 0, 1, MidiMaster.GetKnob(5));
        if (midiParticleLifetime != midiParticleLifetimePrev)
        {
            midiParticleSystem.startLifetime = midiParticleLifetime;
            midiParticleLifetimePrev = midiParticleLifetime;
        }
        //FORCE OVER LIFETIME
        midiParticleforceOverLifetime = Map(0, 500, 0, 1, MidiMaster.GetKnob(6));
        //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
        if (midiParticleforceOverLifetime != midiParticleforceOverLifetimePrev) {
            
            var forceModule = midiParticleSystem.forceOverLifetime;
            // Enable it and set a value
            //forceModule.enabled = true;
            forceModule.y = new ParticleSystem.MinMaxCurve(midiParticleforceOverLifetime);
            midiParticleforceOverLifetimePrev = midiParticleforceOverLifetime;
        }

        //DAMPEN
        midiParticleDampen = Map(0, 1, 0, 1, MidiMaster.GetKnob(81));
        //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
        if (midiParticleDampen != midiParticleDampenPrev)
        { 
            var limitVelocityModule = midiParticleSystem.limitVelocityOverLifetime;
            // Enable it and set a value
            //limitVelocityModule.enabled = true;
            limitVelocityModule.dampen = midiParticleDampen;
            midiParticleDampenPrev = midiParticleDampen;
        }


        if (Input.GetButton("Fire1"))
        {
            //Debug.Log(midiParticleSystem.main.simulationSpeed);
            //var forceModule = GetComponent<ParticleSystem>().forceOverLifetime;
            var forceModule = midiParticleSystem.forceOverLifetime;

            // Enable it and set a value
            forceModule.enabled = true;
            forceModule.x = new ParticleSystem.MinMaxCurve(15.0f);
        }

    }

    // used to normalize any value range (new min, new max, old min, old max, oldValue)
    public float Map(float from, float to, float from2, float to2, float value)
    {
        if (value <= from2)
        {
            return from;
        }
        else if (value >= to2)
        {
            return to;
        }
        else
        {
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }
    }
}
