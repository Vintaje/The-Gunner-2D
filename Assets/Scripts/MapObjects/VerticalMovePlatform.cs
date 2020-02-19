using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float speedUpDown = -1;
    private float pos ;
    public float distanceUpDown = 2;
    private Vector3 mov;
    public GameObject player;

    private bool playerAbove = false;
   


    private void Start()
    {pos=gameObject.transform.position.y;
    }
    void Update()
    {

        mov = new Vector3(transform.position.x , Mathf.Sin(speedUpDown * Time.time) * distanceUpDown+pos, transform.position.z);
        transform.position = mov;

        if (playerAbove)
        {
            player.transform.position = new Vector3(player.transform.position.x, mov.y,player.transform.position.z);
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerAbove = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerAbove = true;
        }
    }
}
