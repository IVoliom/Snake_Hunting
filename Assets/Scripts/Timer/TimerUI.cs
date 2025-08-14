using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private RectTransform _timerPanel;

    private GameTimer _gameTimer;

    private void Awake()
    {
        _gameTimer = FindAnyObjectByType<GameTimer>();
        _gameTimer.OnTimerVisibilityChanged.AddListener(UpdateTimerVisibility);
        UpdateTimerVisibility();
    }

    private void Update()
    {
        if (_gameTimer.IsTimerRunning && _gameTimer.IsTimerVisible)
        {
            _timerText.text = _gameTimer.GetFormattedTime();
        }
    }

    private void UpdateTimerVisibility()
    {
        _timerPanel.gameObject.SetActive(_gameTimer.IsTimerVisible);
    }
}