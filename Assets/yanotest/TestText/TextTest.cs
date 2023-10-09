using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    public GameObject iki;
    public GameObject run;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            iki.SetActive(false);
            run.SetActive(false);
        }
    }
}
