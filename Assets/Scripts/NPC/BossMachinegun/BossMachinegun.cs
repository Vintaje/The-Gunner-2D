using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMachinegun : MonoBehaviour
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


    public GameObject bossDetect;

    //Script general
    Human human;
    public int timeTillHit;

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

                GetComponent<Animator>().SetBool("Knife", attack);
                GetComponent<Animator>().SetBool("Nade", attack);

            }
            else if (detected)
            {

                if (Time.time > nextFire && distanceToTarget < 0.05f)
                {
                    GetComponent<Animator>().SetBool("Knife", true);
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
        for (int i = 0; i < 5; i++)
        {

            Instantiate(bullet, patronSpawner.position, patronSpawner.rotation).GetComponent<Bullet>().destroyTime = 1.5f;
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < 5; i++)
        {
            gameObject.GetComponent<Animator>().SetBool("Nade", true);
            GameObject nade = Instantiate(hommingMissile, patronSpawner.position, patronSpawner.rotation);
            nade.transform.Translate(new Vector3(0.0f, -0.05f, 0.0f));
            Throw(nade);
            yield return new WaitForSeconds(1.0f);
            gameObject.GetComponent<Animator>().SetBool("Nade", false);

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

}
