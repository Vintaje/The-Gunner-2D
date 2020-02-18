using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLimit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.001f;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, 150.0f, 0.0f));
            other.gameObject.GetComponent<Player>().human.vida = 0;

        }
    }
}
