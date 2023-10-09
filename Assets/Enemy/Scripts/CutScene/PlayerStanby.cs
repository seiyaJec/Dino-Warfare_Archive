using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStanby : MonoBehaviour
{
    private PlayerScript playerScript;
    private GameObject[] UiObjects;

    void Awake()
    {
        UiObjects = GameObject.FindGameObjectsWithTag("UI");
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnEnable()
    {
        foreach (var obj in UiObjects)
        {
            obj.SetActive(false);
        }
        playerScript.enabled = false;
    }

    private void OnDisable()
    {
        foreach (var obj in UiObjects)
        {
            obj.SetActive(true);
        }
        playerScript.enabled = true;
    }
}
