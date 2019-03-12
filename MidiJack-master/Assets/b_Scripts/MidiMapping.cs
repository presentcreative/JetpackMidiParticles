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
    public GameObject Logic;
    public bool editMode = false;
    public int editableButtonNum;
    public int[] particleParams = new int[128];
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
21 RotateSystemX
        */


    // Update is called once per frame
    void Update()
    {
        if (editMode == true) { 
            particleParams[editableButtonNum] = MidiValueGetter.currentKnobNum;
        }
    }
    public void EditParticleParameter(int whichButton)
    {
        if (editMode == true)
        {
            print("edit mode was true but now is false" + editableButtonNum);
            editMode = false;
        }
           else if (editMode == false) { 
            editMode = true;
            editableButtonNum = whichButton;
        print("edit mode was false but now is true " + editableButtonNum);
        }
        
    }

}
