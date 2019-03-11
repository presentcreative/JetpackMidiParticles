﻿
using UnityEngine;
using MidiJack;
using System.Collections;
using System.Collections.Generic;

//BASED OFF DELEGRATE TESTER BY KEIJIRO
//SENDS LISTENS FOR MIDI HARDWARE AND PUTS VALUES IN VARIABLES 

namespace MidiGetter { 
    public class MidiValueGetter : MonoBehaviour
    {
        public MidiMapping midiMappingScript;
        public MidiParamController midiParamControllerScript;
        //public Transform Logic;
        public static int currentKnobNum;

        void NoteOn(MidiChannel channel, int note, float velocity)
        {
            Debug.Log("NoteOn: " + channel + "," + note + "," + velocity);
        }

        void NoteOff(MidiChannel channel, int note)
        {
            Debug.Log("NoteOff: " + channel + "," + note);
        }

        void Knob(MidiChannel channel, int knobNumber, float knobValue)
        {
            //Debug.Log("Knob: " + knobNumber + "," + knobValue);
            currentKnobNum = knobNumber;
            // var indicator = indicatorGroup.GetComponent<KnobIndicatorGroup>();
            //indicator.lastKnobTouched = knobNumber;
            //objectWithOtherScript.GetComponent.< script1 > ().someVariable = someNumber;
            midiParamControllerScript.UniController(knobNumber, knobValue);
            
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
