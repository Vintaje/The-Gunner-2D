using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    private Transform target;
    private bool right = false;
    public float distanceToShot;
    private float distanceToTarget;
    public float fireRate;
    private float nextFire;
    public float timeTillHit = 1f;

    //Disparo
    public GameObject bulletPrefab;
    public Transform shotSpawner;

    private AudioSource shot;

    private Human human;

    public bool mortar;
    // Start is called before the first frame update
    void Start()
    {
        human = GetComponent<Human>();
        shot = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!human.muerto)
        {

            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (transform.position.x < target.position.x) // he's looking right
            {
                right = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (transform.position.x > target.position.x) // he's looking left
            {
                right = false;
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            if (Time.time > nextFire && distanceToTarget < distanceToShot)
            {
                StartCoroutine(DoBlinks(1.4f));
                if (!mortar)
                {
                    StartCoroutine(DoBlinks(0.9f));
                    nextFire = Time.time + fireRate;
                    GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, gameObject.transform.rotation);

                }
                else
                {
                    nextFire = Time.time + fireRate;
                    Throw();
                }

            }
        }

    }

    IEnumerator DoBlinks(float seconds)
    {
        shot.Play();
        GetComponent<Animator>().SetBool("Shooting", true);

        yield return new WaitForSeconds(seconds);

        //make sure renderer is enabled when we exit
        GetComponent<Animator>().SetBool("Shooting", false);

    }

    public void Throw()
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
        GameObject bulletInstance = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation) as GameObject;

        Rigidbody2D rigid;
        rigid = bulletInstance.GetComponent<Rigidbody2D>();

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
