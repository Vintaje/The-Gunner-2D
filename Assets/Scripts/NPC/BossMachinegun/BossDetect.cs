using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetect : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mainCamera;
    public GameObject boss;
    private bool show;
    public GameObject arrow;
    public bool normal;
    public int direccionWall; //1 izquierda, 2 derecha, 3 abajo
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!show && boss.GetComponent<Human>().vida <= 0)
        {
            show = true;
            arrow.SetActive(show);
            mainCamera.GetComponent<CameraFollow>().player = GameObject.FindGameObjectWithTag("Player").transform;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<AudioSource>().Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            boss.GetComponent<Animator>().SetBool("Start", true);
            mainCamera.GetComponent<CameraFollow>().player = gameObject.transform;
            StartCoroutine(closeCollider());
            if (!normal)
            {
                GetComponent<AudioSource>().Play();
                mainCamera.GetComponent<AudioSource>().Stop();
                GetComponents<AudioSource>()[0].Play();
                GetComponents<AudioSource>()[1].Play();
            }
        }
    }

    IEnumerator closeCollider()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Collider2D>().isTrigger = false;
        if (direccionWall == 1)
        {
            GetComponent<Collider2D>().offset = new Vector2(-3.6f, GetComponent<Collider2D>().offset.y);
        }
        else if (direccionWall == 2)
        {
            GetComponent<Collider2D>().offset = new Vector2(3.6f, GetComponent<Collider2D>().offset.y);
        }
        else if (direccionWall == 3)
        {
            GetComponent<Collider2D>().offset = new Vector2(GetComponent<Collider2D>().offset.x, -1.5f);
        }
    }
}
