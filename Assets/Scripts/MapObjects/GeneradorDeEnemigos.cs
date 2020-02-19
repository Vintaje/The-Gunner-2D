using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeEnemigos : MonoBehaviour
{

    public GameObject enemigo_original;
    public GameObject helicoptero;
    public GameObject heli_state;

    public Camera mainCamera;
    public float seconds;

    public Vector3 izquierda, derecha;
    private bool activo;
    public int cantidad;

    private EdgeCollider2D[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        

        colliders = GetComponents<EdgeCollider2D>();

        foreach (EdgeCollider2D col in colliders)
        {
            col.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (activo)
        {
            Debug.Log(heli_state);
            if (heli_state == null)
            {
                Debug.Log("Acaba de ocurri algo");
                activo = false;
                mainCamera.GetComponent<CameraFollow>().habilitado = true;
                
                Invoke("recuperarCamara",1.0f);
                Destroy(gameObject, 1.5f);
                Debug.Log("Fase 1 Terminada");

            }
        }

    }

    void recuperarCamara(){
        mainCamera.GetComponent<CameraFollow>().enabled = true;
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
        GameObject.Instantiate(helicoptero, izquierda, transform.rotation);
        heli_state = GameObject.FindGameObjectWithTag("Helicopter");
        
        foreach (EdgeCollider2D col in colliders)
        {
            col.enabled = true;
        }
        mainCamera.GetComponent<CameraFollow>().habilitado = false;
        activo = true;
        GetComponent<BoxCollider2D>().enabled = false;
        Spawner();
    }

    void Spawner()
    {
        StartCoroutine(DoBlinks(cantidad));
    }

    IEnumerator DoBlinks(int numBlinks)
    {
        for (int i = 0; i < numBlinks * 2; i++)
        {
            yield return new WaitForSeconds(seconds);
            //toggle renderer
            if (cantidad >= 0)
            {
                izquierda.z -= cantidad;
                derecha.z -= cantidad;
                GameObject.Instantiate(enemigo_original, izquierda, transform.rotation).GetComponent<Rigidbody2D>().gravityScale = 1.0f;

                cantidad -= 2;
            }

            //wait for a bit

        }

        //make sure renderer is enabled when we exit

    }
}
