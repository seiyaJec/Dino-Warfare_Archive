using UnityEngine;

//ダメージクラス
public struct DamageMessage
{
    public GameObject damager; //攻撃側
    private float amount;
    public float Amount
    {
        get { return amount; }
        set
        {
            if (value > 0)
            {
                amount = value;
            }
            else
            {
                amount = 0;
            }
        } 
    }

    public Vector3 hitPoint;
    public Vector3 hitNormal;
}