﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =1; 1<=20; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButton>().SetText("button #" + i);
            button.transform.SetParent(buttonTemplate.transform.parent, false);
            
        }
    }


}
