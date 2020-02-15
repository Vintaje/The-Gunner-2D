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



    private void Start()
    {
    }
    void Update()
    {

        if (Mathf.Sin(speedUpDown * Time.time) > 0.98)
        {
            gameObject.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

        }
        else if (Mathf.Sin(speedUpDown * Time.time) < -0.98)
        {
            gameObject.transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);

        }
        mov = new Vector3(Mathf.Sin(speedUpDown * Time.time) * distanceUpDown + 8f, transform.position.y, transform.position.z);
        transform.position = mov;

       /*  if (playerAbove)
        {
            player.transform.position = new Vector3(mov.x, player.transform.position.y, player.transform.position.z);
        } */

    }

/*     private void OnCollisionExit2D(Collision2D other)
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
 */

}
