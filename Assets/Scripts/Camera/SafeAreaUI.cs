using UnityEngine;

public class SafeAreaUI : MonoBehaviour
{
    void Start()
    {
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;

        float w = Screen.width;
        float h = Screen.height;

        Vector2 anchorMin = new Vector2(safeArea.x / w, safeArea.y / h);
        Vector2 anchorMax = new Vector2((safeArea.x + safeArea.width) / w, (safeArea.y + safeArea.height) / h);

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
