using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particalSystemLoader : MonoBehaviour
{

    public GameObject[] particleSystems = new GameObject[10];
    public ParticleSystem currectParticleSystem;
    public GameObject currentParticleSysGameObject;
    public int currentNum;
    public MidiParamController midiParamControllerScript;

    public Vector3 nextLoc;
    public Quaternion nextQuat;
    public Vector3 storedLoc1;
    public Quaternion storedQuat1;
    public Vector3 storedLoc2;
    public Quaternion storedQuat2;

    public Quaternion currentQuat;



    // Update is called once per frame
    void Update()
    {
        
        
        
        if (Input.GetKeyDown("n"))
        {
            
            //NewParticleSystem();
            ReplaceParticleSystem();
        }
        if (Input.GetKeyDown("q"))
        {
            
            nextQuat = storedQuat1;
            nextLoc = storedLoc1;
            
        }
        if (Input.GetKeyDown("w"))
        {

            nextQuat = storedQuat2;
            nextLoc = storedLoc2;

        }
        if (Input.GetKeyDown("0"))
        {
            currentNum = 0;
        }
        if (Input.GetKeyDown("1"))
        {
            currentNum = 1;
        }
        if (Input.GetKeyDown("2"))
        {
            currentNum = 2;
        }
        if (Input.GetKeyDown("3"))
        {
            currentNum = 3;
        }
        if (Input.GetKeyDown("4"))
        {
            currentNum = 4;
        }
        if (Input.GetKeyDown("5"))
        {
            currentNum = 5;
        }
        if (Input.GetKeyDown("6"))
        {
            currentNum = 6;
        }
        if (Input.GetKeyDown("7"))
        {
            currentNum = 7;
        }
        if (Input.GetKeyDown("8"))
        {
            currentNum = 8;
        }
        if (Input.GetKeyDown("9"))
        {
            currentNum = 9;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var mousepos = Input.mousePosition;
            mousepos.z = 10;

            mousepos = Camera.main.ScreenToWorldPoint(mousepos);
            //linehandler = Instantiate(lineprefab, mousepos, Quaternion.identity) as GameObject;
            NewParticleSystemAtLoc(mousepos);
        }
    }

    void NewParticleSystem()
    {
        Destroy(currectParticleSystem);
        
        GameObject newSys = Instantiate(particleSystems[currentNum], nextLoc, nextQuat, null) as GameObject;
        newSys.SetActive(true);
        currectParticleSystem = newSys.GetComponent<ParticleSystem>();
        //currentNum++;
        midiParamControllerScript.midiParticleSystem = currectParticleSystem;
    }
    void ReplaceParticleSystem()
    {
        Destroy(currentParticleSysGameObject);
        currentParticleSysGameObject = Instantiate(particleSystems[currentNum], nextLoc, nextQuat, null) as GameObject;
        currentParticleSysGameObject.SetActive(true);
        currectParticleSystem = currentParticleSysGameObject.GetComponent<ParticleSystem>();
        midiParamControllerScript.midiParticleSystem = currectParticleSystem;
    }

    void NewParticleSystemAtLoc(Vector3 loc)
    {
        Destroy(currectParticleSystem);
        GameObject newSys = Instantiate(particleSystems[currentNum], loc, Quaternion.identity, null) as GameObject;
        newSys.SetActive(true);
        currectParticleSystem = newSys.GetComponent<ParticleSystem>();
        //currentNum++;
        midiParamControllerScript.midiParticleSystem = currectParticleSystem;
    }
    //kill old system currently running
    //instantiate system from array

}
