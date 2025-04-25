using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int pointsPerZombieKill = 10;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string scorePrefix = "Score: ";
    
    private void Awake()
    {
        // Singleton pattern implementation
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Initialize score display
        UpdateScoreDisplay();
    }
    
    /// <summary>
    /// Add points to player score (call this when a zombie is killed)
    /// </summary>
    public void AddZombieKillPoints()
    {
        AddPoints(pointsPerZombieKill);
    }
    
    /// <summary>
    /// Add custom number of points to the score
    /// </summary>
    /// <param name="points">Number of points to add</param>
    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }
    
    /// <summary>
    /// Reset score to zero
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }
    
    /// <summary>
    /// Get current score value
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }
    
    /// <summary>
    /// Update score text UI
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + currentScore.ToString();
        }
    }
    
    /// <summary>
    /// Set the TextMeshProUGUI component to display score
    /// </summary>
    /// <param name="text">TextMeshProUGUI component to use for score display</param>
    public void SetScoreText(TextMeshProUGUI text)
    {
        scoreText = text;
        UpdateScoreDisplay();
    }
} 