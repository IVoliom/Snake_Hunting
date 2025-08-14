using UnityEngine;

public class CameraFitAndLock1920x1080 : MonoBehaviour
{
    // �������� ������� ���� ����
    public float targetWorldHeight = 10f; // ������ � world units
    public float targetWorldWidth = 18f; // ������ � world units

    private Camera _cam;
    private int _lastWidth;
    private int _lastHeight;

    void Awake()
    {
        // ������������� ���������� ���������� 1920x1080
        Screen.SetResolution(1920, 1080, true);
        _cam = GetComponent<Camera>();
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        // ��������� ��������
        AdjustCamera();
    }

    void Update()
    {
        // ���� ���������� ����������, ��������� ������ �����
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            AdjustCamera();
        }

        // ������� ��������� ���������������� �������
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
        // � ���� ������� �� ������������ � Update ����� ������ ������ �������.
        // ����� ������������� ��������� ��������� ������ �����, ���� ��������.
    }
}
