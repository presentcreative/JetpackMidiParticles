using UnityEngine;
using System.Collections.Generic;
using MidiJack;

public class KnobIndicatorGroup : MonoBehaviour
{

    public GameObject prefab;

    public ParticleSystem midiParticleSystem;
    public List<KnobIndicator> indicators;

    //ben variables
   
    public int midiStartSizeNum;
    public float midiStartSize;
    //public float midiStartSizePrev;
    public int midiStartSpeedNum;
    public float midiStartSpeed;
    //public float midiStartSpeedPrev;
    public int midiEmissionRateNum;
    public float midiEmissionRate;
   // public float midiEmissionRatePrev;
    public int midiGravityModifierNum;
    public float midiGravityModifier;
    //public float midiGravityModifierPrev;
    public int midiStartLifetimeNum;
    public float midiStartLifetime;
    //public float midiStartLifetimePrev;
    public int midiForceOverLifetimeNum;
    public float midiForceOverLifetime;
   // public float midiForceOverLifetimePrev;
    public int midiDampenNum;
    public float midiDampen;
    //public float midiDampenPrev;
    public int midiLifetimeNum;
    public float midiLifetime;
    //public float midiLifetimePrev;
    public int midiShapeAngleNum;
    public float midiShapeAngle;
    //public float midiShapeAnglePrev;

    public int newMidiCCNum;
    public bool editMode;
    public int editableButtonNum;
    public int lastKnobTouched;
    public int lastKnobTouchedPrev;

    //used to story the midi CC numbers assigned to each button.
    public int[] particleParams = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };  

    public List<int> midiWokeKnobs; // a list of the the midi CC numbers created in the order they were touched
    public List<float> midiKnobVal; // a list of the values come from those knobs in the same order
    public List<float> midiKnobValPrev;

    //public List<int> midiChannels;

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
            indicators.Add(indicator);  // this is original script and needs to be turned back on
            //ben adds to his list
            //midiWokeKnobs.Add(indicator.knobNumber);
           midiKnobVal.Add(MidiMaster.GetKnob(indicator.knobNumber));
           midiKnobValPrev.Add(MidiMaster.GetKnob(indicator.knobNumber));
            
            
        }
        
        //ben section
        /* foreach (int i in midiKnobVal)
         {
             int midiI = i + 1;
             print(i);
             midiKnobVal[i] = Map(0, 300, 0, 1, MidiMaster.GetKnob(midiI));
         }
         */
        /*
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
       */

        if (Input.GetButton("Fire1"))
        {
            //Debug.Log(midiParticleSystem.main.simulationSpeed);
   //         StartSize();
        }
        //SCRIPT FOR KEEPING TRACK OF WHICH MIDI SIGNALS ARE COMING THROUGH
        if (editMode == false)
        {
            for (int i = 0; i < midiKnobVal.Count; i++)
            {
                int iAdjust = i + 1;
                midiKnobValPrev[i] = MidiMaster.GetKnob(iAdjust);
                midiKnobVal[i] = MidiMaster.GetKnob(iAdjust);
            }
            
        }


        //SCRIPT WHICH BUTTON WAS PRESSED IF YOU ARE IN EDIT MODE
        if (editMode == true) {
            lastKnobTouchedPrev = lastKnobTouched;
            for (int i = 0; i < midiKnobVal.Count; i++)
            {
                //midiKnobValPrev[i] = MidiMaster.GetKnob(i);
                midiKnobVal[i] = MidiMaster.GetKnob(i);
            }
            for (int i = 0; i < midiKnobVal.Count; i++)
            {
                if (midiKnobVal[i] != midiKnobValPrev[i])
                    lastKnobTouched = channels[i];
            }

            //list for knob
            //if knob touched put knobnumber in particleParams[editableButtonNum]
            particleParams[editableButtonNum] = lastKnobTouched;
            if (lastKnobTouched != lastKnobTouchedPrev) {
                editMode = false;
            }
            
        }
        /*
        //START SIZE
        midiStartSize = MidiMaster.GetKnob(midiStartSizeNum);
        if (midiStartSize != midiStartSizePrev)
        {
            StartSize(MidiMaster.GetKnob(midiStartSizeNum));
            
        }*/
    }

    //CHANGE KNOB ASSIGNMENT
    //button array: 1 entry per function with the controller number put in
    //click button selects that buttons entry on the array
    //touch knob assigns knob to selected button
    //

   

    //SCRIPT RECEIVED BY ANY BUTTON CLICK TO START EDIT MODE
    public void ChangeCCNum(int whichButton)
    {
        editMode = true;
        
        editableButtonNum = whichButton;
        print("edit mode is true and editable button clicked was " + editableButtonNum);
    }

    /*
    void StartSize(float passedMidiValue)
    {
        midiStartSize = Map(0, 300, 0, 1, passedMidiValue);
        midiParticleSystem.startSize = midiStartSize;
        midiStartSizePrev = midiStartSize;
    }
    */
    void StartSpeed(float passedMidiValue)
    {
        midiStartSpeed = Map(-500, 500, 0, 1, passedMidiValue);
        midiParticleSystem.startSpeed = midiStartSpeed;
        
    }
    void EmissionRate(float passedMidiValue)
    {
        midiEmissionRate = Map(0, 500, 0, 1, passedMidiValue);
        midiParticleSystem.emissionRate = midiEmissionRate;
        // }
    }
    void GravityModifier(float passedMidiValue)
    {
        midiGravityModifier = Map(0, 500, 0, 1, passedMidiValue);
        midiParticleSystem.gravityModifier = midiGravityModifier;
        // }
    }
    void StartLifeTime(float passedMidiValue)
    {
        midiLifetime = Map(0, 15, 0, 1, passedMidiValue);
        midiParticleSystem.startLifetime = midiLifetime;
        // }
    }
    void Dampen(float passedMidiValue)
    {
        midiDampen = Map(0, 1, 0, 1, passedMidiValue);
        var limitVelocityModule = midiParticleSystem.limitVelocityOverLifetime;
        limitVelocityModule.enabled = true;
        limitVelocityModule.dampen = midiDampen;
    }
    void ShapeAngle(float passedMidiValue)
    {
        midiShapeAngle = Map(0, 500, 0, 1, passedMidiValue);
        var limitVelocityModule = midiParticleSystem.shape;
        // Enable it and set a value
        //limitVelocityModule.enabled = true;
        limitVelocityModule.angle = midiShapeAngle;
    }
    void ForceOverLifeTime(float passedMidiValue)
    {
        //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
        midiForceOverLifetime = Map(0, 500, 0, 1, passedMidiValue);
        var forceModule = midiParticleSystem.forceOverLifetime;
        // Enable it and set a value
        //forceModule.enabled = true;
        forceModule.y = new ParticleSystem.MinMaxCurve(midiForceOverLifetime);
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
