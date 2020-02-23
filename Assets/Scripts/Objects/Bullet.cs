using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public int damage = 1;
    public float destroyTime = 0.1f;
    public GameObject bulletExplo;
    public GameObject stream;

    private float oldPosition = 0.0f;
    private bool right = false;

    public bool enemy = false;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("disable", destroyTime);
        oldPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {

        if (speed != 0)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (stream != null)
            {
                if (right)
                {
                    GameObject streamB = Instantiate(stream, gameObject.transform.position + (new Vector3(1.0f, 0.0f, 0.0f)), gameObject.transform.rotation);
                }
                else
                {
                    GameObject streamB = Instantiate(stream, gameObject.transform.position - (new Vector3(1.0f, 0.0f, 0.0f)), gameObject.transform.rotation);
                }
            }


            oldPosition = transform.position.x;
        }

    }



    private void OnCollisionEnter2D(Collision2D other)
    {



        if (bulletExplo != null)
        {
            GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
        }

        Destroy(gameObject);

        //
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enemy)
        {
            if (other.gameObject.tag.Equals("EnemyBullet") || other.gameObject.tag.Equals("Player"))
            {
                if (bulletExplo != null)
                {
                    GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                if (bulletExplo != null)
                {
                    GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag.Equals("Floor") || other.gameObject.tag.Equals("Wall"))
            {
                if (bulletExplo != null)
                {
                    GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(gameObject);
            }


    }

    public void disable()
    {
        if (bulletExplo != null)
        {
            GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
        }
        Destroy(gameObject);
    }


}
