using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public float normalspeed;
    private float fastspeed;
    private float speed;
    public float distance;
    private bool right = true;
    public Transform eyeDetection;

    
    public float timeTillHit = 1f;//If projectile
    public bool projectile;
    public float fireRate;
    private float nextFire;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    private float oldPosition = 0.0f;
    //Sonido
    private AudioSource shot;
    //Scripts
    Human human;
    Detection detection;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = 0.0f;
        oldPosition = transform.position.x;
        right = true;
        shot = GetComponents<AudioSource>()[0];
        speed = normalspeed;
        fastspeed = 2f * normalspeed;
        human = GetComponent<Human>();
        detection = GetComponent<Detection>();
        human.muerto = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!human.muerto)
        {

            RaycastHit2D groundInfo = Physics2D.Raycast(eyeDetection.position, Vector2.down, distance);

            if (groundInfo.distance > 0.03f)
            {
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            else
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (!detection.detected)
            {
                speed = normalspeed;
                if (groundInfo.collider == true && (groundInfo.collider.gameObject.tag.Equals("Floor") || groundInfo.collider.gameObject.tag.Equals("EnemyBullet")) && !detection.shot)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                if (groundInfo.collider == true && groundInfo.collider.gameObject.tag.Equals("LimiteEnemigos"))
                {
                    if (right)
                    {
                        right = false;
                    }
                    else
                    {
                        right = true;
                    }
                }
            }
            else if (detection.detected)
            {

                if (!detection.shot && groundInfo.collider == true && groundInfo.collider.gameObject.tag.Equals("Floor"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(detection.target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed * Time.deltaTime);
                }
                if (detection.shot)
                {
                    speed = 0;
                }
                else
                {
                    speed = fastspeed;
                }
                if (Time.time > nextFire && detection.shot)
                {
                    StartCoroutine(DoBlinks());
                    Debug.Log("Disparando");
                    nextFire = Time.time + fireRate;
                }
                if (transform.position.x < detection.target.position.x) // he's looking right
                {
                    right = true;
                }

                if (transform.position.x > detection.target.position.x) // he's looking left
                {
                    right = false;
                }
            }
            if (transform.position.x > oldPosition + 0.2f) // he's looking right
            {
                right = true;
            }

            if (transform.position.x < oldPosition - 0.2f) // he's looking left
            {
                right = false;
            }
            oldPosition = transform.position.x;
        }
    }

    void FixedUpdate()
    {
        if (right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (!right)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    IEnumerator DoBlinks()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
        if (!right)
        {
            tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (projectile)
        {
            Throw(tempBullet);
        }

        shot.Play();
        GetComponent<Animator>().SetBool("Shooting", true);

        yield return new WaitForSeconds(0.9f);

        //make sure renderer is enabled when we exit
        GetComponent<Animator>().SetBool("Shooting", false);

    }



    public void Throw(GameObject bullet)
    {
        float xdistance;
        xdistance = detection.target.position.x - shotSpawner.position.x;

        float ydistance;
        ydistance = detection.target.position.y - shotSpawner.position.y;

        float throwAngle; // in radian

        throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);

        float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);

        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);
        Vector3 throwsite = shotSpawner.position;


        Rigidbody2D rigid;
        rigid = bullet.GetComponent<Rigidbody2D>();

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
