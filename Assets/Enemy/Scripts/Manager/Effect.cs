using UnityEngine;

public class Effect : MonoBehaviour
{
    [field: SerializeField] public EffectManager.EffectType type { get; private set; }
    [SerializeField] private float duration;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            gameObject.SetActive(false);
        }
    }
}
