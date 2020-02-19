using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public bool habilitado;

    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void Start()
    {
        habilitado = true;
    }


    void LateUpdate()
    {
        if (habilitado)
        {
            Vector3 desiredPosition = player.position+offset;
            desiredPosition.y += 0.5f;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.z = -20.0f;
            transform.position = smoothedPosition;
        }
    }

    

}
