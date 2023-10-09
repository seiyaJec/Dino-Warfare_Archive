using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class TestIntroduction : MonoBehaviour
{

    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 23)
        {
            StartCoroutine(StartGame1());
        }
    }

    IEnumerator StartGame1()
    {
        yield return new WaitForSeconds(0);
        LoadingSceneController.LoadScene("Newmap");
    }
}
