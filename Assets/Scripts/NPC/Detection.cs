using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Transform target;
    public float distanceToShot;
    public float distanceToTarget;
    public float distance;
    public bool detected;
    public bool shot;
    public AudioSource alert;
    public float targetDetectY;

    // Start is called before the first frame update
    void Start()
    {
        alert = gameObject.GetComponents<AudioSource>()[1];
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Mathf.Abs(transform.position.x - target.position.x);
        if (distanceToTarget > distance)
        {
            detected = false;

        }
        else if (!detected &&Mathf.Abs(transform.position.y - target.position.y) < targetDetectY)
        {
            detected = true;
            alert.Play();
        }
        
        if(distanceToTarget <= distanceToShot && detected){
            shot = true;
        }else{
            shot = false;
        }
    }
}
