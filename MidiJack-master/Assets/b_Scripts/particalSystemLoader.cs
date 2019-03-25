using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particalSystemLoader : MonoBehaviour
{

    public GameObject[] particleSystems = new GameObject[10];
    public ParticleSystem currectParticleSystem;
    public int currentNum;
    public MidiParamController midiParamControllerScript;

   

    // Update is called once per frame
    void Update()
    {
        /*

        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 1.5f;
            var worldPos = camera.ScreenToWorldPoint(mousePos);
            Instantiate(blue, worldPos, Quaternion.identity);
        }
        */
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            NewParticleSystem();
        }
        if (Input.GetKeyDown("0"))
        {
            currentNum = 0;
        }
        if (Input.GetKeyDown("1"))
        {
            currentNum = 1;
        }
    }

    void NewParticleSystem()
    {
        Destroy(currectParticleSystem);

        GameObject newSys = Instantiate(particleSystems[currentNum], transform.position, Quaternion.identity, null) as GameObject;
        newSys.SetActive(true);
        currectParticleSystem = newSys.GetComponent<ParticleSystem>();
        //currentNum++;
        midiParamControllerScript.midiParticleSystem = currectParticleSystem;
    }
    void NewParticleSystemAtLoc()
    {
        Destroy(currectParticleSystem);
        GameObject newSys = Instantiate(particleSystems[currentNum], transform.position, Quaternion.identity, null) as GameObject;
        newSys.SetActive(true);
        currectParticleSystem = newSys.GetComponent<ParticleSystem>();


        //currentNum++;
        midiParamControllerScript.midiParticleSystem = currectParticleSystem;
    }
    //kill old system currently running
    //instantiate system from array

}
