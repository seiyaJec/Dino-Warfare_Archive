//
[System.Serializable]
public class DamageReceiverID 
{
    public enum Part
    {
        Head,
        Arm,
        Leg,
        Body,
    }
    public Part id;

    public float damageRate;
}
