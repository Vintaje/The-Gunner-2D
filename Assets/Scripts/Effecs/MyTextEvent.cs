using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTextEvent : MonoBehaviour
{
    public static MyTextEvent Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance && gameObject.GetComponent<TextMesh>().text.Equals("OUT OF AMMO!"))
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, 10.0f, 0.0f));
            Destroy(gameObject, 1.5f);
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
