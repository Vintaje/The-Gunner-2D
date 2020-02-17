using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HommingMissile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed;
    public int damage = 1;
    public float destroyTime = 0.1f;
    public GameObject bulletExplo;
    private Rigidbody2D rb;
    public float angularspeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, destroyTime);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * angularspeed;
        rb.velocity = transform.up * speed;

    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        this.gameObject.SetActive(false);
        restos();

    }

    private void restos()
    {
        GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }


}
