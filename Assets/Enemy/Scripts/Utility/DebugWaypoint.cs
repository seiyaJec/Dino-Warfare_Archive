using UnityEngine;

public class DebugWaypoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = Color.red;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        UnityEditor.Handles.Label(transform.position, "WayPoint " + "("
            + x + ", " + y + ", " + z + ")", style);

        Gizmos.color = new Vector4(0f, 0f, 0f, 0.3f);  
        Gizmos.DrawSphere(transform.position, 1f);
    }

#endif
}
