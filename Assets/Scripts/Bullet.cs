using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500;
    public int damage = 1;
    public float destroyTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }



    private void OnTriggerEnter2d(Collider2D other){
        Destroy(gameObject);
    }
}
