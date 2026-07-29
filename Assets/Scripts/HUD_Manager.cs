using UnityEngine;
using TMPro;

public class HUD_Manager : MonoBehaviour
{
    [Header("HUD Labels")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI hp;

    private void Update()
    {
        if (Game_Manager.Instance == null)
            return;

        score.text = $"SCORE: {Game_Manager.Instance.TotalScore}";

        float t = Game_Manager.Instance.SurvivalTime;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        time.text = $"TIME: {minutes:00}:{seconds:00}";

        hp.text = $"HP: {Game_Manager.Instance.PlayerHP} / {Game_Manager.Instance.PlayerMaxHP}";
    }
}
