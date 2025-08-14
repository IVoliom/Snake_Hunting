using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutorizationPlayerManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    //public Button newGameButton;
    //public Button continueButton;

    [Header("Continue Menu")]
    public GameObject continuePanel; // Панель со списком игроков ContinuePanel
    public Transform playersListParent; // Content из ScrollView
    public GameObject playerButtonPrefab; // префаб PlayerButton

    [Header("Delete Confirmation")]
    [SerializeField] private GameObject deleteConfirmPanel;
    [SerializeField] private TMP_Text deleteConfirmText;
    private string _playerToDelete;

    // Текущий выбранный игрок
    private PlayerData currentPlayer;

    void Start()
    {
        DebugSavedPlayers(); //
        //newGameButton.onClick.AddListener(StartNewGame);
        //continueButton.onClick.AddListener(ShowContinueMenu);

        //// Проверяем, есть ли сохранённые игроки
        //if (PlayerPrefs.HasKey("SavedPlayers"))
        //{
        //    continueButton.interactable = true;
        //}
    }

    public void StartNewGame()
    {
        string playerName = nameInput.text;

        if (string.IsNullOrEmpty(playerName) || playerName.Length < 3)
        {
            Debug.Log("Имя должно содержать минимум 3 символа!");
            return;
        }

        // Проверяем, существует ли уже игрок с таким именем
        if (SaveData.LoadAllPlayers().Exists(p => p.playerName == playerName))
        {
            Debug.Log("Игрок с таким именем уже существует!");
            return;
        }

        // Создаём нового игрока
        PlayerData newPlayer = new PlayerData(playerName);
        SaveData.SavePlayer(newPlayer);
        SaveData.SetCurrentPlayer(newPlayer);

        SceneManager.LoadScene("Shelter");
    }

    public void ShowContinueMenu()
    {
        continuePanel.SetActive(true);

        // Очищаем предыдущие кнопки
        foreach (Transform child in playersListParent)
        {
            Destroy(child.gameObject);
        }

        // Загружаем всех сохраненных игроков
        List<PlayerData> players = SaveData.LoadAllPlayers();
        Debug.Log($"Загружено игроков: {players.Count}");

        // Если нет сохранений
        if (players.Count == 0)
        {
            GameObject noDataText = new GameObject("NoDataText");
            noDataText.transform.SetParent(playersListParent);
            TextMeshProUGUI text = noDataText.AddComponent<TextMeshProUGUI>();
            text.text = "Нет сохраненных игроков";
            text.alignment = TextAlignmentOptions.Center;
            return;
        }

        // Создаем кнопки для каждого игрока
        foreach (PlayerData player in players)
        {
            GameObject button = Instantiate(playerButtonPrefab, playersListParent);
            //    button.GetComponentInChildren<TextMeshProUGUI>().text =
            //        $"{player.playerName} (Уровень: {player.snakeLevel})";

            //    // Добавляем обработчик нажатия
            //    button.GetComponent<Button>().onClick.AddListener(() =>
            //    {
            //        TryLoadPlayer(player);
            //    });
            //}
            var controller = button.GetComponent<PlayerButton>();
            controller.Setup(player, this);
        }
    }

    public void ShowDeleteConfirmation(PlayerData playerName)
    {
        _playerToDelete = playerName.playerName;
        deleteConfirmText.text = $"Удалить сохранение {playerName}?";
        deleteConfirmPanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        if (!string.IsNullOrEmpty(_playerToDelete) || string.IsNullOrEmpty(_playerToDelete))
        {
            SaveData.DeletePlayer(_playerToDelete);
            deleteConfirmPanel.SetActive(false);
            ShowContinueMenu(); // Обновляем список
        }
    }

    public void CancelDelete()
    {
        deleteConfirmPanel.SetActive(false);
    }

    public void TryLoadPlayer(PlayerData player)
    {
        //SaveData.SetCurrentPlayer(player);

        //-------------------------------------------------------------
        // Убедимся, что мы сохраняем все данные игрока, а не только ссылку
        SaveData.SetCurrentPlayer(new PlayerData(player.playerName)
        {
            snakeLevel = player.snakeLevel,
            miceEaten = player.miceEaten,
            successfulExits = player.successfulExits,
            failedExits = player.failedExits,
            lastPlayTime = player.lastPlayTime
        });
        //----------------------------------------------------------------

        SceneManager.LoadScene("Shelter");

    }

    void DebugSavedPlayers()
    {
        List<PlayerData> players = SaveData.LoadAllPlayers();
        Debug.Log($"Найдено игроков: {players.Count}");
        foreach (var player in players)
        {
            Debug.Log($"Игрок: {player.playerName}, Уровень: {player.snakeLevel}");
        }
    }
}
