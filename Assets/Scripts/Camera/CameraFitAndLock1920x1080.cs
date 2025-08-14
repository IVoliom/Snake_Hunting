using UnityEngine;

public class CameraFitAndLock1920x1080 : MonoBehaviour
{
    // Желаемая видимая зона мира
    public float targetWorldHeight = 10f; // высота в world units
    public float targetWorldWidth = 18f; // ширина в world units

    private Camera _cam;
    private int _lastWidth;
    private int _lastHeight;

    void Awake()
    {
        // Принудительно установить разрешение 1920x1080
        Screen.SetResolution(1920, 1080, true);
        _cam = GetComponent<Camera>();
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        // Первичная подгонка
        AdjustCamera();
    }

    void Update()
    {
        // Если разрешение поменялось, подстроим камеру снова
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            AdjustCamera();
        }

        // Плавное изменение ортографического размера
        float aspect = (float)Screen.width / Screen.height;
        float sizeForHeight = targetWorldHeight * 0.5f;
        float sizeForWidth = (targetWorldWidth * 0.5f) / aspect;
        float desiredSize = Mathf.Max(sizeForHeight, sizeForWidth);
        if (_cam != null)
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, desiredSize, Time.deltaTime * 4f);
        }
    }

    private void AdjustCamera()
    {
        // В этом примере всё регулируется в Update через прямой расчёт величин.
        // Можно дополнительно сохранить отдельную логику здесь, если захотите.
    }
}
