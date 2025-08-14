using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioClip slidingSound;

    #region References
    private Rigidbody2D _rb;
    #endregion

    #region Controls
    private Controls _controls;
    private InputAction _moveInput;
    private InputAction _boostInput;
    private Vector2 _moveDirection;
    #endregion

    [Header("Player Stats")]
    [SerializeField] private PlayerData currentPlayer;
    private int _miceEatenThisRound = 0;
    private bool _levelCompleted = false;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 25f;
    [SerializeField] private float _boostedSpeed = 100f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 20f;
    [SerializeField] private float _rotationSpeed = 3f;

    [Header("Systems")]
    [SerializeField] private StaminaSystem _staminaSystem;
    [SerializeField] private HealthSystem _healthSystem;

    [Header("Exit Settings")]
    [SerializeField] private float _exitTimeRequired = 5f; // Время в секундах для успешного выхода
    private float _currentExitTime = 0f;
    private bool _isInExitZone = false;

    private float _currentSpeedX = 0f;
    private float _currentSpeedY = 0f;
    private bool _isBoosting = false;
    private bool _isMoving = false;

    public bool IsLevelCompleted => _levelCompleted;

    [Header("Snake Body")]
    [SerializeField] private SnakeBody _snakeBody;

    // Экспортируемые данные для UI
    public int MiceEatenThisRound => _miceEatenThisRound;
    public int TotalMiceEaten => currentPlayer != null ? currentPlayer.miceEaten : 0;

    private void Awake()
    {
        _controls = new Controls();
        _moveInput = _controls.Snake.Movement;
        _boostInput = _controls.Snake.Boost;

        if (_staminaSystem == null) _staminaSystem = GetComponent<StaminaSystem>();
        if (_healthSystem == null) _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDeath.AddListener(OnDeath);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        //-----------------------------------------------------------------
        // Загружаем данные текущего игрока
        currentPlayer = SaveData.GetCurrentPlayer();

        if (currentPlayer == null)
        {
            Debug.LogError("No current player found!");
            // Создаём временного игрока, чтобы избежать ошибок
            currentPlayer = new PlayerData("TempPlayer");
        }
        //-----------------------------------------------------------------
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void OnDestroy()
    {
        // 1. Освобождаем систему ввода
        if (_controls != null)
        {
            _controls.Disable(); // Отключаем все действия
            _controls.Dispose(); // Освобождаем ресурсы
            _controls = null;    // Обнуляем ссылку
        }

        // 2. Безопасная отписка от событий
        if (_healthSystem != null)
        {
            _healthSystem.OnDeath.RemoveListener(OnDeath);
        }

        // 3. Дополнительная очистка (по необходимости)
        _moveInput?.Dispose();
        _boostInput?.Dispose();
    }

    private void Update()
    {
        _moveDirection = _moveInput.ReadValue<Vector2>();
        _isMoving = _moveDirection.magnitude > 0.1f; // Проверяем, есть ли ввод движения

        HandleInput();
        HandleStamina();
        HandleExitTimer();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float targetSpeed = _isBoosting ? _boostedSpeed : _walkSpeed;

        // Увеличиваем ускорение при бусте для более резкого старта
        float currentAcceleration = _isBoosting ? _acceleration * 2f : _acceleration;

        // Вычисляем движение
        _currentSpeedX = Mathf.Lerp(
            _currentSpeedX,
            _moveDirection.x * targetSpeed,
            (_moveDirection.x != 0 ? currentAcceleration : _deceleration) * Time.fixedDeltaTime
        );

        _currentSpeedY = Mathf.Lerp(
            _currentSpeedY,
            _moveDirection.y * targetSpeed,
            (_moveDirection.y != 0 ? currentAcceleration : _deceleration) * Time.fixedDeltaTime
        );

        // Применяем скорость
        _rb.linearVelocity = new Vector2(_currentSpeedX, _currentSpeedY);
    }

    void HandleInput()
    {
        // Получаем ввод с клавиатуры/геймпада
        Vector2 inputDirection = _moveInput.ReadValue<Vector2>().normalized;
        Quaternion targetRotation = transform.rotation;

        // Обработка направления движения для 8 направлений
        if (inputDirection.magnitude > 0.1f) // Если есть ввод
        {
            
            // Запрещаем разворот на 180°
            Vector2 newDirection = GetRoundedDirection(inputDirection);
            if (Vector2.Dot(_moveDirection.normalized, newDirection.normalized) > -0.5f)
            {
                _moveDirection = newDirection;

                // Определяем угол поворота для 8 направлений
                float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, 0, angle - 90); // -90 для корректного поворота "головы"
            }
        }

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * _rotationSpeed
        );

        // Ускорение (если есть выносливость и нет кулдауна)
        _isBoosting = _boostInput.IsPressed() &&
                     _isMoving &&
                     _staminaSystem.CanUseStamina;

        
    }

    private void HandleStamina()
    {
        if (_isBoosting)
        {
            _staminaSystem.StartDraining();
        }
        else
        {
            _staminaSystem.StopDraining();
        }
    }

    // Функция для определения ближайшего из 8 направлений
    private Vector2 GetRoundedDirection(Vector2 inputDirection)
    {
        // Определяем угол ввода (от -180 до 180)
        float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

        // Определяем ближайшее направление из 8 возможных (каждые 45 градусов)
        angle = Mathf.Round(angle / 45f) * 45f;

        // Возвращаем соответствующее направление
        return Quaternion.Euler(0, 0, angle) * Vector2.right;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Food"))
        {
            Destroy(coll.gameObject);
            AddMiceEaten();
        }
        else if (coll.CompareTag("Exit"))
        {
            //if (!_levelCompleted) // Чтобы не засчитывало несколько раз
            //{
            //    _levelCompleted = true;
            //    ExitSuccess();
            //}
            _isInExitZone = true;
            _currentExitTime = 0f; // Сбрасываем таймер при каждом новом входе
            
            Debug.Log("Entered exit zone");
        }
        else if (!coll.CompareTag("Untagged")) // Игнорируем объекты без тега
        {
            if (!_levelCompleted)
            {
                _healthSystem.TakeDamage(1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Exit"))
        {
            _isInExitZone = false;
            _currentExitTime = 0f; // Полностью сбрасываем прогресс при выходе
            Debug.Log("Left exit zone");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall collision detected");
            _healthSystem.TakeDamage(1);
            Debug.Log($"Health after damage: {_healthSystem.CurrentHealth}");

            // Для коллизии используем точку контакта
            //Vector2 pushDirection = (transform.position - coll.transform.position).normalized;
            //Debug.Log($"Push direction: {pushDirection}");
            //_rb.AddForce(pushDirection * 50f, ForceMode2D.Impulse);
        }
    }

    public void AddMiceEaten()
    {
        _miceEatenThisRound++;
        // Общее число мышей будет увеличено только при успешном выходе через нору
        Debug.Log($"Съедено за раунд: {_miceEatenThisRound}");

        // Увеличиваем тело змеи каждые 5 мышей (пример: рост на 1 сегмент)
        //if (_snakeBody != null && _miceEatenThisRound % 5 == 0)
        //{
        //    _snakeBody.Grow(1);
        //}
    }

    public void ExitSuccess()
    {
        if (currentPlayer != null)
        {
            currentPlayer.hasExitedThisRound = true;
            currentPlayer.successfulExits++;
            currentPlayer.lastPlayTime = DateTime.Now;

            // Переносим раундовые мыши в общий счёт
            currentPlayer.miceEaten += _miceEatenThisRound;
            _miceEatenThisRound = 0;

            // Попытка повышения уровня
            var progress = new SnakeProgress();
            progress.TryLevelUp(currentPlayer);

            SaveData.SavePlayer(currentPlayer);
        }

        SceneManager.LoadScene("Success");
    }

    public void ExitFail()
    {
        if (currentPlayer != null)
        {
            currentPlayer.failedExits++;
            currentPlayer.lastPlayTime = DateTime.Now;
            // Не трогаем currentPlayer.miceEaten
            SaveData.SavePlayer(currentPlayer);
        }

        _miceEatenThisRound = 0; // сбрасываем счёт за раунд
        SceneManager.LoadScene("GameOver");
    }

    private void OnDeath()
    {
        if (!_levelCompleted)
        {
            _levelCompleted = true;
            ExitFail();
        }
    }
    private void HandleExitTimer()
    {
        if (_isInExitZone && !_levelCompleted)
        {
            _currentExitTime += Time.deltaTime;

            //
            Debug.Log($"Exit progress: {_currentExitTime / _exitTimeRequired * 100f}%");

            if (_currentExitTime >= _exitTimeRequired)
            {
                _levelCompleted = true;
                ExitSuccess();
            }
        }
        else
        {
            _currentExitTime = 0f; // Сбрасываем таймер, если игрок покинул зону
        }
    }
}