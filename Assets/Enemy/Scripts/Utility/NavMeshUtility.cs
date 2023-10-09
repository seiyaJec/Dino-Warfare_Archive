using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtility
{
    //NavMesh上でランダムな位置を取得する
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);
        if (float.IsInfinity(hit.position.x))
        {
            hit.position = GetRandomPointOnNavMesh(center, distance, areaMask);
        }
        return hit.position;
    }
}