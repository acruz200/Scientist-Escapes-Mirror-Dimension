using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactionPromptPanel;
    [SerializeField] private TextMeshProUGUI interactionPromptText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [Header("Health Bar")]
    [SerializeField] private GameObject healthBarPanel;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Damage Flash")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f);
    
    [Header("Score Display")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Awake()
    {
        // Create UI components if they don't exist
        CreateUIComponents();
    }
    
    private void CreateUIComponents()
    {
        // Create Canvas if it doesn't exist
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("UI Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Add required components
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create interaction prompt panel if it doesn't exist
        if (interactionPromptPanel == null)
        {
            // Create prompt panel
            interactionPromptPanel = new GameObject("Interaction Prompt Panel");
            interactionPromptPanel.transform.SetParent(mainCanvas.transform, false);
            
            // Add components
            Image promptPanelImage = interactionPromptPanel.AddComponent<Image>();
            promptPanelImage.color = new Color(0, 0, 0, 0.5f);
            
            // Set panel position and size
            RectTransform promptPanelRect = interactionPromptPanel.GetComponent<RectTransform>();
            promptPanelRect.anchorMin = new Vector2(0.5f, 0);
            promptPanelRect.anchorMax = new Vector2(0.5f, 0);
            promptPanelRect.pivot = new Vector2(0.5f, 0);
            promptPanelRect.anchoredPosition = new Vector2(0, 50);
            promptPanelRect.sizeDelta = new Vector2(400, 50);
            
            // Create prompt text
            GameObject promptTextObj = new GameObject("Prompt Text");
            promptTextObj.transform.SetParent(interactionPromptPanel.transform, false);
            
            interactionPromptText = promptTextObj.AddComponent<TextMeshProUGUI>();
            interactionPromptText.alignment = TextAlignmentOptions.Center;
            interactionPromptText.fontSize = 24;
            interactionPromptText.color = Color.white;
            
            RectTransform promptTextRect = promptTextObj.GetComponent<RectTransform>();
            promptTextRect.anchorMin = Vector2.zero;
            promptTextRect.anchorMax = Vector2.one;
            promptTextRect.sizeDelta = Vector2.zero;
            promptTextRect.offsetMin = new Vector2(10, 5);
            promptTextRect.offsetMax = new Vector2(-10, -5);
        }
        
        // Create dialogue panel if it doesn't exist
        if (dialoguePanel == null)
        {
            // Create dialogue panel
            dialoguePanel = new GameObject("Dialogue Panel");
            dialoguePanel.transform.SetParent(mainCanvas.transform, false);
            
            // Add components
            Image dialoguePanelImage = dialoguePanel.AddComponent<Image>();
            dialoguePanelImage.color = new Color(0, 0, 0, 0.8f);
            
            // Set panel position and size
            RectTransform dialoguePanelRect = dialoguePanel.GetComponent<RectTransform>();
            dialoguePanelRect.anchorMin = new Vector2(0.5f, 0);
            dialoguePanelRect.anchorMax = new Vector2(0.5f, 0);
            dialoguePanelRect.pivot = new Vector2(0.5f, 0);
            dialoguePanelRect.anchoredPosition = new Vector2(0, 150);
            dialoguePanelRect.sizeDelta = new Vector2(800, 100);
            
            // Create dialogue text
            GameObject dialogueTextObj = new GameObject("Dialogue Text");
            dialogueTextObj.transform.SetParent(dialoguePanel.transform, false);
            
            dialogueText = dialogueTextObj.AddComponent<TextMeshProUGUI>();
            dialogueText.alignment = TextAlignmentOptions.Center;
            dialogueText.fontSize = 28;
            dialogueText.color = Color.white;
            
            RectTransform dialogueTextRect = dialogueTextObj.GetComponent<RectTransform>();
            dialogueTextRect.anchorMin = Vector2.zero;
            dialogueTextRect.anchorMax = Vector2.one;
            dialogueTextRect.sizeDelta = Vector2.zero;
            dialogueTextRect.offsetMin = new Vector2(20, 10);
            dialogueTextRect.offsetMax = new Vector2(-20, -10);
        }
        
        // Create health bar panel if it doesn't exist
        if (healthBarPanel == null)
        {
            // Create health bar panel
            healthBarPanel = new GameObject("Health Bar Panel");
            healthBarPanel.transform.SetParent(mainCanvas.transform, false);
            
            // Add components
            Image healthPanelImage = healthBarPanel.AddComponent<Image>();
            healthPanelImage.color = new Color(0, 0, 0, 0.5f);
            
            // Set panel position and size
            RectTransform healthPanelRect = healthBarPanel.GetComponent<RectTransform>();
            healthPanelRect.anchorMin = new Vector2(0, 1);
            healthPanelRect.anchorMax = new Vector2(0, 1);
            healthPanelRect.pivot = new Vector2(0, 1);
            healthPanelRect.anchoredPosition = new Vector2(10, -10);
            healthPanelRect.sizeDelta = new Vector2(300, 40);
            
            // Create health bar background
            GameObject healthBarBgObj = new GameObject("Health Bar Background");
            healthBarBgObj.transform.SetParent(healthBarPanel.transform, false);
            
            Image healthBarBg = healthBarBgObj.AddComponent<Image>();
            healthBarBg.color = Color.gray;
            
            RectTransform healthBarBgRect = healthBarBgObj.GetComponent<RectTransform>();
            healthBarBgRect.anchorMin = new Vector2(0, 0.5f);
            healthBarBgRect.anchorMax = new Vector2(1, 0.5f);
            healthBarBgRect.pivot = new Vector2(0.5f, 0.5f);
            healthBarBgRect.anchoredPosition = new Vector2(0, 0);
            healthBarBgRect.sizeDelta = new Vector2(-20, 20);
            
            // Create health bar fill
            GameObject healthBarFillObj = new GameObject("Health Bar Fill");
            healthBarFillObj.transform.SetParent(healthBarBgObj.transform, false);
            
            healthBarFill = healthBarFillObj.AddComponent<Image>();
            healthBarFill.color = Color.green;
            
            RectTransform healthBarFillRect = healthBarFillObj.GetComponent<RectTransform>();
            healthBarFillRect.anchorMin = Vector2.zero;
            healthBarFillRect.anchorMax = Vector2.one;
            healthBarFillRect.pivot = new Vector2(0, 0.5f);
            healthBarFillRect.anchoredPosition = Vector2.zero;
            healthBarFillRect.sizeDelta = Vector2.zero;
            
            // Create health text
            GameObject healthTextObj = new GameObject("Health Text");
            healthTextObj.transform.SetParent(healthBarPanel.transform, false);
            
            healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
            healthText.alignment = TextAlignmentOptions.Center;
            healthText.fontSize = 18;
            healthText.color = Color.white;
            healthText.text = "100/100";
            
            RectTransform healthTextRect = healthTextObj.GetComponent<RectTransform>();
            healthTextRect.anchorMin = Vector2.zero;
            healthTextRect.anchorMax = Vector2.one;
            healthTextRect.sizeDelta = Vector2.zero;
        }
        
        // Create damage flash overlay if it doesn't exist
        if (damageFlashImage == null)
        {
            GameObject flashObj = new GameObject("Damage Flash");
            flashObj.transform.SetParent(mainCanvas.transform, false);
            
            damageFlashImage = flashObj.AddComponent<Image>();
            damageFlashImage.color = new Color(1f, 0f, 0f, 0f); // Start transparent
            
            // Make it cover the whole screen
            RectTransform flashRect = flashObj.GetComponent<RectTransform>();
            flashRect.anchorMin = Vector2.zero;
            flashRect.anchorMax = Vector2.one;
            flashRect.sizeDelta = Vector2.zero;
            flashRect.anchoredPosition = Vector2.zero;
        }
        
        // Hide UI elements initially
        HideInteractionPrompt();
        HideDialogue();
        
        // Show health bar
        healthBarPanel.SetActive(true);
        UpdateHealthBar(1.0f);
        
        // Create score panel if it doesn't exist
        if (scorePanel == null)
        {
            // Create score panel
            scorePanel = new GameObject("Score Panel");
            scorePanel.transform.SetParent(mainCanvas.transform, false);
            
            // Add components
            Image scorePanelImage = scorePanel.AddComponent<Image>();
            scorePanelImage.color = new Color(0, 0, 0, 0.5f);
            
            // Set panel position and size
            RectTransform scorePanelRect = scorePanel.GetComponent<RectTransform>();
            scorePanelRect.anchorMin = new Vector2(1, 1);
            scorePanelRect.anchorMax = new Vector2(1, 1);
            scorePanelRect.pivot = new Vector2(1, 1);
            scorePanelRect.anchoredPosition = new Vector2(-10, -10);
            scorePanelRect.sizeDelta = new Vector2(200, 40);
            
            // Create score text
            GameObject scoreTextObj = new GameObject("Score Text");
            scoreTextObj.transform.SetParent(scorePanel.transform, false);
            
            scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
            scoreText.alignment = TextAlignmentOptions.Center;
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            scoreText.text = "Score: 0";
            
            RectTransform scoreTextRect = scoreTextObj.GetComponent<RectTransform>();
            scoreTextRect.anchorMin = Vector2.zero;
            scoreTextRect.anchorMax = Vector2.one;
            scoreTextRect.sizeDelta = Vector2.zero;
            scoreTextRect.offsetMin = new Vector2(10, 5);
            scoreTextRect.offsetMax = new Vector2(-10, -5);
            
            // Show score panel
            scorePanel.SetActive(true);
            
            // Connect to the ScoreManager
            SetupScoreManager();
        }
    }
    
    private void SetupScoreManager()
    {
        // Find or create ScoreManager
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            GameObject scoreObj = new GameObject("ScoreManager");
            scoreManager = scoreObj.AddComponent<ScoreManager>();
        }
        
        // Register the score text with ScoreManager
        scoreManager.SetScoreText(scoreText);
    }
    
    public void ShowInteractionPrompt(string promptMessage)
    {
        Debug.Log($"[UIManager] ShowInteractionPrompt called with message: '{promptMessage}'. Panel active state before: {interactionPromptPanel?.activeSelf}", this);
        if (interactionPromptPanel != null && interactionPromptText != null)
        {
            interactionPromptText.text = promptMessage;
            interactionPromptPanel.SetActive(true);
            Debug.Log($"[UIManager] Interaction prompt panel activated. Text set to: '{interactionPromptText.text}'", this);
        }
        else
        {
            Debug.LogError("[UIManager] Interaction Prompt Panel or Text reference is missing!", this);
        }
    }
    
    public void HideInteractionPrompt()
    {
        Debug.Log($"[UIManager] HideInteractionPrompt called. Panel active state before: {interactionPromptPanel?.activeSelf}", this);
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
            Debug.Log("[UIManager] Interaction prompt panel deactivated.", this);
        }
        else
        {
             Debug.LogWarning("[UIManager] Interaction Prompt Panel reference is missing when trying to hide!", this);
        }
    }
    
    public void ShowDialogue(string dialogueMessage)
    {
        dialogueText.text = dialogueMessage;
        dialoguePanel.SetActive(true);
    }
    
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
    
    public void UpdateHealthBar(float healthPercent)
    {
        healthPercent = Mathf.Clamp01(healthPercent);
        
        if (healthBarFill != null)
        {
            // Update fill amount
            healthBarFill.fillAmount = healthPercent;
            
            // Change color based on health percentage
            if (healthPercent > 0.6f)
                healthBarFill.color = Color.green;
            else if (healthPercent > 0.3f)
                healthBarFill.color = Color.yellow;
            else
                healthBarFill.color = Color.red;
            
            // Update health text
            if (healthText != null)
            {
                int currentHealth = Mathf.RoundToInt(healthPercent * 100);
                healthText.text = currentHealth + "/100";
            }
        }
    }
    
    public void SetHealthBarScale(float scale)
    {
        if (healthBarPanel != null)
        {
            // Get the current size
            RectTransform healthPanelRect = healthBarPanel.GetComponent<RectTransform>();
            
            // Apply new scale - base size is 300x40
            healthPanelRect.sizeDelta = new Vector2(300 * scale, 40 * scale);
            
            // Adjust font size for the health text if it exists
            if (healthText != null)
            {
                healthText.fontSize = 18 * scale;
            }
            
            // Find the health bar background and adjust its size
            Transform healthBarBg = healthBarPanel.transform.Find("Health Bar Background");
            if (healthBarBg != null)
            {
                RectTransform healthBarBgRect = healthBarBg.GetComponent<RectTransform>();
                healthBarBgRect.sizeDelta = new Vector2(-20, 20 * scale);
            }
        }
    }
    
    public void ShowDamageFlash()
    {
        StartCoroutine(FlashRoutine());
    }
    
    private IEnumerator FlashRoutine()
    {
        // Fade in
        float elapsed = 0f;
        while (elapsed < flashDuration / 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, flashColor.a, elapsed / (flashDuration / 2));
            damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }
        
        // Fade out
        elapsed = 0f;
        while (elapsed < flashDuration / 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0f, elapsed / (flashDuration / 2));
            damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }
        
        // Ensure it's completely transparent at the end
        damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }
} 