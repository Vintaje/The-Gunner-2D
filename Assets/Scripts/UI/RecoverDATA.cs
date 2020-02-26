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
        player.GetComponent<Human>().vida = PlayerPrefs.GetInt("Vida");

        if (PlayerPrefs.GetInt("Intentos") <= 0)
        {
            recoverPlayer.intentos = 3;
            recoverPlayer.municionspec = 0;
            recoverPlayer.municionextr = 0;
            recoverPlayer.GetComponent<Human>().vida = 10;
        }
        else
        {
            recoverPlayer.municionspec = PlayerPrefs.GetInt("Spec");
            recoverPlayer.municionextr = PlayerPrefs.GetInt("Extra");
            recoverPlayer.intentos = PlayerPrefs.GetInt("Intentos");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
