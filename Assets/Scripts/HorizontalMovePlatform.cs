using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovePlatform : MonoBehaviour
{
    private float speedUpDown = 1;
    private float distanceUpDown = 2;
    private Vector3 mov;
    public GameObject player;

    private bool playerAbove = false;
    public Vector3 Mov
    {
        get { return mov; }
        set { mov = value; }
    }


    private void Start()
    {
    }
    void Update()
    {
        mov = new Vector3(Mathf.Sin(speedUpDown * Time.time) * distanceUpDown + 8f, transform.position.y, transform.position.z);
        transform.position = mov;

        if (playerAbove)
        {
            player.transform.position = new Vector3(mov.x, player.transform.position.y,player.transform.position.z);
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
