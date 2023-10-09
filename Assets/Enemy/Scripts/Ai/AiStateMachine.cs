public class AiStateMachine
{
    public AiState[] states;
    public AiAgent agent;
    public AiStateId currentState;

    //ステートマシン初期化
    public AiStateMachine(AiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AiState[numStates];
    }

    //ステート登録
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;      

        //AIState初期化
        state.Init(agent);
    }

    //ステートマシンに登録されているステート取得
    public AiState GetState(AiStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    //現ステートの処理更新
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    //状態を変える
    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
