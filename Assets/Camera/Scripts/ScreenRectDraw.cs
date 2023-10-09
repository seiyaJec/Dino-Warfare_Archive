using UnityEngine;

public class ScreenRectDraw : MonoBehaviour
{
    LineRenderer _lineRenderer;
    Camera _camera;

    static Rect _screenRect;

    void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 4;
        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;

        _camera = Camera.main;
    }

    public void SetCamera(in Camera camera)
    {
        _camera = camera;
    }

    public void SetEdge(float top, float bottom, float left, float right)
    {
        _screenRect.Set(left, bottom, right, top);
        Vector3[] points = new Vector3[4];
        points[0] = new Vector3(left,  top,    _camera.transform.forward.z);
        points[1] = new Vector3(left,  bottom, _camera.transform.forward.z);
        points[2] = new Vector3(right, bottom, _camera.transform.forward.z);
        points[3] = new Vector3(right, top,    _camera.transform.forward.z);

        //ÉèÅ[ÉãÉhç¿ïWÇ…ïœä∑
        for (int i = 0; i < points.Length; ++i)
        {
            points[i] = _camera.ScreenToWorldPoint(points[i]);
        }

        _lineRenderer.SetPositions(points);
    }
    public void SetPoints(in Vector3[] points)
    {
        Vector3[] pointsCopy = points;
        for (int i = 0; i < points.Length; ++i)
        {
            pointsCopy[i] = _camera.ScreenToViewportPoint(points[i]);
        }
        _lineRenderer.positionCount = pointsCopy.Length;
        _lineRenderer.SetPositions(pointsCopy);
    }

    public Rect GetRect()
    {
        return _screenRect;
    }
}
