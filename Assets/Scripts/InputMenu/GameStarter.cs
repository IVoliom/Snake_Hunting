using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        // ������� GameTimer (�� ������ ���� DontDestroyOnLoad)
        GameTimer gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer != null)
        {
            // ��������� ������
            gameTimer.StartTimer();
        }
        else
        {
            Debug.LogError("GameTimer �� ������!");
        }
    }
}
