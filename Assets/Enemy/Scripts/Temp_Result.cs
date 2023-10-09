using UnityEngine;
using UnityEngine.UI;

public class Temp_Result : MonoBehaviour
{
    [SerializeField] private Text text;

    private void Start()
    {
        if (GameData.state == GameData.GameState.GAMECLEAR)
        {
            text.text = "GameClear";
        }
        else
        {
            text.text = "GameOver";
        }
    }

    public void OnButton()
    {
        LoadingSceneController.LoadScene("NewMap");
    }

}
