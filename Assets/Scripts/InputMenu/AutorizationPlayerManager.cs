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
    public GameObject continuePanel; // ������ �� ������� ������� ContinuePanel
    public Transform playersListParent; // Content �� ScrollView
    public GameObject playerButtonPrefab; // ������ PlayerButton

    [Header("Delete Confirmation")]
    [SerializeField] private GameObject deleteConfirmPanel;
    [SerializeField] private TMP_Text deleteConfirmText;
    private string _playerToDelete;

    // ������� ��������� �����
    private PlayerData currentPlayer;

    void Start()
    {
        DebugSavedPlayers(); //
        //newGameButton.onClick.AddListener(StartNewGame);
        //continueButton.onClick.AddListener(ShowContinueMenu);

        //// ���������, ���� �� ���������� ������
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
            Debug.Log("��� ������ ��������� ������� 3 �������!");
            return;
        }

        // ���������, ���������� �� ��� ����� � ����� ������
        if (SaveData.LoadAllPlayers().Exists(p => p.playerName == playerName))
        {
            Debug.Log("����� � ����� ������ ��� ����������!");
            return;
        }

        // ������ ������ ������
        PlayerData newPlayer = new PlayerData(playerName);
        SaveData.SavePlayer(newPlayer);
        SaveData.SetCurrentPlayer(newPlayer);

        SceneManager.LoadScene("Shelter");
    }

    public void ShowContinueMenu()
    {
        continuePanel.SetActive(true);

        // ������� ���������� ������
        foreach (Transform child in playersListParent)
        {
            Destroy(child.gameObject);
        }

        // ��������� ���� ����������� �������
        List<PlayerData> players = SaveData.LoadAllPlayers();
        Debug.Log($"��������� �������: {players.Count}");

        // ���� ��� ����������
        if (players.Count == 0)
        {
            GameObject noDataText = new GameObject("NoDataText");
            noDataText.transform.SetParent(playersListParent);
            TextMeshProUGUI text = noDataText.AddComponent<TextMeshProUGUI>();
            text.text = "��� ����������� �������";
            text.alignment = TextAlignmentOptions.Center;
            return;
        }

        // ������� ������ ��� ������� ������
        foreach (PlayerData player in players)
        {
            GameObject button = Instantiate(playerButtonPrefab, playersListParent);
            //    button.GetComponentInChildren<TextMeshProUGUI>().text =
            //        $"{player.playerName} (�������: {player.snakeLevel})";

            //    // ��������� ���������� �������
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
        deleteConfirmText.text = $"������� ���������� {playerName}?";
        deleteConfirmPanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        if (!string.IsNullOrEmpty(_playerToDelete) || string.IsNullOrEmpty(_playerToDelete))
        {
            SaveData.DeletePlayer(_playerToDelete);
            deleteConfirmPanel.SetActive(false);
            ShowContinueMenu(); // ��������� ������
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
        // ��������, ��� �� ��������� ��� ������ ������, � �� ������ ������
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
        Debug.Log($"������� �������: {players.Count}");
        foreach (var player in players)
        {
            Debug.Log($"�����: {player.playerName}, �������: {player.snakeLevel}");
        }
    }
}
