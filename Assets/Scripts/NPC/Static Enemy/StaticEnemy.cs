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


    //Disparo
    public GameObject bulletPrefab;
    public Transform shotSpawner;

    private AudioSource shot;

    // Start is called before the first frame update
    void Start()
    {
        shot = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
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
            StartCoroutine(DoBlinks(0.9f));
            nextFire = Time.time + fireRate;
            GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);

            if (!right)
            {
                tempBullet.transform.eulerAngles = new Vector3(0, 0, 180);
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
}
