using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void onGameStart()
    {
        // Сохраняем ссылку на GameTimer перед сменой сцены
        GameTimer gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer != null)
        {
            // Сохраняем текущую сцену как Shelter
            gameTimer.SetPreviousScene(SceneManager.GetActiveScene().name);
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void onShelterLoad()
    {
        SceneManager.LoadScene("Shelter");
    }

    public void onMainMenuLoad()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            // Если в редакторе - останавливаем проигрывание
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // В собранной версии - закрываем приложение
             Application.Quit();
        #endif

        Debug.Log("Игра завершена"); // Для проверки в консоли
    }
}
