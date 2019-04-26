using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureObjectLocation : MonoBehaviour
{
    public GameObject sphereObject;
    public float objLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetLocationX();
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            GetLocationX();
        }
    }

    void GetLocationX() {
        objLoc = sphereObject.transform.position.x;
    }
}
