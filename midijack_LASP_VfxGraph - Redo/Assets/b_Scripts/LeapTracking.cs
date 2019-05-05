using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class LeapTracking : MonoBehaviour
{
    Controller m_leapController;

    public Transform HandPosition;
    public GameObject LeapLeftHand;
    public Vector3 LHandX;
    public float RHandX;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        RHandX = LeapLeftHand.transform.position.x;
        
    }




}
