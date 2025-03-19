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
        
        // Hide UI elements initially
        HideInteractionPrompt();
        HideDialogue();
        
        // Show health bar
        healthBarPanel.SetActive(true);
        UpdateHealthBar(1.0f);
    }
    
    public void ShowInteractionPrompt(string promptMessage)
    {
        interactionPromptText.text = promptMessage;
        interactionPromptPanel.SetActive(true);
    }
    
    public void HideInteractionPrompt()
    {
        interactionPromptPanel.SetActive(false);
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
} 