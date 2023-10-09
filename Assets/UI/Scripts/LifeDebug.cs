using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDebug : MonoBehaviour
{
    [SerializeField]
    private List<LivingEntity> _livingEntitys;

    private Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_livingEntitys == null)
            return;

        _text.text = "";

        foreach(LivingEntity livingEntity in _livingEntitys)
        {
            _text.text += livingEntity.gameObject.name + ":" + livingEntity.currentHealth.ToString() + "\n";
        }
    }
}
