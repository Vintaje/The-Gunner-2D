using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sign : MonoBehaviour
{
    public GameObject text;
    public GameObject image;
    public string mensaje;

    // Start is called before the first frame update
    void Start()
    {
        mensaje = mensaje.Replace("||", "\n");
        text.SetActive(false);
        image.SetActive(false);
        text.GetComponent<TextMesh>().text = mensaje;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            text.SetActive(true);
            image.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            text.SetActive(false);
            image.SetActive(false);
        }
    }
}
