using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int vida;
    // Start is called before the first frame update
    void Start()
    {
        if(vida == 0){
            vida = (int)Random.Range(1.0f, 6.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
