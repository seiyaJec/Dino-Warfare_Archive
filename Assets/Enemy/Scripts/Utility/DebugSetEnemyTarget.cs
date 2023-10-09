using UnityEngine;

public class DebugSetEnemyTarget : MonoBehaviour
{
    private void Start()
    {
        var target = GameObject.FindGameObjectWithTag("Player").GetComponent<LivingEntity>();
        var enemies = FindObjectsOfType<AiAgent>();

        if (target == null)
        {
            Debug.Log("No Player");
            return;
        }

        foreach (var enemy in enemies)
        {
            enemy.SetTarget(target);   
        }
    }

}
