﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public int damage = 1;
    public float destroyTime = 0.1f;
    public GameObject bulletExplo;
    public GameObject stream;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("disable", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (stream != null)
        {
            GameObject streamB = Instantiate(stream, gameObject.transform.position, gameObject.transform.rotation);
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("EnemyBullet"))
        {
            gameObject.SetActive(false);
            GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 0.2f);
        }

    }

    public void disable()
    {
        GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }


}
