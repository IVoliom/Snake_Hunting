using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StaminaSystem _staminaSystem;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _staminaText;
    [SerializeField] private GameObject _cooldownOverlay;
    [SerializeField] private TMP_Text _cooldownText; // Добавляем текстовое поле для отображения таймера

    [Header("Settings")]
    [SerializeField] private float _maxStaminaUnits = 100f; // Максимальное количество единиц стамины
    [SerializeField] private float _staminaDrainPerSecond = 20f; // Единиц стамины в секунду при ускорении

    private float _cooldownDuration = 15f; // Длительность коулдауна
    private float _cooldownTimer = 0f;
    private bool _isOnCooldown = false;

    private void Awake()
    {
        if (_staminaSystem == null)
            _staminaSystem = FindAnyObjectByType<StaminaSystem>();

        // Инициализируем систему стамины с нашими значениями
        _staminaSystem.Initialize(_maxStaminaUnits, _staminaDrainPerSecond);

        _staminaSystem.OnStaminaChanged.AddListener(UpdateStaminaUI);
        _staminaSystem.OnStaminaDepleted.AddListener(StartCooldown);

        _cooldownOverlay.SetActive(false);
        if (_cooldownText != null) _cooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isOnCooldown)
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownText != null)
            {
                _cooldownText.text = Mathf.CeilToInt(_cooldownTimer).ToString();
            }

            if (_cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }
    }

    private void UpdateStaminaUI(float currentStamina)
    {
        _fillImage.fillAmount = currentStamina / _maxStaminaUnits;

        // Отображаем абсолютное значение стамины (округляем до целого)
        _staminaText.text = $"{Mathf.RoundToInt(currentStamina)}/{Mathf.RoundToInt(_maxStaminaUnits)}";

        // Скрываем коулдаун, если стамина начала восстанавливаться
        if (currentStamina > 0.1f && _cooldownOverlay.activeSelf && !_isOnCooldown)
        {
            _cooldownOverlay.SetActive(false);
            if (_cooldownText != null) _cooldownText.gameObject.SetActive(false);
        }
    }

    private void StartCooldown()
    {
        _isOnCooldown = true;
        _cooldownTimer = _cooldownDuration;
        _cooldownOverlay.SetActive(true);

        if (_cooldownText != null)
        {
            _cooldownText.gameObject.SetActive(true);
            _cooldownText.text = Mathf.CeilToInt(_cooldownTimer).ToString();
        }
    }

    private void EndCooldown()
    {
        _isOnCooldown = false;
        _cooldownOverlay.SetActive(false);
        if (_cooldownText != null) _cooldownText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _staminaSystem.OnStaminaChanged.RemoveListener(UpdateStaminaUI);
        _staminaSystem.OnStaminaDepleted.RemoveListener(StartCooldown);
    }
}