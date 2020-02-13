using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /*     public GameObject ataque_original;
        public GameObject ataque_posicion;
     */
    private bool saltando;
    private bool atacando;
    private bool agachado;
    private bool arriba;
    public bool derecha;
    public Animator animator;
    public int vidas;
    public float fireRate = 0.5f;
    public float nextFire;

    public GameObject bulletPrefab;
    public Transform shotSpawner;
    // Start is called before the first frame update
    void Start()
    {
        vidas = 3;
        saltando = false;
        atacando = false;
        agachado = false;
        derecha = false;
        arriba = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!agachado)
            {
                transform.Translate(new Vector3(-0.03f, 0.0f));

                transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
                animator.SetBool("Running", true);
                derecha = false;
            }else{
                transform.Translate(new Vector3(-0.005f, 0.0f));

                transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
                animator.SetBool("Running", true);
                derecha = false;
            }

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!agachado)
            {
                transform.Translate(new Vector3(0.03f, 0.0f));

                transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
                animator.SetBool("Running", true);
                derecha = true;
            }else{
                transform.Translate(new Vector3(0.005f, 0.0f));

                transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
                animator.SetBool("Running", true);
                derecha = true;
            }
        }
        else
        {
            animator.SetBool("Running", false);
        }



        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            if (saltando == false && !agachado)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                saltando = true;
                animator.SetBool("Running", false);
                animator.SetBool("Jumping", saltando);
            }
            else if (agachado)
            {
                arriba = true;
                animator.SetBool("AimUp", arriba);
            }
        }
        else
        {
            arriba = false;
            animator.SetBool("AimUp", arriba);

        }


        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<AudioSource>().time = 0.65f;
            gameObject.GetComponent<AudioSource>().Play();
            animator.SetBool("Death", true);
           
        }

        if (Input.GetKey(KeyCode.F) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
            shotSpawner.transform.localScale = (new Vector3(1.5f, 1.5f, 1.0f));
            if (!derecha)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                if (saltando)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 270);
                }
                if (arriba)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
        }
        else
        {
            shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!saltando)
            {
                agachado = true;

            }
        }
        else
        {
            agachado = false;
        }
        animator.SetBool("Crouch", agachado);


    }


    void OnCollisionEnter2D(Collision2D _col)
    {

        if (_col.gameObject.tag == "Floor")
        {
            saltando = false;
            animator.SetBool("Jumping", saltando);
        }
    }


}
