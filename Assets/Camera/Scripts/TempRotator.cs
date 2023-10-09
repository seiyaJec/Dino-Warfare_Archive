using UnityEngine;
using System.Collections;

public class TempRotator : MonoBehaviour
{
    [SerializeField] private float yaw;
    [SerializeField] private float pitch;
    [SerializeField] private float duration;
    [Range(0f, 1f)] private float t;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Rotate(other.transform));
    }

    IEnumerator Rotate(Transform target)
    {
        float startYaw = target.eulerAngles.y;
        float targetYaw = startYaw;

        float startPitch = target.eulerAngles.x;
        float targetPitch = startPitch;

        t = 0f;

        while(targetYaw != yaw && targetPitch != pitch)
        {
            t += Time.deltaTime / duration;

            targetYaw = Mathf.Lerp(startYaw, yaw, t);
            targetPitch = Mathf.Lerp(startPitch, pitch, t);

            target.rotation = Quaternion.Euler(targetPitch, targetYaw, target.rotation.z);

            yield return new WaitForEndOfFrame();
        }
    }
}
