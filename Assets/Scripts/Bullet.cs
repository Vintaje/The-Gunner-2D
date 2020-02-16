using System.Collections;
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
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        GameObject explosion = Instantiate(stream, gameObject.transform.position, gameObject.transform.rotation);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Bullet")
        {

            this.gameObject.SetActive(false);
            Invoke("restos", 0.1f);
            Destroy(this.gameObject, 0.5f);

        }
    }

    private void restos()
    {
        GameObject explosion = Instantiate(bulletExplo, gameObject.transform.position, gameObject.transform.rotation);
    }
}
