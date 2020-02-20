using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Human : MonoBehaviour
{
    // Start is called before the first frame update
    public int vida;
    public bool muerto;
    public float hurt_duration;
    private Bullet bullet;
    public GameObject ghost;

    public bool destroyed;

    public float destroyTime = 1.5f;
    //Effects

    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (vida <= 0 && !muerto)
        {
            muerto = true;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
            if (!destroyed && !gameObject.tag.Equals("Player"))
            {
                destroyed = true;
                Destroy(gameObject, destroyTime);
            }

            if (!destroyed && gameObject.tag.Equals("Player"))
            {
                destroyed = true;
                gameObject.GetComponent<Player>().playermuerto();

            }
            else
            {
                GetComponent<Animator>().SetBool("Death", muerto);
                GameObject temp_ghost = Instantiate(ghost, gameObject.transform.position, gameObject.transform.rotation);
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet" && this.vida > 0)
        {
            bullet = other.gameObject.GetComponent<Bullet>();
            if ((bullet.enemy && gameObject.tag.Equals("Player") )|| (!bullet.enemy && gameObject.tag.Equals("EnemyBullet")))
            {
                BlinkPlayer(2);

                //Damage received
                vida -= bullet.damage;
                Debug.Log("Item " + gameObject.name + " damage received: " + bullet.damage + " || Vidas Restantes: " + vida);
            }
        }
        if (other.gameObject.tag == "Explosion" && this.vida > 0)
        {
            this.vida = 0;
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

    }



}
