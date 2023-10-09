using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField] private Transform fireTransform;
    [SerializeField] private GameObject grenadePref;
    [SerializeField] private Transform grenadeParent;

    private LivingEntity health;
    private LivingEntity ownerLife;

    public void SetOwnerLife(LivingEntity ownerLife)
    {
        this.ownerLife = ownerLife;
    }

    private void Awake()
    {
        health = GetComponent<LivingEntity>();
    }

    public bool dead
    {
        get { return health.dead; }
    }

    public bool OwnerDead()
    {
        return ownerLife.dead;
    }

    public GameObject Fire(Transform target)
    {
        GameObject instance = Instantiate(grenadePref, fireTransform.position, Quaternion.identity, grenadeParent);
        var grenadeMove = instance.GetComponent<Grenade_Move>();
        grenadeMove.Target = target;
        grenadeMove.SetOwnerLauncher(this.transform);
        return instance;
    }
}
