
using UnityEngine;
using MidiJack;
using System.Collections;
using UnityEngine.Experimental.VFX;
using System.Collections.Generic;

//BASED OFF DelegateTester.cs BY KEIJIRO
// LISTENS FOR MIDI HARDWARE AND PUTS VALUES IN VARIABLES 

namespace MidiGetter { 
    public class MidiValueGetter : MonoBehaviour
    {
        // public MidiMapping midiMappingScript;
        // public MidiParamController midiParamControllerScript;
        // public particalSystemLoader particleSystemLoaderScript;
        // public VfxController vfxConntrol;

        public VisualEffect _target = null;
        public VfxController vfxControllerScript;
        //public Transform Logic;
        public static int currentKnobNum;

        void NoteOn(MidiChannel channel, int note, float velocity)
        {
            Debug.Log("NoteOn: " + channel + "," + note + "," + velocity);
           // particleSystemLoaderScript.NewHitParticleSystem(note,velocity);
        }

        void NoteOff(MidiChannel channel, int note)
        {
            Debug.Log("NoteOff: " + channel + "," + note);
        }

        void Knob(MidiChannel channel, int knobNumber, float knobValue)
        {
            Debug.Log("Knob: " + knobNumber + "," + knobValue);
            //currentKnobNum = knobNumber;
            // var indicator = indicatorGroup.GetComponent<KnobIndicatorGroup>();
            //indicator.lastKnobTouched = knobNumber;
            //objectWithOtherScript.GetComponent.< script1 > ().someVariable = someNumber;
            /* if(midiMappingScript.editMode == true)
             {
                 midiMappingScript.EditThisChannel();
             }
             if(midiMappingScript.editMode == false)
             {
                 midiParamControllerScript.UniController(knobNumber, knobValue);
             }
             */
            //vfxControllerScript.parameterValue = knobValue;
            if (knobNumber == 0) { 
            _target.SetFloat("Midi0", knobValue);
            }
            if (knobNumber == 1)
            {
                _target.SetFloat("Midi1", knobValue);
            }
            if (knobNumber == 2)
            {
                _target.SetFloat("Midi2", knobValue);
            }
            if (knobNumber == 3)
            {
                _target.SetFloat("Midi3", knobValue);
            }
            if (knobNumber == 4)
            {
                _target.SetFloat("Midi4", knobValue);
            }
            if (knobNumber == 5)
            {
                _target.SetFloat("Midi5", knobValue);
            }
            if (knobNumber == 6)
            {
                _target.SetFloat("Midi6", knobValue);
            }
            if (knobNumber == 7)
            {
                _target.SetFloat("Midi7", knobValue);
            }
            if (knobNumber == 8)
            {
                _target.SetFloat("Midi8", knobValue);
            }
            if (knobNumber == 9)
            {
                _target.SetFloat("Midi9", knobValue);
            }

        }

        void OnEnable()
        {
            MidiMaster.noteOnDelegate += NoteOn;
            MidiMaster.noteOffDelegate += NoteOff;
            MidiMaster.knobDelegate += Knob;
        }

        void OnDisable()
        {
            MidiMaster.noteOnDelegate -= NoteOn;
            MidiMaster.noteOffDelegate -= NoteOff;
            MidiMaster.knobDelegate -= Knob;
        }
    }
}
