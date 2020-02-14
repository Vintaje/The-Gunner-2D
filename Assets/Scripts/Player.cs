using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator animator;


    //Parametros del jugador
    public int vidas;
    public float fireRate = 0.5f;
    public float nextFire;
    public float speed;
    public float speedagachado;
    public float speednormal;


    //Disparos
    public GameObject bulletPrefab;
    public Transform shotSpawner;


    //Boolean animaciones
    private bool saltando;
    private bool atacando;
    private bool agachado;
    private bool arriba;
    public bool derecha;

    //Controles
    protected Joystick joystick;
    public JumpJoybutton jump;
    public FireJoybutton fire;
    public float deadzone;



    // Start is called before the first frame update
    void Start()
    {
        vidas = 3;
        saltando = false;
        atacando = false;
        agachado = false;
        derecha = true;
        arriba = false;


        //Buscamos los controles
        joystick = FindObjectOfType<Joystick>();


        animator.SetBool("Running", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (arriba)
        {
            speed = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || joystick.Horizontal < deadzone * -1 || Input.GetAxis("Horizontal") < deadzone * -1)
        {
            transform.Translate(new Vector3(speed * -1, 0.0f));

            transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
            animator.SetBool("Running", true);
            derecha = false;

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || joystick.Horizontal > deadzone || Input.GetAxis("Horizontal") > deadzone)
        {

            transform.Translate(new Vector3(speed, 0.0f));

            transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
            animator.SetBool("Running", true);
            derecha = true;


        }
        else
        {
            animator.SetBool("Running", false);
        }


        if (Application.platform == RuntimePlatform.Android)
        {

            if (Input.GetKey(KeyCode.Space) || (jump.Pressed && jump.gameObject.tag == "JumpButton"))
            {

                if (saltando == false && !agachado)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                    saltando = true;
                    animator.SetBool("Running", false);
                    animator.SetBool("Jumping", saltando);
                }

            }

            if ((fire.Pressed && fire.gameObject.tag == "FireButton") && Time.time > nextFire)
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
        }
        else
        {

            if (Input.GetKey(KeyCode.Space) || (jump.Pressed && jump.gameObject.tag == "JumpButton") || Input.GetButton("Jump"))
            {

                if (saltando == false && !agachado)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
                    saltando = true;
                    animator.SetBool("Running", false);
                    animator.SetBool("Jumping", saltando);
                }

            }

            if ((Input.GetKey(KeyCode.F) || Input.GetButton("Fire1")) && Time.time > nextFire)
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
            }


            if (Input.GetKey(KeyCode.W) || joystick.Vertical > deadzone-0.5 || Input.GetAxis("Vertical") > deadzone-0.5)
            {
                arriba = true;
                speed = 0;
            }
            else
            {
                arriba = false;
                speed = speednormal;
            }
            animator.SetBool("AimUp", arriba);


            if (Input.GetKey(KeyCode.L))
            {
                gameObject.GetComponent<AudioSource>().time = 0.65f;
                gameObject.GetComponent<AudioSource>().Play();
                animator.SetBool("Death", true);

            }


            shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
        }


        if (Input.GetAxis("Vertical") < deadzone * -1 || joystick.Vertical < deadzone * -1 || Input.GetKey(KeyCode.S))
        {
            if (!saltando)
            {
                agachado = true;
                speed = speedagachado;
            }
        }
        else
        {
            agachado = false;
            speed = speednormal;
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
