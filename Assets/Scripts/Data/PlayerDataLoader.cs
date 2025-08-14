using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    private void Start()
    {
        // ��������� �������� ������ ��� ������ �����
        PlayerData currentPlayer = SaveData.GetCurrentPlayer();

        if (currentPlayer != null)
        {
            Debug.Log($"�������� �����: {currentPlayer.playerName}");
            // ����� ����� �������� UI ��� ������ �������� ����
        }
        else
        {
            Debug.Log("������� ����� �� ������. ����� ������� ������.");
        }
    }
}
