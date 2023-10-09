using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEvent : MonoBehaviour
{
    [SerializeField] private Transform lookTransform;
    [SerializeField] private float _rotateSpeed;

    private Transform _targetTransform;
    private Vector3   _direction;
    private bool      _isRotation;

    // Start is called before the first frame update
    void Start()
    {
        _direction = Vector3.zero;
        _isRotation = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRotation)
            return;
        if (_targetTransform == null)
            return;

        //‰ñ“]‚·‚é•ûŒü‚ð‹‚ß‚é
        _direction = lookTransform.position - this.transform.position;
        if (_direction.sqrMagnitude >= 1.0f)
            _direction.Normalize();

        //‰ñ“] ŽŸ‚Ìƒ|ƒCƒ“ƒg‚ÖŒü‚­
        Quaternion rotate = Quaternion.LookRotation(_direction);
        _targetTransform.rotation = Quaternion.Slerp(_targetTransform.rotation, rotate, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isRotation = true;
            _targetTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isRotation = false;
            _targetTransform = null;
        }
    }
}