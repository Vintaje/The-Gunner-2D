using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string nextScene;
    public float waitTime = 35.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Saltar")){
            SceneManager.LoadScene(nextScene);
        }
    }


    IEnumerator startScene()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nextScene);
    }
}
