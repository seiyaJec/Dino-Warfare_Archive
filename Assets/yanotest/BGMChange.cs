using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChange : MonoBehaviour
{
    float time;
    public float fadetime;
    bool BGMkey;
    public AudioSource audioSorce;

    private void Start()
    {
        BGMkey = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (BGMkey)
        {
            time += Time.deltaTime;

            if(audioSorce.volume <= 0)
            {
                audioSorce.Stop();
            }

            if(time > 0.1)
            {
                audioSorce.volume = audioSorce.volume - fadetime;
                time = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BGMkey = true;
        }
    }
}
