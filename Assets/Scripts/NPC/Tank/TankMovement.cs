using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    public float visionRadius;
    public float speed;
    private GameObject player;
    private float oldPosition;
    public Transform shotSpawner;
    public float timeTillHit;
    public GameObject bulletPrefab;
    private bool right;
    public float fireRate;
    private float nextFire;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        float fixedSpeed = speed * Time.deltaTime;

        if (dist < visionRadius)
        {

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), fixedSpeed);
            Debug.DrawLine(transform.position, player.transform.position, Color.green);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Throw();
            }

        }



        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1);
            right = false;
        }

        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1);
            right = true;

        }

    }
    public void ResetAttack()
    {
    }

    public void Throw()
    {
        float xdistance;
        xdistance = player.transform.position.x - shotSpawner.position.x;

        float ydistance;
        ydistance = player.transform.position.y - shotSpawner.position.y;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
