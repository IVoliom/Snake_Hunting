using UnityEngine;
using UnityEngine.Events;

public class StaminaSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float _maxStamina;
    [SerializeField] private float _staminaDrainRate;
    [SerializeField] private float _staminaRegenRate = 3f;

    [Header("Events")]
    public UnityEvent<float> OnStaminaChanged; // float - текущее значение stamina (0-1)
    public UnityEvent OnStaminaDepleted;

    private float _currentStamina;
    private float _staminaCooldownTimer = 0f;
    private bool _isDraining = false;

    public bool CanUseStamina => _currentStamina > 0 && _staminaCooldownTimer <= 0;

    public void Initialize(float maxStamina, float drainRate)
    {
        _maxStamina = maxStamina;
        _staminaDrainRate = drainRate;
        _currentStamina = _maxStamina;
    }

    private void Awake()
    {
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        if (_isDraining && CanUseStamina)
        {
            _currentStamina -= _staminaDrainRate * Time.deltaTime;
            OnStaminaChanged?.Invoke(_currentStamina);

            if (_currentStamina <= 0)
            {
                _currentStamina = 0;
                _staminaCooldownTimer = 15f;
                OnStaminaDepleted?.Invoke();
            }
        }
        else if (!_isDraining && _staminaCooldownTimer <= 0)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _maxStamina);
            OnStaminaChanged?.Invoke(_currentStamina);
        }

        if (_staminaCooldownTimer > 0)
        {
            _staminaCooldownTimer -= Time.deltaTime;
        }
    }

    public void StartDraining()
    {
        if (CanUseStamina) _isDraining = true;
    }

    public void StopDraining()
    {
        _isDraining = false;
    }
}