using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberFly : MonoBehaviour
{

    public float speed;
    public float distance;

    private bool right = true;
    private bool detected = false;

    public Transform groundDetection;
    private Transform target;
    public Transform targetDetection;

    public float fireRate;
    private float nextFire;
    public GameObject bulletPrefab;
    public Transform shotSpawner;
    private RaycastHit2D targetInfo;
    private RaycastHit2D groundInfo;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nextFire = 0.0f;
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
    }

    // Update is called once per frame
    void Update()
    {

        if (groundInfo.collider.gameObject.tag.Equals("Player"))
        {
            detected = true;
        }
        if (detected)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed * Time.deltaTime);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                GameObject tempBullet = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);
            }
        }
        else if (!detected)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!groundInfo.collider.gameObject.tag.Equals("Floor"))
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
}
