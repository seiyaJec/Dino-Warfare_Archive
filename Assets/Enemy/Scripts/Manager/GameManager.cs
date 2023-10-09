using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;

    private void Awake()
    {
        
    }

    public void GameOver()
    {
        GameData.state = GameData.GameState.GAMEOVER;
        UIManager.Instance.SetActiveGameoverPanel();
    }

    public void GameClear()
    {
        FindObjectOfType<PlayerHealth>().Stanby();

        GameData.state = GameData.GameState.GAMECLEAR;
        UIManager.Instance.SetActiveGameClearPanel();
    }

    //-------------------------------------------------------
    //ci0329
    public void CheckContinue()
    {
        GameData.state = GameData.GameState.GAMEOVER;
        UIManager.Instance.SetActiveCheckContinuePanel();
    }
    //-------------------------------------------------------

}
