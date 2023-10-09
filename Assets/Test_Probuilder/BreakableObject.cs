using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Transform brokenPrefab;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Transform brokenTransform = Instantiate(brokenPrefab,new Vector3(0,0,0),transform.rotation);

            foreach(Rigidbody rigidbody in brokenTransform.GetComponentsInChildren<Rigidbody>())
            {
                 rigidbody.AddExplosionForce(200f,transform.position + Vector3.up * 0.5f,5f);
            }

            Destroy(gameObject);
        }
    }
}
