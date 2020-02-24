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
    public float speed;
    public float speedagachado;
    public float speednormal;


    //Armamento
    private float fireRate; //Velocidad de disparo
    private float nextFire; //Siguiente disparo
    public int municionspec; // Municion especial
    public int municionextr; // Municion explosiva
    public float normalRate;
    public float specRate;
    public float exploRate;
    public float changewepDelay;
    private float nextWeapon;


    //Disparos
    public GameObject bulletPrefab;
    public GameObject bulletSpecialPrefab;
    public GameObject bulletExploPrefab;
    public Transform shotSpawner;
    private int weapon;//0 normal; 1 special; 2 extreme
    private bool wep1;
    private bool wep2;
    private bool wep3;


    //Boolean animaciones
    private bool saltando;
    private bool agachado;
    private bool arriba;
    private bool derecha;
    private bool running;


    //Controles
    public float deadzone;
    protected Joystick joystick;
    public Joybutton fireButton;
    public Joybutton jumpButton;
    public Joybutton switchButton;


    //Sonidos
    private AudioSource death;
    private AudioSource footstep;
    private AudioSource jumping;
    private AudioSource ammo;
    private AudioSource pickup;
    private AudioSource no_ammo;
    private AudioSource change_weapon;
    private AudioSource[] sounds;
    //Sonidos Disparo
    private AudioSource normalweapon;
    private AudioSource specweapon;
    private AudioSource extraweapon;

    //UI
    public GameObject ammospectext;
    public GameObject ammoextratext;
    public GameObject ghost;
    public GameObject life;

    //Efectos
    public GameObject text;


    //Script general
    public Human human;

    private int plataforma; //1 PC //3 Android



    // Start is called before the first frame update
    void Start()
    {
        //Script principal
        human = GetComponent<Human>();



        //Plataforma
        if (Application.platform == RuntimePlatform.Android)
        {
            plataforma = 3;

            joystick = FindObjectOfType<Joystick>();

        }
        else
        {
            plataforma = 2;
        }




        weapon = 0;
        saltando = false;
        derecha = true;


        //sonidos
        sounds = GetComponents<AudioSource>();
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

        //Animaciones iniciales
        shotSpawner.transform.localScale = (new Vector3(0.0f, 0.0f, 1.0f));
        wep1 = true;
        wep2 = false;
        wep3 = false;
        animator.SetBool("Running", false);
        animator.SetBool("Weapon 1", wep1);
        animator.SetBool("Weapon 2", wep2);
        animator.SetBool("Weapon 3", wep3);

        //UI

        StartCoroutine(NoAmmoBlink(0.05f));
        fireRate = normalRate;

    }

    //Update
    void Update()
    {
        life.GetComponent<HealthBar>().SetHealth(human.vida);
        if (plataforma == 2)
        {
            pcPlatformGamePad();
        }
        else if (plataforma == 3)
        {
            androidPlatform();
        }

    }

    void androidPlatform()
    {
        ammospectext.GetComponent<Text>().text = municionspec + "";
        ammoextratext.GetComponent<Text>().text = municionextr + "";
        if (!human.muerto)
        {

            if (joystick.Vertical == 0)
            {
                running = false;
                agachado = false;
                arriba = false;
                speed = speednormal;
            }

            if (joystick.Horizontal < (deadzone / 2) * -1 && !arriba && !agachado)
            {
                transform.Translate(new Vector3(speed * -1, 0.0f));
                transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
                derecha = false;
                running = true;
            }
            if (joystick.Horizontal > (deadzone / 2) && !arriba && !agachado)
            {
                transform.Translate(new Vector3(speed, 0.0f));
                transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
                derecha = true;
                running = true;
            }
            if ((joystick.Horizontal < (deadzone / 2) && joystick.Horizontal > deadzone * -1))
            {
                running = false;
            }

            if (running && !saltando && !arriba)
            {
                footstep.UnPause();
            }
            else
            {
                footstep.Pause();
            }



            if (switchButton.Pressed && Time.time > nextWeapon)
            {
                nextWeapon = Time.time + changewepDelay;
                if (weapon == 2)
                {
                    weapon = 0;
                    wep1 = true;
                    wep2 = false;
                    wep3 = false;
                    fireRate = normalRate;
                }
                else if (weapon == 0)
                {
                    weapon = 1;
                    fireRate = specRate;
                    wep1 = false;
                    wep2 = true;
                    wep3 = false;
                }
                else if (weapon == 1)
                {
                    weapon = 2;
                    fireRate = exploRate;
                    wep1 = false;
                    wep2 = false;
                    wep3 = true;
                }
                change_weapon.Play();
            }

            if (joystick.Vertical < (deadzone / 2) * -1 && !saltando)
            {
                agachado = true;
                running = false;
                speed = speedagachado;
            }


            if (jumpButton.Pressed)
            {
                saltar();
            }

            if (fireButton.Pressed && Time.time > nextFire)
            {
                disparar();
            }
            else
            {
                shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
            }


            if (joystick.Vertical > (deadzone / 2))
            {
                arriba = true;
                running = false;
                speed = 0;
            }


            animator.SetBool("Running", running);
            animator.SetBool("Crouch", agachado);
            animator.SetBool("AimUp", arriba);
            animator.SetBool("Weapon 1", wep1);
            animator.SetBool("Weapon 2", wep2);
            animator.SetBool("Weapon 3", wep3);

        }
    }

    void pcPlatformGamePad()
    {
        ammospectext.GetComponent<Text>().text = municionspec + "";
        ammoextratext.GetComponent<Text>().text = municionextr + "";
        if (!human.muerto)
        {

            if (Input.GetAxisRaw("Vertical") == 0)
            {
                running = false;
                agachado = false;
                arriba = false;
                speed = speednormal;
            }

            if (Input.GetAxisRaw("Horizontal") < deadzone * -1 && !arriba && !agachado)
            {
                transform.Translate(new Vector3(speed * -1, 0.0f));
                transform.localScale = (new Vector3(-1.0f, 1.0f, 1.0f));
                derecha = false;
                running = true;
            }
            if (Input.GetAxisRaw("Horizontal") > deadzone && !arriba && !agachado)
            {
                transform.Translate(new Vector3(speed, 0.0f));
                transform.localScale = (new Vector3(1.0f, 1.0f, 1.0f));
                derecha = true;
                running = true;
            }
            if ((Input.GetAxisRaw("Horizontal") < deadzone && Input.GetAxisRaw("Horizontal") > deadzone * -1))
            {
                running = false;
            }

            if (running && !saltando && !arriba)
            {
                footstep.UnPause();
            }
            else
            {
                footstep.Pause();
            }



            if (Input.GetButtonDown("Arma"))
            {
                if (weapon == 2)
                {
                    weapon = 0;
                    wep1 = true;
                    wep2 = false;
                    wep3 = false;
                    fireRate = normalRate;
                }
                else if (weapon == 0)
                {
                    weapon = 1;
                    fireRate = specRate;
                    wep1 = false;
                    wep2 = true;
                    wep3 = false;
                }
                else if (weapon == 1)
                {
                    weapon = 2;
                    fireRate = exploRate;
                    wep1 = false;
                    wep2 = false;
                    wep3 = true;
                }
                change_weapon.Play();
            }

            if (Input.GetAxisRaw("Vertical") < deadzone * -1 && !saltando)
            {
                agachado = true;
                running = false;
                speed = speedagachado;
            }
            else
            {
                agachado = false;
                speed = speednormal;
            }


            if (Input.GetButtonDown("Saltar"))
            {
                saltar();
            }

            if ((Input.GetButton("Disparar") && Time.time > nextFire))
            {
                disparar();
            }
            else
            {
                shotSpawner.transform.localScale = (new Vector3(0.0f, 1.0f, 1.0f));
            }


            if (Input.GetAxis("Vertical") > deadzone - 0.5)
            {
                arriba = true;
                running = false;
                speed = 0;
            }


            animator.SetBool("Running", running);
            animator.SetBool("Crouch", agachado);
            animator.SetBool("AimUp", arriba);
            animator.SetBool("Weapon 1", wep1);
            animator.SetBool("Weapon 2", wep2);
            animator.SetBool("Weapon 3", wep3);

        }
    }




    // Update is called once per frame
    void FixedUpdate()
    {

    }


    private void saltar()
    {
        if (!saltando && !agachado)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 175.0f));
            saltando = true;
            animator.SetBool("Running", running);
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
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-30.0f, 50.0f, 0.0f));
            }
            else if (!derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(30.0f, 50.0f, 0.0f));
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
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-80.0f, 50.0f, 0.0f));
            }
            else if (!derecha && !saltando && !arriba)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(80.0f, 50.0f, 0.0f));
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
        }

        if (_col.gameObject.tag.Equals("Wall"))
        {
            gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0;
        }



    }

    void OnCollisionExit2D(Collision2D _col)
    {
        if (_col.gameObject.tag.Equals("Wall"))
        {
            gameObject.GetComponent<Rigidbody2D>().sharedMaterial.friction = 421;
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
        if (collider.gameObject.tag.Equals("Medkit"))
        {
            GameObject tempText = Instantiate(text, (new Vector3(0.0f, 0.2f, 0.0f)) + collider.gameObject.transform.position, collider.gameObject.transform.rotation);
            tempText.gameObject.GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
            human.vida += collider.gameObject.GetComponent<Medkit>().vida;
            tempText.GetComponent<TextMesh>().text = " + " + collider.gameObject.GetComponent<Medkit>().vida + " HP";
            if (human.vida > 10)
            {
                human.vida = 10;
            }
            pickup.Play();
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject, 0.3f);
        }
        if (collider.gameObject.tag.Equals("Explosion"))
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

        }

    }



    public void playermuerto()
    {
        PlayerPrefs.SetInt("Vida", 10);
        PlayerPrefs.SetInt("Spec", 0);
        PlayerPrefs.SetInt("Extra", 0);
        GameObject temp_ghost = Instantiate(ghost, (new Vector3(0.0f, 0.1f, 0.0f)) + gameObject.transform.position, gameObject.transform.rotation);
        footstep.Pause();
        BlinkPlayer(3);
        death.time = 0.65f;
        death.Play();
        animator.SetBool("Death", true);
        Invoke("restart", 2);
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
