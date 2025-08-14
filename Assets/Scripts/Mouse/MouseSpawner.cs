using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private int _maxMice = 100;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnDelay = 2f;
    [SerializeField] private float _spawnRadius = 1f; // Радиус вокруг кустов

    [Header("References")]
    [SerializeField] private Tilemap _wallTilemap; // Ссылка на Tilemap стен

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

        // Выбираем случайный куст
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
            // Случайная точка в радиусе вокруг куста
            Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPos = bushPos + new Vector3(randomCircle.x, randomCircle.y, 0);

            // Проверяем, что позиция не внутри стены
            if (!IsWallTile(spawnPos))
                return spawnPos;

            attempts++;
        }
        return Vector3.zero; // Если не нашли валидную позицию
    }

    private bool IsWallTile(Vector3 position)
    {
        // Конвертируем мировые координаты в координаты тайлмапа
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