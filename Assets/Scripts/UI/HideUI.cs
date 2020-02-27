using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject joystick;
    public GameObject shot;
    public GameObject jump;
    public GameObject switchB;

    public Canvas canvas;
    private bool hide;
    private bool started;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            joystick.SetActive(false);
            shot.SetActive(false);
            jump.SetActive(false);
            switchB.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButtonDown("HideUI"))
        {
            if (!hide)
            {
                hide = true;
            }
            else if (hide)
            {
                hide = false;
            }
            canvas.enabled = hide;
        }
    }
}
