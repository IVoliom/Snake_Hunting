using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameTimer : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerMovement _playerController;

    [Header("Настройки таймера")]
    [SerializeField] private float _gameDuration = 20f; // Общее время игры в секундах (3 минуты по умолчанию)
    [SerializeField] private bool _timerRunning = false; // Флаг работы таймера

    [Header("Настройки ввода")]
    private Controls _controls;
    private InputAction _timerAction;

    [Header("События")]
    public UnityEvent OnTimerStart; // Событие запуска таймера
    public UnityEvent OnTimerEnd; // Событие окончания времени
    public UnityEvent OnTimerVisibilityChanged; // Событие изменения видимости таймера

    private float _currentTime; // Текущее оставшееся время
    private string _previousSceneName; // Имя предыдущей сцены (убежища)
    private bool _isTimerVisible = false; // Видимость таймера

    // Свойства для доступа к приватным полям
    public float CurrentTime => _currentTime; // Текущее время
    public bool IsTimerRunning => _timerRunning; // Работает ли таймер
    public bool IsTimerVisible => _isTimerVisible; // Видим ли таймер

    private void Awake()
    {
        // Делаем объект неуничтожаемым при загрузке новых сцен
        DontDestroyOnLoad(gameObject);

        // Инициализируем систему ввода
        _controls = new Controls();
        _timerAction = _controls.Snake.Timer;

        // Подписываемся на событие
        _timerAction.performed += ctx => ToggleTimerVisibility();

        // Автоматически ищем PlayerMovement, если не назначен в инспекторе
        if (_playerController == null)
        {
            _playerController = FindAnyObjectByType<PlayerMovement>();
            if (_playerController == null)
            {
                Debug.LogError("PlayerMovement not found in the scene!");
            }
        }
    }

    private void OnEnable()
    {
        // Включаем обработку действий ввода при активации объекта/компонента
        // Это необходимо для подписки на события ввода (например, нажатие клавиш)
        _timerAction.Enable();
    }

    private void OnDisable()
    {
        // Отключаем обработку действий ввода при деактивации объекта/компонента
        // Важно для предотвращения утечек памяти и обработки ввода когда объект неактивен
        _timerAction.Disable();
    }

    private void OnDestroy()
    {
        // Освобождаем ресурсы системы ввода
        _controls?.Dispose();

        // Отписываемся от событий ввода
        if (_timerAction != null)
        {
            _timerAction.performed -= ctx => ToggleTimerVisibility();
        }
    }

    private void Update()
    {
        // Если таймер не работает - выходим
        if (!_timerRunning) return;

        // Уменьшаем оставшееся время
        _currentTime -= Time.deltaTime;

        // Проверяем окончание времени
        if (_currentTime <= 0f)
        {
            _currentTime = 0f; // Обнуляем время
            StopTimer(); // Останавливаем таймер

            // Проверяем, существует ли _playerController
            if (_playerController != null)
            {
                _playerController.ExitFail();
            }
            else
            {
                Debug.LogError("PlayerController is null! Assign it in the inspector.");
            }

            // Вызываем событие окончания времени
            OnTimerEnd?.Invoke();

            // Добавляем вызов неудачного завершения уровня
            //if (_playerController != null && !_playerController.IsLevelCompleted)
            //{
            //    _playerController.ExitFail();
            //}
        }
    }

    // Метод переключения видимости таймера
    public void ToggleTimerVisibility()
    {
        _isTimerVisible = !_isTimerVisible; // Инвертируем состояние видимости
        OnTimerVisibilityChanged?.Invoke(); // Вызываем событие изменения видимости
    }

    // Метод запуска таймера
    public void StartTimer(float duration = 0f)
    {
        // Если передано новое время - используем его
        if (duration > 0f)
        {
            _gameDuration = duration;
        }

        _currentTime = _gameDuration; // Устанавливаем текущее время
        _timerRunning = true; // Запускаем таймер
        OnTimerStart?.Invoke(); // Вызываем событие старта
    }

    // Метод остановки таймера
    public void StopTimer()
    {
        _timerRunning = false; // Останавливаем таймер
    }

    // Метод форматирования времени в строку MM:SS
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60f); // Получаем минуты
        int seconds = Mathf.FloorToInt(_currentTime % 60f); // Получаем секунды
        return $"{minutes:00}:{seconds:00}"; // Возвращаем форматированную строку
    }

    // Метод возврата в убежище
    //public void ReturnToShelter()
    //{
    //    SceneManager.LoadScene(_previousSceneName); // Загружаем предыдущую сцену
    //}

    public void SetPreviousScene(string sceneName)
    {
        _previousSceneName = sceneName;
    }
}