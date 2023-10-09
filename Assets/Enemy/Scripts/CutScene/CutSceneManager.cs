using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    //Singleton
    private static CutSceneManager m_Instance;
    public static CutSceneManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<CutSceneManager>();
            return m_Instance;
        }
    }

    public enum SceneType
    {
        Triceratops,
        Tyranno
    }

    [SerializeField] private CutScene[] scenes;

    public void PlayCutScene(SceneType type)
    {
        foreach (var scene in scenes)
        {
            if (scene.type == type)
            {
                scene.timeLine.SetActive(true);
            }
        }
    }
}
