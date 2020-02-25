using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossGunman : MonoBehaviour
{
    public float normalspeed;
    private float fastspeed;
    private float speed;
    public float distance;

    //Animaciones 
    private bool attack;
    private bool up, down, missiles;

    private bool right = true;
    private bool detected = false;
    public Transform target;

    public float fireRate;
    private float nextFire;
    private bool patron;

    public float patronRate;
    private float nextPatron;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    public Transform missileSpawner;
    public Transform patronSpawner;

    public GameObject hommingMissile;
    public GameObject grenade;
    public GameObject bullet;
    public Image fade;
    private float oldPosition = 0.0f;
    public float distanceToShot;
    private float distanceToTarget;


    public int numDisparos;
    public int numBombs;


    public GameObject bossDetect;

    //Script general
    Human human;

    private bool newPatron;
    private bool death;
    public float timeTillHit;


    //Animator
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        nextFire = 0.0f;
        oldPosition = transform.position.x;
        right = true;
        speed = normalspeed;
        fastspeed = 2f * normalspeed;
        human = GetComponent<Human>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        human.muerto = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!human.muerto && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New Animation 0"))
        {
            Vector3 boss = new Vector3(transform.position.x, 0.0f, 0.0f);
            Vector3 targetx = new Vector3(target.position.x, 0.0f, 0.0f);
            distanceToTarget = Vector3.Distance(boss, targetx);
            Debug.Log(distanceToTarget);

            if (!detected)
            {
                attack = false;

                GetComponent<Animator>().SetBool("Shooting", attack);
                GetComponent<Animator>().SetBool("Missiles", attack);

            }
            else if (detected)
            {


                if (Time.time > nextPatron && distanceToTarget < distanceToShot * 4 && Mathf.Abs(transform.position.y - target.position.y) < 0.6f)
                {
                    nextPatron = Time.time + patronRate;
                    patron = true;
                    StartCoroutine(patronAtaque());
                }


                if (transform.position.x < target.position.x && distanceToTarget < -0.5f) // he's looking right
                {
                    right = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (transform.position.x > target.position.x && distanceToTarget > 0.5f) // he's looking left
                {
                    right = false;
                    transform.eulerAngles = new Vector3(0, -180, 0);
                }


            }

            if (distanceToTarget > distance && Mathf.Abs(transform.position.y - target.position.y) < 0.6f)
            {
                detected = false;
                speed = normalspeed;
            }
            else if (!detected)
            {

                detected = true;
                speed = fastspeed;
            }


            oldPosition = transform.position.x;
            animator.SetBool("Shooting", attack);
            animator.SetBool("Up", up);
            animator.SetBool("Down", down);
            animator.SetBool("Missiles", missiles);
        }

        if (human.vida <= 15)
        {
            patronRate = 2;
        }
        if (human.muerto && !death)
        {
            death = true;
            GetComponents<AudioSource>()[0].Play();
        }

    }



    IEnumerator DoBlinks(float seconds)
    {

        yield return new WaitForSeconds(seconds);

        //make sure renderer is enabled when we exit


        GetComponent<Animator>().SetBool("Knife", false);

        yield return new WaitForSeconds(seconds);

    }
    IEnumerator BulletDelay(float seconds)
    {

        yield return new WaitForSeconds(seconds);

        GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
        if (!right)
        {
            tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
        }

    }

    IEnumerator patronAtaque()
    {
        if (human.vida >= 15)
        {
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    attack = true;
                    GameObject b = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
                    b.transform.Rotate(new Vector3(0, 0, i * 4 * -1));
                    down = false;
                    up = true;

                    yield return new WaitForSeconds(0.2f);


                }
                attack = false;
                up = false;
                animator.SetBool("Shooting", attack);
                animator.SetBool("Up", up);
                animator.SetBool("Down", down);

            }

            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    attack = true;
                    GameObject b = Instantiate(bullet, shotSpawner.position, shotSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
                    b.transform.Rotate(new Vector3(0, 0, i * 8 * -1));
                    down = false;
                    up = true;

                    yield return new WaitForSeconds(0.8f);


                }
                attack = false;
                up = false;
                down = false;
                animator.SetBool("Shooting", attack);
                animator.SetBool("Up", up);
                animator.SetBool("Down", down);
            }
            yield return new WaitForSeconds(1.2f);
            for (int i = 0; i < numBombs; i++)
            {
                if (!human.muerto)
                {
                    down = true;
                    up = false;
                    missiles = true;

                    yield return new WaitForSeconds(0.8f);


                    GameObject nade = Instantiate(hommingMissile, missileSpawner.position, missileSpawner.rotation);
                    nade.transform.Translate(new Vector3(0.0f, -0.05f, 0.0f));
                    Throw(nade);
                }


            }
            down = false;
            up = false;
            missiles = false;
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    attack = true;
                    GameObject b = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
                    b.transform.Rotate(new Vector3(0, 0, i * 4 * -1));
                    down = false;
                    up = true;

                    yield return new WaitForSeconds(0.2f);


                }
                attack = false;
                up = false;
                animator.SetBool("Shooting", attack);
                animator.SetBool("Up", up);
                animator.SetBool("Down", down);

            }

            down = false;
            missiles = false;
            animator.SetBool("Missiles", missiles);
            animator.SetBool("Up", up);
            animator.SetBool("Down", down);
            yield return new WaitForSeconds(1.0f);
        }
        else
        {

            for (int i = 0; i < numDisparos * 2; i++)
            {
                if (!human.muerto)
                {
                    attack = true;
                    GameObject b = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
                    b.transform.Rotate(new Vector3(0, 0, i * 4 * -1));
                    down = false;
                    up = true;

                    yield return new WaitForSeconds(0.2f);


                }
                attack = false;
                up = false;
                animator.SetBool("Shooting", attack);
                animator.SetBool("Up", up);
                animator.SetBool("Down", down);

            }
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    attack = true;
                    GameObject b = Instantiate(bullet, shotSpawner.position, shotSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
                    b.transform.Rotate(new Vector3(0, 0, i * 8 * -1));
                    down = false;
                    up = true;

                    yield return new WaitForSeconds(0.5f);


                }
                attack = false;
                up = false;
                down = false;
                animator.SetBool("Shooting", attack);
                animator.SetBool("Up", up);
                animator.SetBool("Down", down);
            }
        }


        patron = false;

    }


    public void Throw(GameObject nade)
    {
        float xdistance;
        xdistance = target.position.x - shotSpawner.position.x;

        float ydistance;
        ydistance = target.position.y - shotSpawner.position.y;

        float throwAngle; // in radian

        throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);

        float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);

        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);
        Vector3 throwsite = shotSpawner.position;


        Rigidbody2D rigid;
        rigid = nade.GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector2(xVelo, yVelo);
        if (right)
        {
            rigid.angularVelocity = -100f;
        }
        else if (!right)
        {
            rigid.angularVelocity = 100f;
        }

    }

    public void endGame()
    {
        StartCoroutine(creditos());
    }

    IEnumerator creditos()
    {
        yield return new WaitForSeconds(0.5f);
        fade.GetComponent<Animator>().SetBool("FadeOut", true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("CreditScene");
    }

}
