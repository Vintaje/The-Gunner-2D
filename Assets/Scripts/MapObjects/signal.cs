using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class signal : MonoBehaviour
{

    public Image image;
    public Text text;
    public string mensaje;

    // Start is called before the first frame update
    void Start()
    {
        text.text = mensaje;
        image.enabled = false;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            image.enabled = true;
            text.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            image.enabled = false;
            text.enabled = false;
        }
    }
}
