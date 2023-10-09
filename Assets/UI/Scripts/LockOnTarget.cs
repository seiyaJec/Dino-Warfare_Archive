using UnityEngine;
using UnityEngine.UI;

public class LockOnTarget : MonoBehaviour
{
    public Transform target { get; private set; }

    //コンポネント
    private RectTransform rectTransform = null;
    private Image image;
    private Animator animator;

    //画面上に表示されているか
    public bool isActive { get; private set; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetActiveImage(true);
    }

    private void OnDisable()
    {
        SetActiveImage(false);
        image.enabled = false;
    }

    //ターゲット設定
    public void SetTarget(Transform target, float size)
    {
        this.target = target;
        if (size < 0.1f || size > 3.0f) { return; }
        rectTransform.localScale = new Vector3 (size, size, 1.0f);
    }

    //画面に表示させる
    private void SetActiveImage(bool isActive)
    {
        this.isActive = isActive;
        if (isActive)
        {
            animator.Rebind();
        }
        animator.enabled = isActive;
    }

    //ターゲットがなくなったらoffにする
    void Update()
    {
        if (!target || !isActive)
        {
            image.enabled = false;
            enabled = false;
            return;
        }

        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);

        if (!image.enabled)
        {
            image.enabled = true;
        }
    }
}