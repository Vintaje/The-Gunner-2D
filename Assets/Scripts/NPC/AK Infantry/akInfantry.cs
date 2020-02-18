using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class akInfantry : MonoBehaviour
{
    public float speed;
    public float distance;

    private bool right = true;
    private bool detected = false;

    public Transform eyeDetection;
    private Transform target;
    public Transform targetDetection;

    public float fireRate;
    private float nextFire;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    private RaycastHit2D groundInfo;

    private float oldPosition = 0.0f;
    //Check derecha o izquierda
    public Vector2 pos1;
    private bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nextFire = 0.0f;
        oldPosition = transform.position.x;
        right = true;
        shooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(eyeDetection.position, Vector2.right, distance);
        if (groundInfo.collider == true)
        {

            if (!groundInfo.collider.gameObject.tag.Equals("Player") && !shooting)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);

            }

        }
        else
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
        if (detected)
        {
            if (!shooting)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed * Time.deltaTime);
            }
           

            if (Time.time > nextFire)
            {
                StartCoroutine(DoBlinks(0.9f));
                nextFire = Time.time + fireRate;
                GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);

                if (!right)
                {
                    tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
                }

            }


            if (transform.position.x > target.position.x) // he's looking right
            {
                right = false;
            }

            if (transform.position.x < target.position.x) // he's looking left
            {
                right = true;
            }

            if (!right)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        

        if ((transform.position.x - target.position.x) > distance ||
        (transform.position.x - target.position.x) < distance)
        {
            detected = false;

        }
        else
        {
            detected = true;
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


    IEnumerator DoBlinks(float seconds)
    {
        GetComponent<Animator>().SetBool("Shooting", true);
        shooting = true;
        yield return new WaitForSeconds(seconds);

        //make sure renderer is enabled when we exit
        GetComponent<Animator>().SetBool("Shooting", false);
        shooting = false;
    }
}
