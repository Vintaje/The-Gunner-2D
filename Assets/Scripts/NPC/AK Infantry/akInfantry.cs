using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class akInfantry : MonoBehaviour
{
    public float normalspeed;
    private float fastspeed;
    private float speed;
    public float distance;

    private bool right = true;
    private bool detected = false;

    public Transform eyeDetection;
    public Transform target;
    public Transform targetDetection;

    public float fireRate;
    private float nextFire;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    private float oldPosition = 0.0f;
    public float distanceToShot;
    private float distanceToTarget;

    //Sonido
    private AudioSource[] sounds;
    private AudioSource shot;
    private AudioSource alert;

    private GameObject[] enemies;

    //Script general
    Human human;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = 0.0f;
        oldPosition = transform.position.x;
        right = true;
        sounds = GetComponents<AudioSource>();
        shot = sounds[0];
        alert = sounds[1];
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
            distanceToTarget = Vector3.Distance(transform.position, target.position);
            RaycastHit2D groundInfo = Physics2D.Raycast(eyeDetection.position, Vector2.down, distance);

            if (groundInfo.distance > 0.35)
            {
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            else
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (!detected)
            {

                if (groundInfo.collider == true && groundInfo.collider.gameObject.tag.Equals("Floor") && distanceToTarget > distanceToShot)
                {

                    if (!groundInfo.collider.gameObject.tag.Equals("Player"))
                    {
                        transform.Translate(Vector2.right * speed * Time.deltaTime);

                    }

                }
                else if (!groundInfo.collider.gameObject.tag.Equals("EnemyBullets") && !groundInfo.collider.gameObject.tag.Equals("LimiteEnemigos") && distanceToTarget > distanceToShot)
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

                if (groundInfo.collider.gameObject.tag.Equals("LimiteEnemigos"))
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
            else if (detected)
            {
                if (distanceToTarget > distanceToShot && groundInfo.collider == true && groundInfo.collider.gameObject.tag.Equals("Floor"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed * Time.deltaTime);
                }
                if (Time.time > nextFire && distanceToTarget < distanceToShot)
                {
                    StartCoroutine(DoBlinks(0.9f));
                    nextFire = Time.time + fireRate;
                    GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);

                    if (!right)
                    {
                        tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                    }

                }


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


            }

            if (distanceToTarget > distance)
            {
                detected = false;
                speed = normalspeed;
            }
            else if (!detected)
            {
                alert.Play();
                detected = true;
                speed = fastspeed;
            }

            if (transform.position.x > oldPosition) // he's looking right
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                right = true;
            }

            if (transform.position.x < oldPosition) // he's looking left
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                right = false;
            }
            oldPosition = transform.position.x;
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
