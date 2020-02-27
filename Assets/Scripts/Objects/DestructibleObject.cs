using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int vida;
    public float hurt_duration;
    private Bullet bullet;



    //Effects
    public GameObject explosion;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vida <= 0)
        {
            GameObject.Instantiate(explosion, gameObject.transform.position+(new Vector3(0.0f, -50.0f, 0.0f)) , gameObject.transform.rotation);
            Destroy(gameObject);

        }
    }


    public void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.tag == "Bullet" && this.vida > 0)
        {
            bullet = other.gameObject.GetComponent<Bullet>();
            BlinkPlayer(2);

            //Damage received
            vida -= bullet.damage;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == "Bullet" && this.vida > 0)
        {
            bullet = other.gameObject.GetComponent<Bullet>();
            BlinkPlayer(2);

            //Damage received
            vida -= bullet.damage;
            bullet.disable();
        }
    }

    void BlinkPlayer(int numBlinks)
    {
        StartCoroutine(DoBlinks(numBlinks, 0.2f));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        for (int i = 0; i < numBlinks * 2; i++)
        {

            //toggle renderer
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
            //wait for a bit
            yield return new WaitForSeconds(seconds);
        }

        //make sure renderer is enabled when we exit
        gameObject.GetComponent<Renderer>().enabled = true;  
    }

}
