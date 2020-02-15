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
    private bool agachado;
    private bool arriba;
    public bool derecha;
    public bool muerto;
    public bool running;

    //Controles
    protected Joystick joystick;
    public JumpJoybutton jump;
    public FireJoybutton fire;
    public float deadzone;


    //Sonidos
    public AudioSource death;
    public AudioSource footstep;
    public AudioSource jumping;
    public AudioSource[] sounds;



    // Start is called before the first frame update
    void Start()
    {
        vidas = 3;
        saltando = false;
        agachado = false;
        derecha = true;
        arriba = false;
        muerto = false;
        running = true;

        //sonidos
        sounds = sounds = GetComponents<AudioSource>();
        death = sounds[0];
        footstep = sounds[1];
        jumping = sounds[2];

        //Buscamos los controles
        joystick = FindObjectOfType<Joystick>();

shotSpawner.transform.localScale = (new Vector3(0.0f, 0.0f, 1.0f));
        animator.SetBool("Running", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!muerto)
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
                running = true;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || joystick.Horizontal > deadzone || Input.GetAxis("Horizontal") > deadzone)
            {

                transform.Translate(new Vector3(speed, 0.0f));

                transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
                animator.SetBool("Running", true);
                derecha = true;
                running = true;

            }
            else
            {
                animator.SetBool("Running", false);
                running = false;
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
                        jumping.Play();
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
                    shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
                }


                if (Input.GetKey(KeyCode.W) || joystick.Vertical > deadzone - 0.5 || Input.GetAxis("Vertical") > deadzone - 0.5)
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
            if (running && !saltando)
            {
                footstep.UnPause();
            }else{
                footstep.Pause();
            }
        }
    }

    void Update(){}


    void OnCollisionEnter2D(Collision2D _col)
    {

        if (_col.gameObject.tag == "Floor")
        {
            saltando = false;
            animator.SetBool("Jumping", saltando);
        }

        if (_col.gameObject.tag == "DangerObject")
        {
            if (!muerto)
            {
                muerto = true;
                footstep.Pause();
                BlinkPlayer(3);
                death.time = 0.65f;
                death.Play();
                animator.SetBool("Death", true);
            }
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
