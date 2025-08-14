using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth = 100;

    [Header("Events")]
    public UnityEvent<float> OnHealthChanged; // int - текущее здоровье
    public UnityEvent OnDeath;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        OnHealthChanged?.Invoke((float)_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    //public void Heal(int amount)
    //{
    //    _currentHealth += amount;
    //    _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
    //    OnHealthChanged?.Invoke((float)_currentHealth / _maxHealth);
    //}
}