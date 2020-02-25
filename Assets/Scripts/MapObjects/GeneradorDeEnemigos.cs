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
    private bool activo;
    public int cantidad;
    private bool iniciado;
    private int mode;//1 Con vehiculo, 2 sin vehiculo

    public GameObject spawn;

    public GameObject vehicle;
    public DropItem[] drops;

    private EdgeCollider2D[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponents<EdgeCollider2D>();
        if (helicoptero == null)
        {
            mode = 2;
        }
        else
        {
            mode = 1;
        }
        drops = GetComponents<DropItem>();
    }

    // Update is called once per frame
    void Update()
    {

        if (activo)
        {

            if (heli_state == null && mode == 1)
            {
                Debug.Log("Acaba de ocurri algo");
                activo = false;
                mainCamera.GetComponent<CameraFollow>().habilitado = true;

                Invoke("recuperarCamara", 1.0f);
                Destroy(gameObject, 1.5f);
                Debug.Log("Fase Terminada");
                if(drops != null){
                    foreach(DropItem drop in drops){
                        drop.Spawn();
                    }
                }
            }
            if (mode == 2 && cantidad <= 0)
            {
                Debug.Log("Acaba de ocurri algo");
                activo = false;
                mainCamera.GetComponent<CameraFollow>().habilitado = true;

                Invoke("recuperarCamara", 1.0f);
                Destroy(gameObject, 1.5f);
                Debug.Log("Fase Terminada");
            }
        }

    }

    void recuperarCamara()
    {
        mainCamera.GetComponent<CameraFollow>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player") && !iniciado)
        {
            iniciado = true;
            Invoke("inicio", 0.5f);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }


    void inicio()
    {
        if (helicoptero != null)
        {
            heli_state = GameObject.Instantiate(helicoptero, vehicle.transform.position, transform.rotation);
        }
        foreach (EdgeCollider2D col in colliders)
        {
            col.enabled = true;
        }
        mainCamera.GetComponent<CameraFollow>().habilitado = false;
        activo = true;

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
                GameObject.Instantiate(enemigo_original, spawn.transform.position, transform.rotation).GetComponent<Rigidbody2D>().gravityScale = 1.0f;

                cantidad -= 1;
            }

            //wait for a bit

        }

        //make sure renderer is enabled when we exit

    }
}
