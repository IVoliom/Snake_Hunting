using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialogUI : MonoBehaviour
{
    //[SerializeField] private RectTransform _pausePanel;


    //[SerializeField] private TMP_Text messageText; // или TMP_Text, если используете TextMeshPro
    //[SerializeField] private Button yesButton;
    //[SerializeField] private Button noButton;

    //private Action onYes;
    //private Action onNo;

    //private void Awake()
    //{
    //    if (yesButton != null) yesButton.onClick.AddListener(OnYes);
    //    if (noButton != null) noButton.onClick.AddListener(OnNo);
    //    gameObject.SetActive(false); // скрыто по умолчанию
    //}

    //public void Show(string message, Action onYesAction, Action onNoAction = null)
    //{
    //    if (messageText != null) messageText.text = message;
    //    onYes = onYesAction;
    //    onNo = onNoAction;
    //    gameObject.SetActive(true);
    //}

    //private void OnYes()
    //{
    //    onYes?.Invoke();
    //    Close();
    //}

    //private void OnNo()
    //{
    //    onNo?.Invoke();
    //    Close();
    //}

    //private void Close()
    //{
    //    gameObject.SetActive(false);
    //    onYes = null;
    //    onNo = null;
    //}
}
