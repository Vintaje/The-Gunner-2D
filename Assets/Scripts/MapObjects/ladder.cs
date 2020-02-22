using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && Input.GetAxisRaw("Vertical") > 0)
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);

        }
        else if (other.tag.Equals("Player") && Input.GetAxisRaw("Vertical") < 0)
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
        }
        /*else if(other.tag.Equals("EnemyBullet"))
        {
            other.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 1.5f);
        }*/
    }
}
