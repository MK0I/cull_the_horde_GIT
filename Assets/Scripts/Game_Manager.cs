using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    // Timer and Score
    public float SurvivalTime { get; private set; }
    public int EnemyKills { get; private set; }
    public int TotalScore { get; private set; }

    // HP
    public int PlayerMaxHP { get; private set; } = 5;
    public int PlayerHP { get; private set; }

    // Difficulty Scaling
    [Header("Difficulty Scaling (per minute survived)")]
    [SerializeField] private float speedScalePerMinute = 0.10f;
    [SerializeField] private float hpScalePerMinute = 0.20f;

    public float EnemySpeedMultiplier { get; private set; } = 1f;
    public float EnemyHPMultiplier { get; private set; } = 1f;

    // Music
    [Header("Music")]
    [SerializeField] private AudioClip level1Track;
    [SerializeField] private AudioClip level2Track;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    private AudioSource audioSource;

    private float scoreTimer;
    private bool trackingScore = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        ResetScore();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayTrackForCurrentScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayTrackForCurrentScene();
    }

    private void PlayTrackForCurrentScene()
    {
        if (audioSource == null)
            return;

        int index = SceneManager.GetActiveScene().buildIndex;
        AudioClip clip = index == 0 ? level1Track : level2Track;

        if (clip == null)
            return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (!trackingScore)
            return;

        SurvivalTime += Time.deltaTime;

        // Survival Score System
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            scoreTimer -= 1f;
            TotalScore++;
        }

        // Difficulty Scaling
        float minutesSurvived = SurvivalTime / 30f;
        EnemySpeedMultiplier = 1f + (speedScalePerMinute * minutesSurvived);
        EnemyHPMultiplier = 1f + (hpScalePerMinute * minutesSurvived);
    }

    // HP Tracker
    public void SetPlayerHP(int current, int max)
    {
        PlayerHP = current;
        PlayerMaxHP = max;
    }

    public void ReportPlayerDeath()
    {
        StopScoring();
    }

    // Kill Tracker
    public void AddEnemyKill()
    {
        EnemyKills++;
        TotalScore += 100;
    }

    // Stopper
    public void StopScoring()
    {
        trackingScore = false;
    }

    public void ResetScore()
    {
        SurvivalTime = 0f;
        EnemyKills = 0;
        TotalScore = 0;
        scoreTimer = 0f;
        trackingScore = true;
        PlayerHP = PlayerMaxHP;
        EnemySpeedMultiplier = 1f;
        EnemyHPMultiplier = 1f;
    }

    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No next scene found.");
            return;
        }

        SceneManager.LoadScene(nextScene);
    }
}