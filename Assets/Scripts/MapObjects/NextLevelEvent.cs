using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelEvent : MonoBehaviour
{
    public GameObject boss;
    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            Human human = other.gameObject.GetComponent<Human>();
            Player player = other.gameObject.GetComponent<Player>();
            PlayerPrefs.SetInt("Vida", human.vida);
            PlayerPrefs.SetInt("Spec", player.municionspec);
            PlayerPrefs.SetInt("Extra", player.municionextr);
            PlayerPrefs.SetInt("Intentos", player.intentos);
            SceneManager.LoadScene(nextScene);
        }
    }
}
