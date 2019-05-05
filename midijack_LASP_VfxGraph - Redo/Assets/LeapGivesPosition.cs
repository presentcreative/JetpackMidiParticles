using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapGivesPosition : MonoBehaviour
{
    public LeapToVFX LeapToVFXScript;
    // Start is called before the first frame update
    public float HandX;
    public float HandY;
    public float HandZ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(this.gameObject.transform.position);
        LeapToVFXScript.LPalmX = this.gameObject.transform.position.x;
        HandX = this.transform.position.x;
        LeapToVFXScript.LPalmY = this.gameObject.transform.position.y;
        HandY = this.transform.position.y;
        LeapToVFXScript.LPalmZ = this.gameObject.transform.position.z;
        HandZ = this.transform.position.z;
    }
}
