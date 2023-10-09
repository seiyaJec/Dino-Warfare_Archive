//ステートID、各ステートはこのIDを使って登録し、取得される
public enum AiStateId
{
    //All Enemy Common
    Idle,
    Detour,
    Chase,
    Death,
    Patrol,

    //Common Melee Attack
    NormalAttack,

    //Boss Common
    Stanby,
    BackOff,
    MoveLeft,
    MoveRight,
    Rush,
    GetHit,

    //
    RangedAttack,
}

//ステートインタフェース
public interface AiState
{
    AiStateId GetId(); //ステートのID取得
    void Init(AiAgent agent);
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent); 
}