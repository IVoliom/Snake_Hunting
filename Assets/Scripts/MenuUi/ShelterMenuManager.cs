using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShelterMenuManager : MonoBehaviour
{
    public Text playerNameText;
    public Text statsText;

    private PlayerData currentPlayer;

    void Start()
    {
        // ��������� �������� ������ (���������� �����������)
        currentPlayer = SaveData.LoadAllPlayers().Last();

        UpdateUI();
    }

    void UpdateUI()
    {
        playerNameText.text = currentPlayer.playerName;
        statsText.text = $"�������: {currentPlayer.snakeLevel}\n" +
                         $"����� �������: {currentPlayer.miceEaten}\n" +
                         $"�������� �������: {currentPlayer.successfulExits}\n" +
                         $"������: {currentPlayer.failedExits}";
    }

    public void StartHunting()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        // �������������� ��� ������
        SaveData.SavePlayer(currentPlayer);
    }
}