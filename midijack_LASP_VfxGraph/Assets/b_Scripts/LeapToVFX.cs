using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class LeapToVFX : MonoBehaviour
{
    public Transform LPalmTransform;
    public GameObject LeftPalm;
    public float LPalmX;
    public float LPalmY;
    public float LPalmZ;
    public float DefaultLPalmX;
    public float DefaultLPalmY;
    public float DefaultLPalmZ;
    public float OldLPalmX;
    public float OldLPalmY;
    public float OldLPalmZ;

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
        }
        else {
            _target.SetFloat("LeapX", DefaultLPalmX);
        }
        if (OldLPalmY != LPalmY)
        {
            _target.SetFloat("LeapY", LPalmY);
            OldLPalmY = LPalmY;
        }
        else {
            _target.SetFloat("LeapY", DefaultLPalmY);
        }

        if (OldLPalmZ != LPalmZ)
        {
            _target.SetFloat("LeapZ", LPalmZ);
            OldLPalmZ = LPalmZ;
        }
        else {
            _target.SetFloat("LeapZ", DefaultLPalmZ);
        }
    }
}
