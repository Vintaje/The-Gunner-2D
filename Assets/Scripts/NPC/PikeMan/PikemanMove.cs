using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikemanMove : MonoBehaviour
{
    public float visionRadius;
    public float speed;
    private GameObject player;
    private float oldPosition = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        float fixedSpeed = speed * Time.deltaTime;

        if (dist < visionRadius)
        {
            if (dist < 0.40f)
            {
                gameObject.GetComponent<Animator>().SetBool("attack", true);

            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("attack", false);


                Debug.Log("dist:" + dist + " vision:" + visionRadius);
                gameObject.GetComponent<Animator>().SetBool("isPlayerVisible", true);



                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);

                Debug.DrawLine(transform.position, player.transform.position, Color.red);
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isPlayerVisible", false);

        }


        if (transform.position.x > oldPosition)
        {
            transform.localScale = new Vector3(0.33f, 0.33f, 1);

        }

        if (transform.position.x < oldPosition)
        {
            transform.localScale = new Vector3(-0.33f, 0.33f, 1);
        }
        oldPosition = transform.position.x;
    }

}
