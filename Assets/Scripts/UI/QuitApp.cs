using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         //quit app
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
       if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
}
