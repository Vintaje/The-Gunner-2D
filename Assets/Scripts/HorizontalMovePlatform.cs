using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovePlatform : MonoBehaviour
{
    private float speedUpDown = 0.33f;
    private float distanceUpDown = 2;

    private void Start()
    {
    }
    void Update()
    {
        Vector3 mov = new Vector3(Mathf.Sin(speedUpDown * Time.time) * distanceUpDown+8f, transform.position.y, transform.position.z);
        transform.position = mov;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
    }
}
