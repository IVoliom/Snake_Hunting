using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button selectButton;

    //private PlayerData playerData;
    private PlayerData _playerData;
    private AutorizationPlayerManager autorizationManager;

    public void Setup(PlayerData player, AutorizationPlayerManager manager)
    {
        _playerData = player; // ��������� ������
        autorizationManager = manager;

        playerNameText.text = $"{player.playerName} (�������: {player.snakeLevel})";

        // ��������� �����������
        deleteButton.onClick.AddListener(OnDeleteClicked);
        selectButton.onClick.AddListener(OnContinueClick);
    }

    private void OnDeleteClicked()
    {
        autorizationManager.ShowDeleteConfirmation(_playerData);
    }

    public void OnContinueClick()
    {
        autorizationManager.TryLoadPlayer(_playerData);
    }
}
