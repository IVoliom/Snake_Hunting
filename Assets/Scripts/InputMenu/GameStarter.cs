using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        // Находим GameTimer (он должен быть DontDestroyOnLoad)
        GameTimer gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer != null)
        {
            // Запускаем таймер
            gameTimer.StartTimer();
        }
        else
        {
            Debug.LogError("GameTimer не найден!");
        }
    }
}
