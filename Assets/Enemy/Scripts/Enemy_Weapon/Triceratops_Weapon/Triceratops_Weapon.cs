using UnityEngine;

public class Triceratops_Weapon : MonoBehaviour
{
    [SerializeField] private Transform fireTransform;
    [SerializeField] private GameObject missilePref;

    private LivingEntity health;

    private void Awake()
    {
        health = GetComponent<LivingEntity>();
    }

    public bool dead
    {
        get { return health.dead; }
    }

    public void Fire(Transform target)
    {
        var missile = Instantiate(missilePref, fireTransform.position, Quaternion.identity).GetComponent<HomingMissile_Move>();
        missile.Target = target;
    }
}
