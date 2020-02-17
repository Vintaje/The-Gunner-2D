using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Animator animator;


    //Parametros del jugador
    public int vidas;
    public float speed;
    public float speedagachado;
    public float speednormal;


    //Armamento
    private float fireRate = 0.5f; //Velocidad de disparo
    private float nextFire; //Siguiente disparo
    private int municionspec; // Municion especial
    private int municionextr; // Municion explosiva
    public float normalRate;
    public float specRate;
    public float exploRate;


    //Disparos
    public GameObject bulletPrefab;
    public GameObject bulletSpecialPrefab;
    public GameObject bulletExploPrefab;
    public Transform shotSpawner;
    private int weapon;//0 normal; 1 special; 2 extreme


    //Boolean animaciones
    private bool saltando;
    private bool agachado;
    private bool arriba;
    private bool derecha;
    private bool muerto;
    private bool running;

    //Controles
    protected Joystick joystick;
    public JumpJoybutton jump;
    public FireJoybutton fire;
    public float deadzone;


    //Sonidos
    public AudioSource death;
    public AudioSource footstep;
    public AudioSource jumping;
    public AudioSource ammo;
    public AudioSource pickup;
    public AudioSource no_ammo;
    public AudioSource change_weapon;
    public AudioSource[] sounds;
    //Sonidos Disparo
    public AudioSource normalweapon;
    public AudioSource specweapon;
    public AudioSource extraweapon;

    //UI
    public GameObject ammospectext;
    public GameObject ammoextratext;
    public GameObject ghost;



    //Efectos
    public GameObject text;

    //Condiciones start;
    private bool firstStart;



    // Start is called before the first frame update
    void Start()
    {
        weapon = 0;
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
        ammo = sounds[3];
        pickup = sounds[4];
        no_ammo = sounds[5];
        change_weapon = sounds[6];
        normalweapon = sounds[7];
        specweapon = sounds[8];
        extraweapon = sounds[9];


        //Buscamos los controles
        joystick = FindObjectOfType<Joystick>();

        shotSpawner.transform.localScale = (new Vector3(0.0f, 0.0f, 1.0f));
        animator.SetBool("Running", false);
        animator.SetBool("Weapon 1", true);
        animator.SetBool("Weapon 2", false);
        animator.SetBool("Weapon 3", false);

        //UI
        ammospectext.GetComponent<Text>().text = "0";
        ammoextratext.GetComponent<Text>().text = "0";
        StartCoroutine(NoAmmoBlink(0.1f));
        firstStart = false;
    }

    //Update
    void Update()
    {
        
        if (!muerto)
        {

            if (Input.GetButtonDown("Jump"))
            {
                if (weapon == 2)
                {
                    weapon = 0;
                    fireRate = normalRate;
                    animator.SetBool("Weapon 1", true);
                    animator.SetBool("Weapon 2", false);
                    animator.SetBool("Weapon 3", false);
                }
                else if (weapon == 0)
                {
                    weapon = 1;
                    fireRate = specRate;
                    animator.SetBool("Weapon 1", false);
                    animator.SetBool("Weapon 2", true);
                    animator.SetBool("Weapon 3", false);
                }
                else if (weapon == 1)
                {
                    weapon = 2;
                    fireRate = exploRate;
                    animator.SetBool("Weapon 1", false);
                    animator.SetBool("Weapon 2", false);
                    animator.SetBool("Weapon 3", true);
                }
                change_weapon.Play();
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (weapon == 0)
        {
            fireRate = normalRate;
            animator.SetBool("Weapon 1", true);
            animator.SetBool("Weapon 2", false);
            animator.SetBool("Weapon 3", false);
        }
        else if (weapon == 1)
        {

            fireRate = specRate;
            animator.SetBool("Weapon 1", false);
            animator.SetBool("Weapon 2", true);
            animator.SetBool("Weapon 3", false);
        }
        else if (weapon == 2)
        {
            fireRate = exploRate;
            animator.SetBool("Weapon 1", false);
            animator.SetBool("Weapon 2", false);
            animator.SetBool("Weapon 3", true);
        }
        ammospectext.GetComponent<Text>().text = municionspec + "";
        ammoextratext.GetComponent<Text>().text = municionextr + "";
        if (!muerto)
        {
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
            if (arriba)
            {
                speed = 0;
            }

            if (Application.platform == RuntimePlatform.Android)
            {

                if ((jump.Pressed && jump.gameObject.tag == "JumpButton"))
                {
                    saltar();

                }

                if ((fire.Pressed && fire.gameObject.tag == "FireButton") && Time.time > nextFire)
                {
                    disparar();
                }
                else
                {

                    shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
                }
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.Space) || (jump.Pressed && jump.gameObject.tag == "JumpButton") || Input.GetButtonDown("Fire1"))
                {

                    saltar();

                }

                if (Input.GetButton("Fire2") && Time.time > nextFire)
                {
                    disparar();
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
                    if (running)
                    {
                        running = false;
                    }
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
            }
            else
            {
                footstep.Pause();
            }




        }
    }



    private void saltar()
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


    void disparar()
    {
        if (weapon == 0)
        {
            normalweapon.Play();
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
            shotSpawner.transform.localScale = (new Vector3(1.5f, 1.5f, 1.0f));
            if (!derecha)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                if (arriba)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
        }
        if (weapon == 1 && municionspec > 0)
        {
            specweapon.Play();
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletSpecialPrefab, shotSpawner.position, shotSpawner.rotation);
            shotSpawner.transform.localScale = (new Vector3(1.5f, 1.5f, 1.0f));
            if (!derecha)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                if (arriba)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
            municionspec--;
            if (derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-50.0f, 50.0f, 0.0f));
            }
            else if (!derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(50.0f, 50.0f, 0.0f));
            }
            else if (arriba && saltando)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, -50.0f, 0.0f));
            }
        }
        else if (weapon == 2 && municionextr > 0)
        {
            extraweapon.Play();
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletExploPrefab, shotSpawner.position, shotSpawner.rotation);
            shotSpawner.transform.localScale = (new Vector3(1.5f, 1.5f, 1.0f));
            if (!derecha)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                if (arriba)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
            municionextr--;
            if (derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-100.0f, 50.0f, 0.0f));
            }
            else if (!derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(100.0f, 50.0f, 0.0f));
            }
            else if (arriba && saltando)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, -100.0f, 0.0f));
            }
        }
        if (weapon == 1 && municionspec == 0)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(NoAmmoBlink(0.5f));
            GameObject tempText = Instantiate(text, (new Vector3(0.0f, 0.2f, 0.0f)) + gameObject.transform.position, gameObject.transform.rotation);
            tempText.gameObject.GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
            tempText.GetComponent<TextMesh>().text = "OUT OF AMMO!";
        }
        if (weapon == 2 && municionextr == 0)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(NoAmmoBlink(0.5f));
            GameObject tempText = Instantiate(text, (new Vector3(0.0f, 0.2f, 0.0f)) + gameObject.transform.position, gameObject.transform.rotation);
            tempText.gameObject.GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
            tempText.GetComponent<TextMesh>().text = "OUT OF AMMO!";
        }


    }

    void OnCollisionEnter2D(Collision2D _col)
    {

        if (_col.gameObject.tag.Equals("Floor"))
        {
            gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 421;
            saltando = false;
            animator.SetBool("Jumping", saltando);
        }else{
            gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0;
        }


        if (_col.gameObject.tag.Equals("Explosion"))
        {
            
            if (derecha)
            {   
                gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 421;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-20.0f, 100.0f));
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 421;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(20.0f, 100.0f));
            }
            playermuerto();
        }

        

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Floor"))
        {
            saltando = true;
            animator.SetBool("Jumping", true);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("AmmoCrate"))
        {
            GameObject tempText = Instantiate(text, (new Vector3(0.0f, 0.2f, 0.0f)) + collider.gameObject.transform.position, collider.gameObject.transform.rotation);
            tempText.gameObject.GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
            if (collider.gameObject.GetComponent<AmmoCrate>().weapon == 1)
            {

                municionspec += collider.gameObject.GetComponent<AmmoCrate>().cantidad;
                tempText.GetComponent<TextMesh>().text = " + " + collider.gameObject.GetComponent<AmmoCrate>().cantidad + "\n TYPE " + " SPECIAL";
            }
            else if (collider.gameObject.GetComponent<AmmoCrate>().weapon == 2)
            {

                municionextr += collider.gameObject.GetComponent<AmmoCrate>().cantidad;
                tempText.GetComponent<TextMesh>().text = " + " + collider.gameObject.GetComponent<AmmoCrate>().cantidad + "\n TYPE " + " EXPLOSIVE";

            }
            ammo.Play();
            pickup.Play();
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject, 0.9f);
        }
    }



    public void playermuerto()
    {
        if (!muerto)
        {
            GameObject temp_ghost = Instantiate(ghost, (new Vector3(0.0f, 0.1f, 0.0f)) + gameObject.transform.position, gameObject.transform.rotation); muerto = true;
            footstep.Pause();
            BlinkPlayer(3);
            death.time = 0.65f;
            death.Play();
            animator.SetBool("Death", true);
            Invoke("restart", 2);
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

    IEnumerator NoAmmoBlink(float seconds)
    {
        no_ammo.UnPause();
        //wait for a bit
        yield return new WaitForSeconds(seconds);
        //make sure renderer is enabled when we exit
        no_ammo.Pause();
    }



    void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
