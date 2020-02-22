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
    private bool patron;

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

    private GameObject[] enemies;

    //Script general
    Human human;

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
                run = false;
                GetComponent<Animator>().SetBool("Attacking", attack);
                GetComponent<Animator>().SetBool("Running", run);

            }
            else if (detected)
            {
                if (distanceToTarget > distanceToShot && Mathf.Abs(transform.position.y - target.position.y) < 0.6f && !patron)
                {
                    run = true;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed * Time.deltaTime);
                }
                else
                {
                    run = false;
                }
                GetComponent<Animator>().SetBool("Running", run);
                if (Time.time > nextFire && distanceToTarget < distanceToShot && Mathf.Abs(transform.position.y - target.position.y) < 0.6f)
                {
                    GetComponent<Animator>().SetBool("Attacking", true);
                    StartCoroutine(BulletDelay(0.8f));
                    StartCoroutine(DoBlinks(0.9f));
                    nextFire = Time.time + fireRate;



                }

                if (Time.time > nextPatron && distanceToTarget < distanceToShot * 4 && Mathf.Abs(transform.position.y - target.position.y) < 0.6f)
                {
                    nextPatron = Time.time + patronRate;
                    patron = true;
                    StartCoroutine(patronAtaque());


                }

                Debug.Log(Mathf.Abs(transform.position.x - target.position.x));
                if (transform.position.x < target.position.x && Mathf.Abs(transform.position.x - target.position.x) > 0.5f) // he's looking right
                {
                    right = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (transform.position.x > target.position.x && Mathf.Abs(transform.position.x - target.position.x) > 0.5f) // he's looking left
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

            if (transform.position.x > oldPosition && Mathf.Abs(oldPosition - transform.position.x) < 0.3f) // he's looking right
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                right = true;
            }

            if (transform.position.x < oldPosition && Mathf.Abs(oldPosition - transform.position.x) < 0.3f) // he's looking left
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                right = false;
            }
            oldPosition = transform.position.x;
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
        for (int i = 0; i < 5; i++)
        {

            Instantiate(bullet, patronSpawner.position, patronSpawner.rotation);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 5; i++)
        {

            Instantiate(hommingMissile, patronSpawner.position, patronSpawner.rotation).transform.Translate(new Vector3(0.0f, -0.2f, 0.0f));
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(1.5f);
        patron = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("LimiteEnemigos"))
        {
            if (right)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                right = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                right = true;
            }
        }
    }


}
