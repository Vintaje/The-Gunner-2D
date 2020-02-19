using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeEnemigos : MonoBehaviour
{

    public GameObject enemigo_original;
    public Camera mainCamera;
    public Vector3 izquierda, derecha;
    private bool activo;
    public int cantidad;
    // Start is called before the first frame update
    void Start()
    {
        activo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activo)
        {
            if (cantidad >= 0)
            {
                DecideSiEnemigo();
            }
            else
            {
                mainCamera.GetComponent<CameraFollow>().enabled = true;
                Destroy(gameObject);
            }
        }

    }


    private void DecideSiEnemigo()
    {
        float random = Random.Range(0.0f, 100.0f);
        if (random < 0.7f)
        {
            GameObject.Instantiate(enemigo_original, transform.position, transform.rotation).GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            cantidad--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Invoke("inicio", 1.0f);
        }
    }


    void inicio()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        mainCamera.GetComponent<CameraFollow>().enabled = false;
        activo = true;
    }
}
