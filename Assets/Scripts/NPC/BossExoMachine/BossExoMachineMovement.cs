using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExoMachineMovement : MonoBehaviour
{

    public float visionRadius;
    public float speed;
    private GameObject player;
    private float oldPosition = 0.0f;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {                animator.SetBool("Attacking", true);


        float dist = Vector3.Distance(player.transform.position, transform.position);
        float fixedSpeed = speed * Time.deltaTime;

        if (dist < visionRadius)
        {

            visionRadius = 3.5f;
            if (dist < 1f)
            {
                if (dist > 0.50f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
                }
            }
            else
            {
                animator.SetBool("Attacking", false);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
                Debug.Log("dist:" + dist + " vision:" + visionRadius);
                Debug.DrawLine(transform.position, player.transform.position, Color.red);

            }



            if (transform.position.x - 0.01f > oldPosition)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 2);

            }

            if (transform.position.x + 0.01f < oldPosition)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 2);
            }
            oldPosition = transform.position.x;



        }else{
                            animator.SetBool("IsPlayerVisible", false);

        }
    }
    public void ResetAttack()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
