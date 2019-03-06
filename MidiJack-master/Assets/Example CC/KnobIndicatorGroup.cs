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
    public float midiParticleStartSpeed;
    public float midiParticleEmissionRate;
    public float midiParticleGravityModifier;
    public float midiParticleStartLifetime;
    public float midiParticleforceOverLifetime;
    public float midiParticleDampen;
    public float midiParticleLifetime;
    public float midiParticleShapeAngle;

   

    public List<int> midiWokeKnobs; // a list of the the midi CC numbers created in the order they were touched
    public List<float> midiKnobVal; // a list of the values come from those knobs in the same order
    public List<float> midiKnobValPrev;

    void Start()
    {
        indicators = new List<KnobIndicator>();
    }

    void Update()
    {
        
        var channels = MidiMaster.GetKnobNumbers();

        // If a new channel was added...
        if (indicators.Count != channels.Length)
        {
            // Instantiate the new indicator.
            var go = Instantiate<GameObject>(prefab);  // this is for the bars for each new midi knob
            go.transform.position = Vector3.right * indicators.Count;

            // Initialize the indicator.
            var indicator = go.GetComponent<KnobIndicator>();
            indicator.knobNumber = channels[indicators.Count];

            // Add it to the indicator list.
            indicators.Add(indicator);
            //ben adds to his list
            midiWokeKnobs.Add(indicator.knobNumber);
           midiKnobVal.Add(MidiMaster.GetKnob(midiWokeKnobs.Count));
           midiKnobValPrev.Add(MidiMaster.GetKnob(midiWokeKnobs.Count));
            //midiConnectors.Add(midiWokeKnobs[indicator.Count]);
            //midiConnectors[indicators.Count] = midiWokeKnobs[indicators.Count];
        }

        //ben section
        /* foreach (int i in midiKnobVal)
         {
             int midiI = i + 1;
             print(i);
             midiKnobVal[i] = Map(0, 300, 0, 1, MidiMaster.GetKnob(midiI));
         }
         */

        for (int i = 0; i < midiKnobVal.Count; i++)
        {
            Debug.Log("for statement things the midiKnobVal Count is " + midiKnobVal.Count + "and the current number is " + i);
            int midiI = i + 1;
            midiKnobVal[i] = MidiMaster.GetKnob(midiI);
            if(midiKnobValPrev[i] != midiKnobVal[i])
            {
                
                
                switch (i)
                {
                    case 7:
                        ShapeAngle(midiKnobVal[i]);
                        break;
                    case 6:
                        Dampen(midiKnobVal[i]);
                        break;
                    case 5:
                        print("5");
                        ForceOverLifeTime(midiKnobVal[i]);
                        break;
                    case 4:
                        print("4");
                        StartLifeTime(midiKnobVal[i]);
                        break;
                    case 3:
                        print("3");
                        GravityModifier(midiKnobVal[i]);
                        break;
                    case 2:
                        print("2");
                        EmissionRate(midiKnobVal[i]);
                        break;
                    case 1:
                        print("1 start size");
                        StartSpeed(midiKnobVal[i]);
                        break;
                    default:
                        print("start size");
                        StartSize(midiKnobVal[i]);
                        break;
                }
                midiKnobValPrev[i] = midiKnobVal[i];
            }
            //Debug.Log("midiKnobVale COunt is " + i);
        }
        if (Input.GetButton("Fire1"))
        {
            //Debug.Log(midiParticleSystem.main.simulationSpeed);
   //         StartSize();
        }

    }

    //CHANGE KNOB ASSIGNMENT
    //click button
    //touch knob assigns knob to button
    //

    void StartSize(float passedMidiValue)
    {
        midiParticleStartSize = Map(0, 300, 0, 1, passedMidiValue);
        midiParticleSystem.startSize = midiParticleStartSize;
       // }
    }
    void StartSpeed(float passedMidiValue)
    {
        midiParticleStartSpeed = Map(-500, 500, 0, 1, passedMidiValue);
        midiParticleSystem.startSpeed = midiParticleStartSpeed;
        // }
    }
    void EmissionRate(float passedMidiValue)
    {
        midiParticleEmissionRate = Map(0, 500, 0, 1, passedMidiValue);
        midiParticleSystem.emissionRate = midiParticleEmissionRate;
        // }
    }
    void GravityModifier(float passedMidiValue)
    {
        midiParticleGravityModifier = Map(0, 500, 0, 1, passedMidiValue);
        midiParticleSystem.gravityModifier = midiParticleGravityModifier;
        // }
    }
    void StartLifeTime(float passedMidiValue)
    {
        midiParticleLifetime = Map(0, 15, 0, 1, passedMidiValue);
        midiParticleSystem.startLifetime = midiParticleLifetime;
        // }
    }
    void Dampen(float passedMidiValue)
    {
        midiParticleDampen = Map(0, 1, 0, 1, passedMidiValue);
        var limitVelocityModule = midiParticleSystem.limitVelocityOverLifetime;
        limitVelocityModule.enabled = true;
        limitVelocityModule.dampen = midiParticleDampen;
    }
    void ShapeAngle(float passedMidiValue)
    {
        midiParticleShapeAngle = Map(0, 500, 0, 1, passedMidiValue);
        var limitVelocityModule = midiParticleSystem.shape;
        // Enable it and set a value
        //limitVelocityModule.enabled = true;
        limitVelocityModule.angle = midiParticleShapeAngle;
    }
    void ForceOverLifeTime(float passedMidiValue)
    {
        //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
        midiParticleforceOverLifetime = Map(0, 500, 0, 1, passedMidiValue);
        var forceModule = midiParticleSystem.forceOverLifetime;
        // Enable it and set a value
        //forceModule.enabled = true;
        forceModule.y = new ParticleSystem.MinMaxCurve(midiParticleforceOverLifetime);
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
