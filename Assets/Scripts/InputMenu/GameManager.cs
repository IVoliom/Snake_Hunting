using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void onGameStart()
    {
        // ��������� ������ �� GameTimer ����� ������ �����
        GameTimer gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer != null)
        {
            // ��������� ������� ����� ��� Shelter
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
            // ���� � ��������� - ������������� ������������
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // � ��������� ������ - ��������� ����������
             Application.Quit();
        #endif

        Debug.Log("���� ���������"); // ��� �������� � �������
    }
}
