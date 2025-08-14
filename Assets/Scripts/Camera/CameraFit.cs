using UnityEngine;

public class CameraFit : MonoBehaviour
{
    public float targetWorldHeight = 10f; // видимая высота в мире
    public float targetWorldWidth = 18f;  // и ширина

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (cam == null) return;
        float aspect = (float)Screen.width / Screen.height;
        float sizeForHeight = targetWorldHeight * 0.5f;
        float sizeForWidth = (targetWorldWidth * 0.5f) / aspect;
        cam.orthographicSize = Mathf.Max(sizeForHeight, sizeForWidth);
    }
}
