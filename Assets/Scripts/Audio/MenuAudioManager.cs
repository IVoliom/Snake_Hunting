using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;

    public AudioSource audioSource;
    public string[] menuScenes = { "Menu", "GameOver", "Shelter", "Success"};

    private bool isFadingOut = false;
    private float fadeDuration = 1f;
    private float fadeTimer = 0f;
    private float originalVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            originalVolume = audioSource.volume;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(originalVolume, 0f, fadeTimer / fadeDuration);

            if (fadeTimer >= fadeDuration)
            {
                audioSource.Stop();
                isFadingOut = false;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isMenuScene = System.Array.Exists(menuScenes, x => x == scene.name);

        if (isMenuScene)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = originalVolume;
                audioSource.Play();
            }
        }
        else
        {
            StartFadeOut();
        }
    }

    public void StartFadeOut()
    {
        if (audioSource.isPlaying)
        {
            isFadingOut = true;
            fadeTimer = 0f;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
