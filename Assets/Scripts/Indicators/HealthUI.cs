using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _healthText;

    private void Awake()
    {
        if (_healthSystem == null)
            _healthSystem = FindAnyObjectByType<HealthSystem>();

        _healthSystem.OnHealthChanged.AddListener(UpdateHealthUI);

        // Инициализируем UI с текущими значениями
        UpdateHealthUI((float)_healthSystem.CurrentHealth / _healthSystem.MaxHealth);
    }

    private void UpdateHealthUI(float currentHealth)
    {
        // Обновляем fillAmount
        _fillImage.fillAmount = currentHealth;

        // Обновляем текстовое значение
        _healthText.text = $"{Mathf.RoundToInt(currentHealth * _healthSystem.MaxHealth)}/{_healthSystem.MaxHealth}";

        //for (int i = 0; i < _healthHearts.Length; i++)
        //{
        //    _healthHearts[i].enabled = i < currentHealth;
        //}
    }

    private void OnDestroy()
    {
        if (_healthSystem != null)
            _healthSystem.OnHealthChanged.RemoveListener(UpdateHealthUI);
    }
}