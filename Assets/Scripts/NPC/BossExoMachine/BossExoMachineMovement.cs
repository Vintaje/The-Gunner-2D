using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExoMachineMovement : MonoBehaviour
{
    public float normalspeed;
    private float fastspeed;
    private float speed;
    public float distance;

    //Animaciones 
    private bool attack;
    private bool run;

    private bool right = true;
    private bool detected = false;
    public Transform target;

    public float fireRate;
    private float nextFire;

    public float patronRate;
    private float nextPatron;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    public Transform patronSpawner;

    public GameObject hommingMissile;
    public GameObject bullet;
    private float oldPosition = 0.0f;
    public float distanceToShot;
    private float distanceToTarget;


    public int numDisparos;
    public int numBombs;

    private GameObject[] enemies;


    public GameObject bossDetect;

    public GameObject door;

    //Script general
    Human human;

    private bool newPatron;
    private bool death;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = 0.0f;
        oldPosition = transform.position.x;
        right = true;
        speed = normalspeed;
        fastspeed = 2f * normalspeed;
        human = GetComponent<Human>();
        enemies = GameObject.FindGameObjectsWithTag("EnemyBullet");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        human.muerto = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!human.muerto)
        {
            Vector3 boss = new Vector3(transform.position.x, 0.0f, 0.0f);
            Vector3 targetx = new Vector3(target.position.x, 0.0f, 0.0f);
            distanceToTarget = Vector3.Distance(boss, targetx);


            if (!detected)
            {
                attack = false;

                GetComponent<Animator>().SetBool("Attacking", attack);

            }
            else if (detected)
            {

                if (Time.time > nextFire && distanceToTarget < 0.05f)
                {
                    GetComponent<Animator>().SetBool("Attacking", true);
                    StartCoroutine(BulletDelay(0.8f));
                    StartCoroutine(DoBlinks(0.9f));
                    nextFire = Time.time + fireRate;
                }

                if (Time.time > nextPatron && distanceToTarget < distanceToShot && Mathf.Abs(transform.position.y - target.position.y) < 0.6f)
                {
                    nextPatron = Time.time + patronRate;
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


        GetComponent<Animator>().SetBool("Attacking", false);

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
        if (human.vida > 10)
        {
            GetComponent<Animator>().SetBool("Attacking", false);
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(bullet, patronSpawner.position, patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;

                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < numBombs; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(hommingMissile, patronSpawner.position - new Vector3(0.0f, 0.1f, 0.0f), patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;

                    yield return new WaitForSeconds(1.0f);
                }
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < numDisparos / 2; i++)
            {
                GameObject b = Instantiate(hommingMissile, patronSpawner.position - new Vector3(0.0f, 0.05f, 0.0f), patronSpawner.rotation);
                b.GetComponent<Bullet>().destroyTime = 1.5f;

                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < numBombs; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(hommingMissile, patronSpawner.position, patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;

                    yield return new WaitForSeconds(0.25f);
                }
            }

            yield return new WaitForSeconds(1.0f);

        }
        else
        {
            if (!newPatron)
            {
                GetComponents<AudioSource>()[1].Play();
                newPatron = true;
            }
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(bullet, patronSpawner.position, patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;

                    yield return new WaitForSeconds(0.25f);
                }
            }

            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(hommingMissile, patronSpawner.position, patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;
 
                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < numDisparos; i++)
            {
                if (!human.muerto)
                {
                    GameObject b = Instantiate(hommingMissile, patronSpawner.position, patronSpawner.rotation);
                    b.GetComponent<Bullet>().destroyTime = 1.5f;

                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield return new WaitForSeconds(1.0f);

        }
    }




}
