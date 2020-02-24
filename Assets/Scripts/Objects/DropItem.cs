using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject[] items;

    public float dropRate;
    float sum;

    public void Spawn()
    {
        Random random = new Random();
        if (Random.Range(0.0f, 100.0f) < dropRate)
        {
            int num = Random.Range (0, 100); 
            if(num > 30){
                num = 1;
            } else{
                num = 0;
            }
            Instantiate(items[num], transform.position, Quaternion.identity);
        }else{
            Debug.Log("No dropea nada");
        }
    }
}
