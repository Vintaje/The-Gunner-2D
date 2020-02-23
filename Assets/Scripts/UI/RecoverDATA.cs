using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverDATA : MonoBehaviour
{
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        Player recoverPlayer = player.GetComponent<Player>();
        player.GetComponent<Human> ().vida = PlayerPrefs.GetInt("Vida");
        recoverPlayer.municionspec = PlayerPrefs.GetInt("Spec");
        recoverPlayer.municionextr = PlayerPrefs.GetInt("Extra");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
