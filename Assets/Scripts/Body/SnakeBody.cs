using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private GameObject bodySegmentPrefab;

    [Header("Body Settings")]
    [SerializeField] private int initialSize = 5;
    [SerializeField] private float segmentDistance = 0.6f; // ���������� ����� ����������
    [SerializeField] private float pointSpacing = 0.15f;   // ����������� ���������� ������ ����� ������� ����������
    [SerializeField] private int baseSortingOrder = 1000;    // ������� ������� ���������

    private List<Transform> bodySegments = new List<Transform>();
    private List<Vector3> headPath = new List<Vector3>(); // ���������� ������

    private void Start()
    {
        if (headTransform == null)
        {
            Debug.LogError("SnakeBody: Head Transform is not assigned!");
            return;
        }
        // ������� ��������� ���� ������ ������
        for (int i = 0; i < initialSize; i++)
        {
            Vector3 behind = headTransform.position - headTransform.up * segmentDistance * (i + 1);
            GameObject seg = Instantiate(bodySegmentPrefab, behind, Quaternion.identity, transform);
            bodySegments.Add(seg.transform);
            SetSegmentSorting(seg, i + 1);
        }

        // ������������� ���������� ������ ���, ����� ��������� ����� ���� ����� �������,
        // � ���������� � �� ���������� segmentDistance ���� �� �����.
        headPath.Clear();
        for (int i = initialSize; i >= 1; i--)
        {
            headPath.Add(headTransform.position - headTransform.up * segmentDistance * i);
        }

        // ��������� ���������� ������
        headPath.Add(headTransform.position);
    }

    // ���������� ������ ������ (���� �����)
    public void InitializeHead(Transform head)
    {
        headTransform = head;
    }

    // ���� ����
    public void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 behind;
            if (bodySegments.Count > 0)
            {
                // ������� �� ��������� ���������
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
        // ��������� ����������, ����� ����� �������� ����� ����� �����
        RebuildHeadPath();
    }

    // ���������� ���������� ����� �����
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

        // ���������� ���������� ������
        Vector3 headPos = headTransform.position;
        if (headPath.Count == 0 || Vector3.Distance(headPos, headPath[headPath.Count - 1]) > pointSpacing)
        {
            headPath.Add(headPos);
        }

        // ������������ ������ ����������
        int maxPathPoints = (bodySegments.Count + 2) * 4;
        if (headPath.Count > maxPathPoints)
            headPath.RemoveAt(0);
    }

    private void LateUpdate()
    {
        if (headTransform == null || headPath.Count < 2) return;

        // ��������� ������ ������� �� ������ ����� ����������
        for (int i = 0; i < bodySegments.Count; i++)
        {
            float dist = (i + 1) * segmentDistance;
            Vector3 pos = GetPointAtDistanceFromHead(headPath, headPath.Count - 1, dist);
            bodySegments[i].position = pos;

            // �������� ������� � ������ ����������� (���� �� ����� � ������)
            Vector3 dir = (headTransform.position - bodySegments[i].position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bodySegments[i].rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            SetSegmentSorting(bodySegments[i].gameObject, i + 1);
        }
    }

    // �������� ����� �� ���������� �� ���������� distance �� ������
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
            // ��� ������ index, ��� ���� ����, ����� ������ ���� ������
            sr.sortingOrder = baseSortingOrder - index;
        }
    }
}