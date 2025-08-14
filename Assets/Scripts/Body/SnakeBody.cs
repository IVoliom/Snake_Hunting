using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private GameObject bodySegmentPrefab;

    [Header("Body Settings")]
    [SerializeField] private int initialSize = 5;
    [SerializeField] private float segmentDistance = 0.6f; // рассто€ние между сегментами
    [SerializeField] private float pointSpacing = 0.15f;   // минимальное рассто€ние головы между точками траектории
    [SerializeField] private int baseSortingOrder = 1000;    // базовый пор€док отрисовки

    private List<Transform> bodySegments = new List<Transform>();
    private List<Vector3> headPath = new List<Vector3>(); // траектори€ головы

    private void Start()
    {
        if (headTransform == null)
        {
            Debug.LogError("SnakeBody: Head Transform is not assigned!");
            return;
        }
        // —оздать начальное тело позади головы
        for (int i = 0; i < initialSize; i++)
        {
            Vector3 behind = headTransform.position - headTransform.up * segmentDistance * (i + 1);
            GameObject seg = Instantiate(bodySegmentPrefab, behind, Quaternion.identity, transform);
            bodySegments.Add(seg.transform);
            SetSegmentSorting(seg, i + 1);
        }

        // ѕредзаполн€ем траекторию головы так, чтобы последн€€ точка была самой головой,
        // а предыдущие Ч на рассто€нии segmentDistance друг от друга.
        headPath.Clear();
        for (int i = initialSize; i >= 1; i--)
        {
            headPath.Add(headTransform.position - headTransform.up * segmentDistance * i);
        }

        // Ќачальна€ траектори€ головы
        headPath.Add(headTransform.position);
    }

    // ѕрограммно задать голову (если нужно)
    public void InitializeHead(Transform head)
    {
        headTransform = head;
    }

    // –ост тела
    public void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 behind;
            if (bodySegments.Count > 0)
            {
                // позици€ за последним сегментом
                Transform last = bodySegments[bodySegments.Count - 1];
                behind = last.position - headTransform.up * segmentDistance;
            }
            else
            {
                behind = headTransform.position - headTransform.up * segmentDistance;
            }
            GameObject seg = Instantiate(bodySegmentPrefab, behind, Quaternion.identity, transform);
            bodySegments.Add(seg.transform);
            SetSegmentSorting(seg, bodySegments.Count);
        }
        // ќбновл€ем траекторию, чтобы новые сегменты сразу нашли место
        RebuildHeadPath();
    }

    // ќбновление траектории после роста
    private void RebuildHeadPath()
    {
        headPath.Clear();
        for (int i = bodySegments.Count; i >= 1; i--)
        {
            headPath.Add(headTransform.position - headTransform.up * segmentDistance * i);
        }
        headPath.Add(headTransform.position);
    }

    private void Update()
    {
        if (headTransform == null) return;

        // «аписываем траекторию головы
        Vector3 headPos = headTransform.position;
        if (headPath.Count == 0 || Vector3.Distance(headPos, headPath[headPath.Count - 1]) > pointSpacing)
        {
            headPath.Add(headPos);
        }

        // ќграничиваем размер траектории
        int maxPathPoints = (bodySegments.Count + 2) * 4;
        if (headPath.Count > maxPathPoints)
            headPath.RemoveAt(0);
    }

    private void LateUpdate()
    {
        if (headTransform == null || headPath.Count < 2) return;

        // –азмещаем каждый сегмент на нужной точке траектории
        for (int i = 0; i < bodySegments.Count; i++)
        {
            float dist = (i + 1) * segmentDistance;
            Vector3 pos = GetPointAtDistanceFromHead(headPath, headPath.Count - 1, dist);
            bodySegments[i].position = pos;

            // ѕриводим сегмент в нужное направление (хот€ бы ближе к голове)
            Vector3 dir = (headTransform.position - bodySegments[i].position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bodySegments[i].rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            SetSegmentSorting(bodySegments[i].gameObject, i + 1);
        }
    }

    // ѕолучить точку на траектории на рассто€нии distance от головы
    private Vector3 GetPointAtDistanceFromHead(List<Vector3> path, int headIndex, float distance)
    {
        if (path == null || path.Count < 2) return headTransform.position;

        float remaining = distance;
        int idx = headIndex;

        while (idx > 0)
        {
            float segDist = Vector3.Distance(path[idx], path[idx - 1]);
            if (remaining <= segDist)
            {
                float t = remaining / segDist;
                return Vector3.Lerp(path[idx], path[idx - 1], t);
            }
            else
            {
                remaining -= segDist;
                idx--;
            }
        }

        return path[0];
    }

    private void SetSegmentSorting(GameObject seg, int index)
    {
        var sr = seg.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // „ем больше index, тем ниже слой, чтобы голова была сверху
            sr.sortingOrder = baseSortingOrder - index;
        }
    }
}