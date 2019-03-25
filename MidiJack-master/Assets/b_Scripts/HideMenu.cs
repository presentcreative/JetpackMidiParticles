using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenu : MonoBehaviour
{

    public GameObject menu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideMenuScript()
    {
        if (menu.active == true)
        {
            menu.SetActive(false);
            
            
        }
        else
        {
            menu.SetActive(true);
            
        }
        
    }
}
