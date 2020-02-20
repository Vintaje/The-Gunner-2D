using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
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

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
            Debug.DrawLine(transform.position, player.transform.position, Color.green);

        }



        if (transform.position.x - 0.01f > oldPosition)
        {
            transform.localScale = new Vector3(1, 1, 2);

        }

        if (transform.position.x + 0.01f < oldPosition)
        {
            transform.localScale = new Vector3(-1, 1, 2);
        }
        oldPosition = transform.position.x;

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
