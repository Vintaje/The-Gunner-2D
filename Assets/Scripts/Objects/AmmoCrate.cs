using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    public int weapon;
    public int cantidad;


    // Start is called before the first frame update
    void Start()
    {
        float rand = Random.Range(0.0f, 100.0f);
        if (weapon == 0)
        {
            float randw = Random.Range(1.0f, 2.0f);
            if (randw > 1.7f)
            {
                weapon = 2;
            }
            else
            {
                weapon = 1;
            }
        }
        if (rand < 5.0f)
        {
            if (weapon == 1)
            {
                cantidad = 30;
            }
            else if (weapon == 2)
            {
                cantidad = 9;
            }
        }

        if (rand > 5.0f && rand < 30.0f)
        {
            if (weapon == 1)
            {
                cantidad = 20;
            }
            else if (weapon == 2)
            {
                cantidad = 6;
            }
        }
        if (rand > 30.0f)
        {
            if (weapon == 1)
            {
                cantidad = 10;
            }
            else if (weapon == 2)
            {
                cantidad = 3;
            }
        }
        Debug.Log("Weapon " + weapon);

        Debug.Log("Cantidad " + cantidad);
        if (weapon == 2)
        {
            GetComponent<Animator>().SetBool("Weapon 2", true);
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collider)
    {



    }
}
