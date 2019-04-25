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

    public VisualEffect _target = null;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _target.SetFloat("LeapX", LPalmX);
        _target.SetFloat("LeapY", LPalmY);
        _target.SetFloat("LeapZ", LPalmZ);

    }
}
