﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    void Start()
    {
        if (!gameObject.name.Equals("AttackCollider"))
        {
            Destroy(gameObject, 0.4f);
        }
        try
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        catch (UnityException) { 

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
