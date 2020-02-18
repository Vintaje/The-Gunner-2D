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
    private RaycastHit2D groundInfo;

    private float oldPosition = 0.0f;
    //Check derecha o izquierda
    public Vector2 pos1;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nextFire = 0.0f;
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        oldPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == true)
        {

            if (groundInfo.collider.gameObject.tag.Equals("Floor"))
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z), speed*1.1f * Time.deltaTime);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                GameObject tempMissile = Instantiate(bulletPrefab, shotSpawner.position, shotSpawner.rotation);

            }
        }

        if ((transform.position.x - target.position.x) > 1.5 ||
        (transform.position.x - target.position.x) < -1.5)
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
            right = true;
        }
        oldPosition = transform.position.x;
        Debug.Log("Detected Enemy" + detected);
    }
}
