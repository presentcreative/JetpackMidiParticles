using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public int testInt;
    public MidiMapping midiMappingScript;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

            if (Input.GetKeyDown(KeyCode.S))
        { 
            //PlayerPrefs.SetInt("testPrefs",testInt);
            //Debug.Log("playprefs set" + testInt);
            SaveArray(midiMappingScript.particleParams);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // testInt = PlayerPrefs.GetInt("testPrefs");
            // Debug.Log("playprefs get" + PlayerPrefs.GetInt("testPrefs"));
            LoadArray();
        }
    }
    public void SaveArray(int[] midiMapArrayToSave)
    { 
        //public int[] puntaje;
        for(int i=0;i< midiMapArrayToSave.Length;i++)
            {
                 PlayerPrefs.SetInt("midiMapArrayToSave" + i, midiMapArrayToSave[i]);
            }
        for(int i=0;i< midiMapArrayToSave.Length;i++)
            {
                print(PlayerPrefs.GetInt("midiMapArrayToSave" + i));
            }
    }
    public void LoadArray()
    {
        for (int i = 0; i < midiMappingScript.particleParams.Length; i++)
        {
            midiMappingScript.particleParams[i] = PlayerPrefs.GetInt("midiMapArrayToSave" + i);
            //print(PlayerPrefs.GetInt("arrayToSave" + i));
        }
    }
    

}
