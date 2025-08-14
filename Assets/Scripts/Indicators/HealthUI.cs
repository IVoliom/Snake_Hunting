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

        // �������������� UI � �������� ����������
        UpdateHealthUI((float)_healthSystem.CurrentHealth / _healthSystem.MaxHealth);
    }

    private void UpdateHealthUI(float currentHealth)
    {
        // ��������� fillAmount
        _fillImage.fillAmount = currentHealth;

        // ��������� ��������� ��������
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