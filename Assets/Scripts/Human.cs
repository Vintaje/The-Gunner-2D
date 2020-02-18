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


    //Effects

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vida <= 0 && !muerto)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
            muerto = true;
            if (gameObject.tag.Equals("Player"))
            {
                gameObject.GetComponent<Player>().playermuerto();
            }
            else
            {
                GetComponent<Animator>().SetBool("Death", muerto);
                GameObject temp_ghost = Instantiate(ghost, (new Vector3(0.0f, 0.1f, 0.0f)) + gameObject.transform.position, gameObject.transform.rotation);
            }
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
            Debug.Log("Item " + gameObject.name + " damage received: " + bullet.damage+" || Vidas Restantes: "+vida);
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
