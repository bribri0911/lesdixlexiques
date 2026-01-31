using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class CameraBounds : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private Camera cam;

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        UpdateCameraBounds();
    }

    public void UpdateCameraBounds()
    {
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));

        Vector2[] points = new Vector2[5];
        points[0] = bottomLeft;
        points[1] = topLeft;
        points[2] = topRight;
        points[3] = bottomRight;
        points[4] = bottomLeft; 

        edgeCollider.points = points;
    }

    void Update()
    {
        #if UNITY_EDITOR
        UpdateCameraBounds(); 
        #endif
    }
}