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
    [SerializeField] private TMP_Text levelProgressPctText; // Optional: ������� ���������

    private PlayerData currentPlayer;

    void Start()
    {
        LoadPlayer();
        UpdateUI();
    }

    // ��������� �������� ������ �� SaveData
    void LoadPlayer()
    {
        currentPlayer = SaveData.GetCurrentPlayer();
        if (currentPlayer == null)
        {
            // ���� ���������� ��� ��� � ������ ����������� ���
            currentPlayer = new PlayerData("TempPlayer");
        }
    }

    // ��������� ��� UI-��������
    public void UpdateUI()
    {
        if (playerNameText != null) playerNameText.text = $"������� ������: {currentPlayer.playerName}";
        if (levelText != null) levelText.text = $"������� ����: {currentPlayer.snakeLevel}";
        if (totalMiceText != null) totalMiceText.text = $"����� ������� ��������: {currentPlayer.miceEaten}";

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

    // ������� ���� �����, ���� ����� �������� UI ����� ����� ������ ��� ����������
    public void RefreshFromSave()
    {
        LoadPlayer();
        UpdateUI();
    }
}