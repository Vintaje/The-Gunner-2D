using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    public int weapon;
    public int cantidad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag.Equals("Player")){
            if(weapon == 1){
                collider.gameObject.GetComponent<Player>().municionspec += cantidad;
            }else if (weapon == 2){
                collider.gameObject.GetComponent<Player>().municionextr += cantidad;
            }
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            Destroy(gameObject, 0.9f);
        }
        

    }
}
