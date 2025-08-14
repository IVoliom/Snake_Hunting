using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameTimer : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerMovement _playerController;

    [Header("��������� �������")]
    [SerializeField] private float _gameDuration = 20f; // ����� ����� ���� � �������� (3 ������ �� ���������)
    [SerializeField] private bool _timerRunning = false; // ���� ������ �������

    [Header("��������� �����")]
    private Controls _controls;
    private InputAction _timerAction;

    [Header("�������")]
    public UnityEvent OnTimerStart; // ������� ������� �������
    public UnityEvent OnTimerEnd; // ������� ��������� �������
    public UnityEvent OnTimerVisibilityChanged; // ������� ��������� ��������� �������

    private float _currentTime; // ������� ���������� �����
    private string _previousSceneName; // ��� ���������� ����� (�������)
    private bool _isTimerVisible = false; // ��������� �������

    // �������� ��� ������� � ��������� �����
    public float CurrentTime => _currentTime; // ������� �����
    public bool IsTimerRunning => _timerRunning; // �������� �� ������
    public bool IsTimerVisible => _isTimerVisible; // ����� �� ������

    private void Awake()
    {
        // ������ ������ �������������� ��� �������� ����� ����
        DontDestroyOnLoad(gameObject);

        // �������������� ������� �����
        _controls = new Controls();
        _timerAction = _controls.Snake.Timer;

        // ������������� �� �������
        _timerAction.performed += ctx => ToggleTimerVisibility();

        // ������������� ���� PlayerMovement, ���� �� �������� � ����������
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
        // �������� ��������� �������� ����� ��� ��������� �������/����������
        // ��� ���������� ��� �������� �� ������� ����� (��������, ������� ������)
        _timerAction.Enable();
    }

    private void OnDisable()
    {
        // ��������� ��������� �������� ����� ��� ����������� �������/����������
        // ����� ��� �������������� ������ ������ � ��������� ����� ����� ������ ���������
        _timerAction.Disable();
    }

    private void OnDestroy()
    {
        // ����������� ������� ������� �����
        _controls?.Dispose();

        // ������������ �� ������� �����
        if (_timerAction != null)
        {
            _timerAction.performed -= ctx => ToggleTimerVisibility();
        }
    }

    private void Update()
    {
        // ���� ������ �� �������� - �������
        if (!_timerRunning) return;

        // ��������� ���������� �����
        _currentTime -= Time.deltaTime;

        // ��������� ��������� �������
        if (_currentTime <= 0f)
        {
            _currentTime = 0f; // �������� �����
            StopTimer(); // ������������� ������

            // ���������, ���������� �� _playerController
            if (_playerController != null)
            {
                _playerController.ExitFail();
            }
            else
            {
                Debug.LogError("PlayerController is null! Assign it in the inspector.");
            }

            // �������� ������� ��������� �������
            OnTimerEnd?.Invoke();

            // ��������� ����� ���������� ���������� ������
            //if (_playerController != null && !_playerController.IsLevelCompleted)
            //{
            //    _playerController.ExitFail();
            //}
        }
    }

    // ����� ������������ ��������� �������
    public void ToggleTimerVisibility()
    {
        _isTimerVisible = !_isTimerVisible; // ����������� ��������� ���������
        OnTimerVisibilityChanged?.Invoke(); // �������� ������� ��������� ���������
    }

    // ����� ������� �������
    public void StartTimer(float duration = 0f)
    {
        // ���� �������� ����� ����� - ���������� ���
        if (duration > 0f)
        {
            _gameDuration = duration;
        }

        _currentTime = _gameDuration; // ������������� ������� �����
        _timerRunning = true; // ��������� ������
        OnTimerStart?.Invoke(); // �������� ������� ������
    }

    // ����� ��������� �������
    public void StopTimer()
    {
        _timerRunning = false; // ������������� ������
    }

    // ����� �������������� ������� � ������ MM:SS
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60f); // �������� ������
        int seconds = Mathf.FloorToInt(_currentTime % 60f); // �������� �������
        return $"{minutes:00}:{seconds:00}"; // ���������� ��������������� ������
    }

    // ����� �������� � �������
    //public void ReturnToShelter()
    //{
    //    SceneManager.LoadScene(_previousSceneName); // ��������� ���������� �����
    //}

    public void SetPreviousScene(string sceneName)
    {
        _previousSceneName = sceneName;
    }
}