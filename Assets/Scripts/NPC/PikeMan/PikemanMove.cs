using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikemanMove : MonoBehaviour
{
    public float visionRadius;
    public float speed;
    private AudioSource attack;
    private GameObject player;
    private float oldPosition = 0.0f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPosition = transform.position.x;
        attack = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        float fixedSpeed = speed * Time.deltaTime;

        if (dist < visionRadius)
        {
            visionRadius = 3.5f;
            if (dist < 0.60f)
            {

                animator.SetBool("attack", true);
                if (dist > 0.50f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
                }
            }
            else
            {

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);

                animator.SetBool("attack", false);


                animator.SetBool("isPlayerVisible", true);
                Debug.DrawLine(transform.position, player.transform.position, Color.red);

            }

        }
        else
        {
            animator.SetBool("isPlayerVisible", false);

        }


        if (transform.position.x - 0.01f > oldPosition)
        {
            transform.localScale = new Vector3(0.35f, 0.35f, 2);

        }

        if (transform.position.x + 0.01f < oldPosition)
        {
            transform.localScale = new Vector3(-0.35f, 0.35f, 2);
        }
        oldPosition = transform.position.x;

    }
    public void ResetAttack()
    {
    }

    public void AttackSound()
    {
        attack.Play();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
