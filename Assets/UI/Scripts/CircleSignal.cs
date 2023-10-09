using UnityEngine;
using UnityEngine.UI;

public class CircleSignal : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image cImage;
    [SerializeField] private RectTransform rect;
    [SerializeField] private RectTransform cRect;

    [SerializeField] private float startSize = 6.0f;
    [SerializeField] private float rotateSpeed = 60.0f;

    public Transform target { get; private set; }

    private float duration;
    private float timer = 0f;

    private void Awake()
    {
        duration = 5.0f;
    }

    public void Init(Transform target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }

    private void OnEnable()
    {
        cRect.localScale = new Vector3(startSize, startSize, 1.0f);
        timer = 0f;
    }

    private void OnDisable()
    {
        image.enabled = false;
        cImage.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / duration;
        float size = Mathf.Lerp(startSize, 1.0f, t);

        cRect.localScale = new Vector3(size, size, 1);

        if (size <= 1f || !target)
        {
            this.enabled = false;
        }

        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
        rect.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
        
        if (!image.enabled && this.enabled)
        {
            image.enabled = true;
            cImage.enabled = true;
        }
    }
}
