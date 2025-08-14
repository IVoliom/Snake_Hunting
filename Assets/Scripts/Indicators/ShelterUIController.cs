using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text totalMiceText;
    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private TMP_Text levelProgressPctText; // Optional: процент прогресса

    private PlayerData currentPlayer;

    void Start()
    {
        LoadPlayer();
        UpdateUI();
    }

    // Загружаем текущего игрока из SaveData
    void LoadPlayer()
    {
        currentPlayer = SaveData.GetCurrentPlayer();
        if (currentPlayer == null)
        {
            // Если сохранения ещё нет — создаём минимальный мок
            currentPlayer = new PlayerData("TempPlayer");
        }
    }

    // Обновляем все UI-элементы
    public void UpdateUI()
    {
        if (playerNameText != null) playerNameText.text = $"Убежище игрока: {currentPlayer.playerName}";
        if (levelText != null) levelText.text = $"Уровень змеи: {currentPlayer.snakeLevel}";
        if (totalMiceText != null) totalMiceText.text = $"Всего съедено грызунов: {currentPlayer.miceEaten}";

        int requiredMice = Mathf.Max(1, currentPlayer.snakeLevel * 5);
        float t = Mathf.Clamp01((float)currentPlayer.miceEaten / requiredMice);

        if (levelProgressSlider != null)
        {
            levelProgressSlider.value = t;
        }

        if (levelProgressPctText != null)
        {
            levelProgressPctText.text = Mathf.RoundToInt(t * 100f) + "%";
        }
    }

    // Вызвать этот метод, если нужно обновить UI после смены игрока или сохранения
    public void RefreshFromSave()
    {
        LoadPlayer();
        UpdateUI();
    }
}