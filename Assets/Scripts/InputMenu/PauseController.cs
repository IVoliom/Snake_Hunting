using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }
    [Header("UI Pause Overlay")]
    [SerializeField] private GameObject pausePanel;      

    private bool isPaused = false;

    // Input System
    private Controls _controls;
    private InputAction _pauseAction;

    private CanvasGroup _pauseCanvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        _controls = new Controls();
        _pauseAction = _controls.Snake.Pause;
        _pauseAction.performed += OnPausePerformed;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindUI();          // перепривязать UI в новой сцене
        PauseGame(false);    // снять паузу после загрузки

        // настройка курсора под сцену
        ShowCursor(!(scene.name == "Menu" || scene.name == "Shelter" || scene.name == "GameOver" || scene.name == "Success"));
    }

    private void BindUI()
    {
        // Привязать PausePanel, если он не привязан в текущей сцене
        if (pausePanel == null)
        {
            var found = GameObject.Find("PauseOverlayPanel");
            if (found != null) pausePanel = found;
        }

        if (pausePanel != null)
        {
            // Обеспечить наличие CanvasGroup для управления интерактивностью
            var cg = pausePanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = pausePanel.AddComponent<CanvasGroup>();
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        // Привязка кнопок
        if (pausePanel != null)
        {
            foreach (var btn in pausePanel.GetComponentsInChildren<UnityEngine.UI.Button>(true))
            {
                btn.onClick.RemoveAllListeners();
                if (btn.name == "Continue")
                    btn.onClick.AddListener(OnContinue);
                else if (btn.name == "Yes_Menu")
                    btn.onClick.AddListener(OnMainMenu);
                else if (btn.name == "Yes_Shelter")
                    btn.onClick.AddListener(OnShelter);
            }
        }
    }

    private void ShowCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _controls?.Enable();
        _pauseAction?.Enable();
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    private void OnDisable()
    {
        if (_pauseAction != null) _pauseAction.performed -= OnPausePerformed;
        _pauseAction?.Disable();
        _controls?.Disable();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_pauseAction != null) _pauseAction.performed -= OnPausePerformed;
        _pauseAction?.Disable();
        _controls?.Dispose();
        _controls = null;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        PauseGame(!isPaused);
    }

    public void PauseGame(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        if (pausePanel != null) pausePanel.SetActive(pause);

        // курсор
        ShowCursor(pause);

        // через CanvasGroup отключаем/включаем интерактивность
        if (pausePanel != null)
        {
            var cg = pausePanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = pause;
                cg.blocksRaycasts = pause;
            }
        }
    }

    // Продолжить
    public void OnContinue()
    {
        PauseGame(false);
    }

    // Главное меню
    public void OnMainMenu()
    {
        PauseGame(false);
        SceneManager.LoadScene("Menu");
    }

    // Убежище
    public void OnShelter()
    {
        PauseGame(false);
        SceneManager.LoadScene("Shelter");
    }
}