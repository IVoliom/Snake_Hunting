using TMPro;
using UnityEngine;

public class SnakeUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TMP_Text totalMiceText;
    [SerializeField] private TMP_Text roundMiceText;

    private void Update()
    {
        if (playerMovement != null)
        {
            if (totalMiceText != null)
                totalMiceText.text = $"—ъедено мышей: {playerMovement.TotalMiceEaten}";

            if (roundMiceText != null)
                roundMiceText.text = $": {playerMovement.MiceEatenThisRound}";
        }
    }
}
