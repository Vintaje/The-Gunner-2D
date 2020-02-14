using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
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



    private void OnCollisionEnter2D(Collision2D other){
       if (other.gameObject.tag != "Bullet")
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
