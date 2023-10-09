using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 破壊可能なビル
/// https://www.youtube.com/watch?v=0OivAwgmW8M
/// </summary>
public class KnockableBuilding : MonoBehaviour
{
    [SerializeField] Transform brokenPrefab;

    /// <summary>
    /// 左クリックでビル破壊
    /// </summary>
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Transform brokenTransform = Instantiate(brokenPrefab,transform.position,transform.rotation);
            brokenTransform.localScale = transform.localScale;
            foreach(Rigidbody rigidbody in brokenTransform.GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.AddExplosionForce(2000f,transform.position + Vector3.up * 0.5f,50f);
            }

            Destroy(gameObject);
        }
    }
}
