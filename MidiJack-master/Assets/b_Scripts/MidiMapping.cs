using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using MidiGetter;

public enum ParticleParameters {



    StartLifeTime,
    StartSpeed,
    StartSize,
    StartRotation,
    GravityMod,
    SimSpeed,
    EmissionRate,
    ShapeAngle,
    ShapeRadius,
    Dampen,
    ForceOverLifetimeX,
    ForceOverLifetimeY,
    ForceOverLifetimeZ,
    NoiseFreq,
    NoiseStrength,
    NoiseSizeAmount,
    TrailsWidthOverTrails,
    TrailsRibbonCount
}
public class MidiMapping : MonoBehaviour
{
    public ParticleParameters particleParametersEnum;
    public GameObject Logic;
    public bool editMode = false;
    public int editableButtonNum;
    public int[] particleParams = new int[128];//array of CC numbers to Switch Number

    /*
     * LIST:
     * https://docs.google.com/spreadsheets/d/15kZxqkWyTQan2jUgxxzp13Im2Ho-FEw3r3c5pc22KBw/edit#gid=0
     1	StartLifeTime,
2	StartSpeed,
3	StartSize,
4	StartRotation,
5	GravityMod,
6	SimSpeed,
7	EmissionRate,
8	ShapeAngle,
9	ShapeRadius,
10	Dampen,
11	ForceOverLifetimeX,
12	ForceOverLifetimeY,
13	ForceOverLifetimeZ,
14	NoiseFreq,
15	NoiseStrength,
16	NoiseSizeAmount,
17	TrailsWidthOverTrails,

        18, TrailsRibbonCount
        19, RendererSpeedScale
        20   RendererLengthScale
21 RotateSystemX22222222
        */


    // Update is called once per frame
    void Update()
    {
        /*
        if (editMode == true) {
            particleParams[MidiValueGetter.currentKnobNum] =  editableButtonNum; 
        }
        */
        if (Input.GetKeyDown("space"))
        {
            if(editMode == true)
            {
                Debug.Log("MidiValueGetter.currentKnobNum is " + MidiValueGetter.currentKnobNum);
                Debug.Log("particleParams[MidiValueGetter.currentKnobNum] " + particleParams[MidiValueGetter.currentKnobNum]);
                Debug.Log("editableButtonNum " + editableButtonNum);
            }
            //Debug.Log(midiParticleSystem.main.simulationSpeed);
            //         StartSize();
        }
    }
    public void EditThisChannel()
    {
        Debug.Log("Edit this CHannel " + particleParams[MidiValueGetter.currentKnobNum]);
        particleParams[MidiValueGetter.currentKnobNum] = editableButtonNum;
        editMode = false;
    }

    public void EditParticleParameter(int whichButton)
    {
        //save switch number and put in right place in CC array

        if (editMode == true)
        {
            print("edit mode was true but now is false" + editableButtonNum);
            editMode = false;
        }
           else if (editMode == false) { 
            
            editableButtonNum = whichButton;  //places button number in editbale num
            editMode = true;
            print("edit mode was false but now is true " + editableButtonNum);
        }
        
    }

}
