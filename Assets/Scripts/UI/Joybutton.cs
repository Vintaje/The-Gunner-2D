﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool Pressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Pressed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            this.enabled = false;
        }
    }
}
