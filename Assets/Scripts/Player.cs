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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-0.03f, 0.0f));

            transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
            animator.SetBool("Running", true);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(0.03f, 0.0f));
            transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            if (saltando == false)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                saltando = true;
                animator.SetBool("Running", false);
                animator.SetBool("Jumping", saltando);
            }
        }


        if(Input.GetKey(KeyCode.Space)){
            animator.SetBool("Death", true);
        }

        if(Input.GetKey(KeyCode.F) && Time.time > nextFire){
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner);
        }


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
