using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkCameraShake : MonoBehaviour
{
    [SerializeField] public float _shakeMin;
    [SerializeField] public float _shakeMax;
    [SerializeField] public float _shakeSpeed;
    [SerializeField] public bool _shaking;          //true‚È‚çƒJƒƒ‰‚Ì—h‚ê‚ª—LŒø

    [HideInInspector]private float _moveCount;


    private void Start()
    {
        _moveCount = 0;
    }

    public void StartShake()
    {
        _shaking = true;
    }


    void Update()
    {
        if( _shaking || _moveCount != 0)
        {
            Vector3 cameraPos = transform.localPosition;
            _moveCount += Time.deltaTime * _shakeSpeed;


            if (_moveCount > 1.0f)
            {
                cameraPos.y = 0;
                _moveCount = 0;
            }
            else
            {
                cameraPos.y = Mathf.Sin((_moveCount * 360) * Mathf.Deg2Rad);
                if(cameraPos.y < 0f)
                {
                    cameraPos.y *= _shakeMin;
                }
                else
                {
                    cameraPos.y *= _shakeMax;
                }
            }


            transform.localPosition = cameraPos;

        }
    }
}
