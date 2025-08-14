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
        // Загружаем текущего игрока (последнего сохранённого)
        currentPlayer = SaveData.LoadAllPlayers().Last();

        UpdateUI();
    }

    void UpdateUI()
    {
        playerNameText.text = currentPlayer.playerName;
        statsText.text = $"Уровень: {currentPlayer.snakeLevel}\n" +
                         $"Мышей съедено: {currentPlayer.miceEaten}\n" +
                         $"Успешных выходов: {currentPlayer.successfulExits}\n" +
                         $"Неудач: {currentPlayer.failedExits}";
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
        // Автосохранение при выходе
        SaveData.SavePlayer(currentPlayer);
    }
}