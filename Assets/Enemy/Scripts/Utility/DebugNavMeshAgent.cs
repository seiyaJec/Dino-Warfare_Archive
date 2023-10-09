using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

#if UNITY_EDITOR//ビルドエラー防止
//エディタ上のシーンデバッグ用
public class DebugNavMeshAgent : MonoBehaviour
{
    [SerializeField] private bool velocity;
    [SerializeField] private bool desiredVelocity;
    [SerializeField] private bool path;
    [SerializeField] private bool atRoot;
    [SerializeField] private bool eyeTr;

    [SerializeField] private AiAgentConfig config;
    [SerializeField] private Transform attackRoot;
    [SerializeField] private Transform eyeTransform;

    private NavMeshAgent navMesh;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }


    private void OnDrawGizmos()
    {
        if (config)
        {
            if (attackRoot != null && atRoot)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawSphere(attackRoot.position, config.attackRadius); //オブジェクトの攻撃半径を球体に描画
            }

            if (eyeTransform != null && eyeTr)
            {
                var leftEyeRotation = Quaternion.AngleAxis(-config.fieldOfView * 0.5f, Vector3.up);// vector3.upの基準になる軸はY軸

                var leftRayDirection = leftEyeRotation * transform.forward;

                Handles.color = new Color(1f, 1f, 1f, 0.2f);
                Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, config.fieldOfView, config.viewDistance);
            }
        }

        if (navMesh)
        {
            if (velocity)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + navMesh.velocity);
            }

            if (desiredVelocity)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + navMesh.desiredVelocity);
            }

            if (path)
            {
                Gizmos.color = Color.gray;
                var agentPath = navMesh.path;
                Vector3 prevCorner = transform.position;
                foreach (var corner in agentPath.corners)
                {
                    Gizmos.DrawLine(prevCorner, corner);
                    Gizmos.DrawSphere(corner, 0.15f);
                    prevCorner = corner;
                }
            }
        }
    }
}
#endif