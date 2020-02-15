using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovePlatform : MonoBehaviour
{
    private float speedUpDown = 0.33f;
    private float distanceUpDown = 2;
    private Vector3 mov;

    public Vector3 Mov
    {
        get { return mov; }
        set { mov = value; }
    }
    

    private void Start()
    {
    }
    void Update()
    {
        mov = new Vector3(Mathf.Sin(speedUpDown * Time.time) * distanceUpDown + 8f, transform.position.y, transform.position.z);
        transform.position = mov;

    }

   
}
