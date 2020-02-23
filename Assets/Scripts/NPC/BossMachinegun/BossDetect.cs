using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetect : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mainCamera;
    public GameObject boss;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Dentro");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag.Equals("Player"))
        {
            boss.GetComponent<Animator>().SetBool("Start", true);
            mainCamera.GetComponent<CameraFollow>().player = gameObject.transform;
            StartCoroutine(closeCollider());
        }
    }

    IEnumerator closeCollider()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Collider2D>().isTrigger = false;
    }
}
