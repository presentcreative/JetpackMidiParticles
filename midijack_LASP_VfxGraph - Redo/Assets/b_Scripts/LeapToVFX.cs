using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using Leap;

public class LeapToVFX : MonoBehaviour
{
   // public Transform LPalmTransform;
   // public GameObject LeftPalm;
    public float LPalmX;
    public float LPalmY;
    public float LPalmZ;
    public float DefaultLPalmX;
    public float DefaultLPalmY;
    public float DefaultLPalmZ;
    public float OldLPalmX;
    public float OldLPalmY;
    public float OldLPalmZ;

    public Transform PalmPosition;

    public VisualEffect _target = null;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (OldLPalmX != LPalmX)
        {
            _target.SetFloat("LeapX", LPalmX);
            OldLPalmX = LPalmX;
            _target.SetFloat("LeapY", LPalmY);
            OldLPalmY = LPalmY;
            _target.SetFloat("LeapZ", LPalmZ);
            OldLPalmZ = LPalmZ;

        }

        if (Input.GetKeyDown("space"))
        {
            Cleanup();



        }
        


    }
    public void Cleanup()
    {
        _target.SetFloat("LeapX", DefaultLPalmX);
        LPalmX = DefaultLPalmX;
        OldLPalmX = DefaultLPalmX;

        _target.SetFloat("LeapX", DefaultLPalmX);
        LPalmX = DefaultLPalmX;
        OldLPalmX = DefaultLPalmX;


        _target.SetFloat("LeapY", DefaultLPalmY);
        LPalmY = DefaultLPalmY;
        OldLPalmY = DefaultLPalmY;
    }

}
