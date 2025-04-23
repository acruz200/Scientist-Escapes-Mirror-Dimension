using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [Header("UI Elements")]
    private GameObject gameOverPanel;
    private Button restartButton;
    private Button mainMenuButton;
    
    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private string mainMenuSceneName = "Start Screen";
    
    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            // Make this persist across scene loads
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }
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

        // Ensure an EventSystem exists for UI interaction
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
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
            
            // --- Add Credits --- 

            // Create Team Names Text (Centered)
            GameObject teamNamesTextObj = new GameObject("Team Names Text");
            teamNamesTextObj.transform.SetParent(gameOverPanel.transform, false);
            TextMeshProUGUI teamNamesText = teamNamesTextObj.AddComponent<TextMeshProUGUI>();
            teamNamesText.text = "<b>Team</b>\nJunaid Ali\nAlex Cruz\nSyed Ali";
            teamNamesText.fontSize = 24; // Restore original size
            teamNamesText.alignment = TextAlignmentOptions.Center; // Restore center alignment
            teamNamesText.color = Color.white;
            RectTransform teamNamesRect = teamNamesTextObj.GetComponent<RectTransform>();
            // Restore original anchor/position
            teamNamesRect.anchorMin = new Vector2(0.5f, 0.2f); 
            teamNamesRect.anchorMax = new Vector2(0.5f, 0.2f);
            teamNamesRect.pivot = new Vector2(0.5f, 0.5f); // Default center pivot
            teamNamesRect.sizeDelta = new Vector2(600, 80); // Restore original size
            teamNamesRect.anchoredPosition = new Vector2(0, -20); // Restore original position

            // Create Course Info Text (Centered)
            GameObject courseInfoTextObj = new GameObject("Course Info Text");
            courseInfoTextObj.transform.SetParent(gameOverPanel.transform, false);
            TextMeshProUGUI courseInfoText = courseInfoTextObj.AddComponent<TextMeshProUGUI>();
            courseInfoText.text = "Developed at UIC as part of CS426 Videogame Design";
            courseInfoText.fontSize = 18; // Restore original size
            courseInfoText.alignment = TextAlignmentOptions.Center; // Restore center alignment
            courseInfoText.color = Color.gray;
            RectTransform courseInfoRect = courseInfoTextObj.GetComponent<RectTransform>();
            // Restore original anchor/position
            courseInfoRect.anchorMin = new Vector2(0.5f, 0.1f); 
            courseInfoRect.anchorMax = new Vector2(0.5f, 0.1f);
            courseInfoRect.pivot = new Vector2(0.5f, 0.5f); // Default center pivot
            courseInfoRect.sizeDelta = new Vector2(700, 30); // Restore original size
            courseInfoRect.anchoredPosition = new Vector2(0, -40); // Restore original position

            // Create Asset Credits Text (Right Side)
            GameObject assetCreditsTextObj = new GameObject("Asset Credits Text");
            assetCreditsTextObj.transform.SetParent(gameOverPanel.transform, false);
            TextMeshProUGUI assetCreditsText = assetCreditsTextObj.AddComponent<TextMeshProUGUI>();
            
            // Format the credits string (Keep this updated content)
            string credits = "<b>Asset Credits</b>\n\n" +
                             "<b>Animations/Models:</b>\n" +
                             "Mixamo\n" +
                             "Prisoner B Styperek\n" +
                             "Vanguard By T. Choonyung @Taunt\n\n" +
                             "<b>Unity Asset Store:</b>\n" +
                             "Chemistry Lab Items Pack\n" +
                             "Low Poly Cartoon House Interiors\n" +
                             "FREE Trees\n" +
                             "LeanTween\n\n" +
                             "<b>Audio (from Pixabay):</b>\n" +
                             "Open Door Sound (247415)\n" +
                             "Switch Sound (150130)\n" +
                             "Lego Walking (208360)\n" +
                             "Monster Growl (251374)";

            assetCreditsText.text = credits;
            assetCreditsText.fontSize = 32; // Doubled font size
            assetCreditsText.alignment = TextAlignmentOptions.TopLeft; // Align text to the left within its own box
            assetCreditsText.color = Color.gray;
            RectTransform assetCreditsRect = assetCreditsTextObj.GetComponent<RectTransform>();
            // Anchor slightly higher up (Y=0.4), still near the right edge (X=0.95)
            assetCreditsRect.anchorMin = new Vector2(0.95f, 0.4f); 
            assetCreditsRect.anchorMax = new Vector2(0.95f, 0.4f);
            assetCreditsRect.pivot = new Vector2(1f, 0f); // Pivot at bottom-right
            assetCreditsRect.sizeDelta = new Vector2(350, 450); // Significantly increased height for much larger font
            assetCreditsRect.anchoredPosition = Vector2.zero; // Position relative to anchor

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