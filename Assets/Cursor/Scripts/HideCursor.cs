using UnityEngine.UI;

public class HideCursor
{
    Image _cursor;

    public HideCursor(Image cursorImage)
    {
        _cursor = cursorImage;
    }

    public void Hide()
    {
        _cursor.enabled = false;
    }
    public void Visible()
    {
        _cursor.enabled = true;
    }
}
