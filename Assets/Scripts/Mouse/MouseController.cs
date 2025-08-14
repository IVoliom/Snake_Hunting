using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 25f;
    [SerializeField] private float _directionChangeInterval = 3f;
    [SerializeField] private float _wallCheckDistance = 1f;

    [SerializeField] private LayerMask wallLayer; // Слой для стен

    private Tilemap _wallTilemap;
    private Rigidbody2D _rb;
    private Vector2 _currentDirection;
    private float _directionTimer;

    private Animator animator;
    private float movementTimer;
    private bool isMoving = true;

    [SerializeField] private float obstacleAvoidForce = 2f; // Сила избегания препятствий
    private bool isEscapingWall; // Флаг - убегает ли от стены

    private void Awake()
    {
        // Получаем компоненты при создании
        _rb = GetComponent<Rigidbody2D>();

        // Находим тайлмап стен по тегу
        _wallTilemap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();


        // Настройки для плавной физики:
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Для точного обнаружения столкновений
        _rb.interpolation = RigidbodyInterpolation2D.Extrapolate; // Сглаживание движения

        //ChangeDirection();
    }

    private void Start()
    {
        ChangeDirection();

        //wallLayer = LayerMask.GetMask("Wall"); // Убедитесь, что стены на этом слое
    }

    private void Update()
    {
        // Обновляем таймер
        _directionTimer += Time.deltaTime;

        // Если пришло время - меняем направление
        if (_directionTimer >= _directionChangeInterval)
        {
            ChangeDirection();
            _directionTimer = 0; // Сбрасываем таймер
        }
    }

    private void FixedUpdate()
    {
        //Move();

        // Получаем силу для избегания препятствий
        Vector2 avoidanceForce = GetObstacleAvoidanceForce();
        // Рассчитываем итоговое направление с учетом избегания
        Vector2 desiredDirection = (_currentDirection + avoidanceForce).normalized;
        // Применяем движение через физический движок
        _rb.linearVelocity = desiredDirection * _moveSpeed;
    }

    private void Move()
    {
        transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        // 8 направлений (каждые 45 градусов)
        float angle = Random.Range(0, 8) * 45f;
        _currentDirection = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Если столкнулись со стеной
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Рассчитываем направление отскока (зеркальное отражение)
            _currentDirection = Vector2.Reflect(
                _currentDirection,
                collision.contacts[0].normal
            ).normalized;

            isEscapingWall = true; // Активируем режим побега
        }
    }

    // Вычисляет силу для избегания стен
    private Vector2 GetObstacleAvoidanceForce()
    {
        Vector2 force = Vector2.zero; // Начальная сила
        isEscapingWall = false; // Сбрасываем флаг

        // Проверяем 4 основных направления (север, восток, юг, запад)
        for (int i = 0; i < 8; i++)
        {
            // Вычисляем направление проверки (0°, 90°, 180°, 270°)
            float angle = i * 45f;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            // Если в этом направлении есть стена
            if (IsWallNearby(dir))
            {
                // Добавляем силу в противоположном направлении
                force += -dir * obstacleAvoidForce;
                isEscapingWall = true; // Активируем режим побега
            }
        }

        return force;
    }

    // Проверяет наличие стены в указанном направлении
    private bool IsWallNearby(Vector2 direction)
    {
        // Переводим мировые координаты в координаты тайлмапа
        Vector3Int cellPosition = _wallTilemap.WorldToCell(
            transform.position + (Vector3)direction * _wallCheckDistance
        );
        // Проверяем есть ли тайл стены в этой клетке
        return _wallTilemap.HasTile(cellPosition);
    }

    //private bool IsWallAhead()
    //{
    //    Vector2 checkPosition = (Vector2)transform.position + _currentDirection * _wallCheckDistance;
    //    Vector3Int cellPosition = _wallTilemap.WorldToCell(checkPosition);
    //    return _wallTilemap.HasTile(cellPosition);
    //}

    //void UpdateAnimation()
    //{
    //    if (_currentDirection == Vector2.zero)
    //    {
    //        animator.Play("Mouse_Idle");
    //        return;
    //    }

    //    float angle = Mathf.Atan2(_currentDirection.y, _currentDirection.x) * Mathf.Rad2Deg;
    //    if (angle < 0) angle += 360;

    //    if (angle >= 22.5f && angle < 67.5f)
    //    {
    //        animator.Play("MouseAnimationsWD");
    //    }
    //    else if (angle >= 67.5f && angle < 112.5f)
    //    {
    //        animator.Play("MouseAnimationsW");
    //    }
    //    else if (angle >= 112.5f && angle < 157.5f)
    //    {
    //        animator.Play("MouseAnimationsAW");
    //    }
    //    else if (angle >= 157.5f && angle < 202.5f)
    //    {
    //        animator.Play("MouseAnimationsA");
    //    }
    //    else if (angle >= 202.5f && angle < 247.5f)
    //    {
    //        animator.Play("MouseAnimationsAS");
    //    }
    //    else if (angle >= 247.5f && angle < 292.5f)
    //    {
    //        animator.Play("MouseAnimationsS");
    //    }
    //    else if (angle >= 292.5f && angle < 337.5f)
    //    {
    //        animator.Play("MouseAnimationsSD");
    //    }
    //    else
    //    {
    //        animator.Play("MouseAnimationsD");
    //    }
    //}

}
