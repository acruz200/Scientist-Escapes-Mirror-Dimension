using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    private GameObject gameOverPanel;
    private Button restartButton;
    private Button mainMenuButton;
    
    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private string mainMenuSceneName = "Start Screen";
    
    private void Awake()
    {
        // Make this persist across scene loads
        DontDestroyOnLoad(gameObject);
    }
    
    public void ShowGameOver()
    {
        // Create UI if it doesn't exist
        CreateGameOverUI();
        
        // Show game over panel with fade in effect
        StartCoroutine(FadeInGameOver());
    }
    
    private void CreateGameOverUI()
    {
        // Find or create canvas
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("UI Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create game over panel
        if (gameOverPanel == null)
        {
            // Create panel
            gameOverPanel = new GameObject("Game Over Panel");
            gameOverPanel.transform.SetParent(mainCanvas.transform, false);
            
            // Add background image
            Image panelImage = gameOverPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);
            
            // Set panel to fill the entire screen
            RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            
            // Create "Game Over" text
            GameObject gameOverTextObj = new GameObject("Game Over Text");
            gameOverTextObj.transform.SetParent(gameOverPanel.transform, false);
            
            TextMeshProUGUI gameOverText = gameOverTextObj.AddComponent<TextMeshProUGUI>();
            gameOverText.text = "GAME OVER";
            gameOverText.fontSize = 72;
            gameOverText.alignment = TextAlignmentOptions.Center;
            gameOverText.color = Color.red;
            
            RectTransform textRect = gameOverTextObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.7f);
            textRect.anchorMax = new Vector2(0.5f, 0.7f);
            textRect.sizeDelta = new Vector2(600, 100);
            textRect.anchoredPosition = Vector2.zero;
            
            // Create restart button
            GameObject restartButtonObj = new GameObject("Restart Button");
            restartButtonObj.transform.SetParent(gameOverPanel.transform, false);
            
            restartButton = restartButtonObj.AddComponent<Button>();
            Image restartButtonImage = restartButtonObj.AddComponent<Image>();
            restartButtonImage.color = new Color(0.2f, 0.2f, 0.2f);
            
            RectTransform restartButtonRect = restartButtonObj.GetComponent<RectTransform>();
            restartButtonRect.anchorMin = new Vector2(0.5f, 0.4f);
            restartButtonRect.anchorMax = new Vector2(0.5f, 0.4f);
            restartButtonRect.sizeDelta = new Vector2(300, 60);
            restartButtonRect.anchoredPosition = Vector2.zero;
            
            // Add text to restart button
            GameObject restartTextObj = new GameObject("Restart Text");
            restartTextObj.transform.SetParent(restartButtonObj.transform, false);
            
            TextMeshProUGUI restartText = restartTextObj.AddComponent<TextMeshProUGUI>();
            restartText.text = "Restart";
            restartText.fontSize = 36;
            restartText.alignment = TextAlignmentOptions.Center;
            restartText.color = Color.white;
            
            RectTransform restartTextRect = restartTextObj.GetComponent<RectTransform>();
            restartTextRect.anchorMin = Vector2.zero;
            restartTextRect.anchorMax = Vector2.one;
            restartTextRect.offsetMin = Vector2.zero;
            restartTextRect.offsetMax = Vector2.zero;
            
            // Add restart button functionality
            restartButton.onClick.AddListener(RestartGame);
            
            // Create main menu button
            GameObject menuButtonObj = new GameObject("Main Menu Button");
            menuButtonObj.transform.SetParent(gameOverPanel.transform, false);
            
            mainMenuButton = menuButtonObj.AddComponent<Button>();
            Image menuButtonImage = menuButtonObj.AddComponent<Image>();
            menuButtonImage.color = new Color(0.2f, 0.2f, 0.2f);
            
            RectTransform menuButtonRect = menuButtonObj.GetComponent<RectTransform>();
            menuButtonRect.anchorMin = new Vector2(0.5f, 0.3f);
            menuButtonRect.anchorMax = new Vector2(0.5f, 0.3f);
            menuButtonRect.sizeDelta = new Vector2(300, 60);
            menuButtonRect.anchoredPosition = Vector2.zero;
            
            // Add text to main menu button
            GameObject menuTextObj = new GameObject("Menu Text");
            menuTextObj.transform.SetParent(menuButtonObj.transform, false);
            
            TextMeshProUGUI menuText = menuTextObj.AddComponent<TextMeshProUGUI>();
            menuText.text = "Main Menu";
            menuText.fontSize = 36;
            menuText.alignment = TextAlignmentOptions.Center;
            menuText.color = Color.white;
            
            RectTransform menuTextRect = menuTextObj.GetComponent<RectTransform>();
            menuTextRect.anchorMin = Vector2.zero;
            menuTextRect.anchorMax = Vector2.one;
            menuTextRect.offsetMin = Vector2.zero;
            menuTextRect.offsetMax = Vector2.zero;
            
            // Add main menu button functionality
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            
            // Initially hide the panel
            gameOverPanel.SetActive(false);
        }
    }
    
    private IEnumerator FadeInGameOver()
    {
        gameOverPanel.SetActive(true);
        
        // Get panel image and all child UI elements
        Image panelImage = gameOverPanel.GetComponent<Image>();
        Graphic[] childGraphics = gameOverPanel.GetComponentsInChildren<Graphic>();
        
        // Set initial alpha to 0
        Color panelColor = panelImage.color;
        panelColor.a = 0;
        panelImage.color = panelColor;
        
        foreach (Graphic graphic in childGraphics)
        {
            Color color = graphic.color;
            color.a = 0;
            graphic.color = color;
        }
        
        // Fade in over time
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            
            // Update panel alpha
            panelColor.a = alpha;
            panelImage.color = panelColor;
            
            // Update all child elements
            foreach (Graphic graphic in childGraphics)
            {
                Color color = graphic.color;
                color.a = alpha;
                graphic.color = color;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure full opacity at the end
        panelColor.a = 1;
        panelImage.color = panelColor;
        
        foreach (Graphic graphic in childGraphics)
        {
            Color color = graphic.color;
            color.a = 1;
            graphic.color = color;
        }
    }
    
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Destroy this game over manager
        Destroy(gameObject);
    }
    
    public void GoToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
        
        // Destroy this game over manager
        Destroy(gameObject);
    }
} 