using TMPro;
using UnityEngine;

public class End_UI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text killsText;

    private void Start()
    {
        if (Game_Manager.Instance == null)
            return;

        scoreText.text = $"Score\n{Game_Manager.Instance.TotalScore}";

        timeText.text =
            $"Time Survived\n{Game_Manager.Instance.SurvivalTime:F0}s";

        killsText.text =
            $"Enemies Defeated\n{Game_Manager.Instance.EnemyKills}";
    }
}