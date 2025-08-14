using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private int _maxMice = 100;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnDelay = 2f;
    [SerializeField] private float _spawnRadius = 1f; // ������ ������ ������

    [Header("References")]
    [SerializeField] private Tilemap _wallTilemap; // ������ �� Tilemap ����

    private GameObject[] _bushes;

    private void Awake()
    {
        _bushes = GameObject.FindGameObjectsWithTag("Bush");
        if (_wallTilemap == null)
            _wallTilemap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMouse), _spawnDelay, _spawnInterval);

    }

    private void SpawnMouse()
    {
        if (GameObject.FindGameObjectsWithTag("Food").Length >= _maxMice || _bushes.Length == 0)
            return;

        // �������� ��������� ����
        GameObject bush = _bushes[Random.Range(0, _bushes.Length)];
        Vector3 spawnPos = GetValidSpawnPosition(bush.transform.position);

        if (spawnPos != Vector3.zero)
            Instantiate(_mousePrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetValidSpawnPosition(Vector3 bushPos)
    {
        int attempts = 0;
        while (attempts < 50)
        {
            // ��������� ����� � ������� ������ �����
            Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPos = bushPos + new Vector3(randomCircle.x, randomCircle.y, 0);

            // ���������, ��� ������� �� ������ �����
            if (!IsWallTile(spawnPos))
                return spawnPos;

            attempts++;
        }
        return Vector3.zero; // ���� �� ����� �������� �������
    }

    private bool IsWallTile(Vector3 position)
    {
        // ������������ ������� ���������� � ���������� ��������
        Vector3Int cellPosition = _wallTilemap.WorldToCell(position);
        return _wallTilemap.HasTile(cellPosition);
    }

    private void OnDrawGizmosSelected()
    {
        if (_bushes == null || _bushes.Length == 0) return;

        Gizmos.color = Color.green;
        foreach (var bush in _bushes)
        {
            if (bush != null)
                Gizmos.DrawWireSphere(bush.transform.position, _spawnRadius);
        }
    }


}