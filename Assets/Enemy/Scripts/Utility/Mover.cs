using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    void Start()
    {
        //Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition = Input.mousePosition;
    }
}
