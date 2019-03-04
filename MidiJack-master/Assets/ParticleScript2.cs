using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript2 : MonoBehaviour
{
    public ParticleSystem theParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;

        //main.startDelay = 5.0f;
        main.startLifetime = .3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
          //  main.startLifetime = 2.0f;

        }
    }
}
