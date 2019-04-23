using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapTracking : MonoBehaviour
{
    public Transform HandPosition;
    public Vector3 LHandVect;
    public Vector3 RHandVect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        LHandVect = HandPosition.position;
    }
}
