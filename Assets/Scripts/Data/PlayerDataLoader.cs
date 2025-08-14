using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    private void Start()
    {
        // Загружаем текущего игрока при старте сцены
        PlayerData currentPlayer = SaveData.GetCurrentPlayer();

        if (currentPlayer != null)
        {
            Debug.Log($"Загружен игрок: {currentPlayer.playerName}");
            // Здесь можно обновить UI или другие элементы игры
        }
        else
        {
            Debug.Log("Текущий игрок не найден. Нужно создать нового.");
        }
    }
}
