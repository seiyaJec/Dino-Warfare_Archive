//ヒートモーションのインタフェース
public interface IGetHit
{
    //ダメージを計算が始まる
    void OnDamageCalculate(DamageReceiverID.Part part = DamageReceiverID.Part.Head);
    //ダメージ計算を消す
    void OffDamageCalculate();
}