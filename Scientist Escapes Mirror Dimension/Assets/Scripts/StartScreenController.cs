using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "GameScene"; // Change this to your actual game scene name
    [SerializeField] private float buttonHoverScale = 1.1f;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip buttonClickSound;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        
        // Hide options panel initially
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        
        // Set up button listeners
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
        
        if (optionsButton != null)
            optionsButton.onClick.AddListener(ToggleOptions);
        
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
            
        // Add hover animations
        AddHoverEffects(startButton);
        AddHoverEffects(optionsButton);
        AddHoverEffects(exitButton);
    }
    
    private void AddHoverEffects(Button button)
    {
        if (button == null) return;
        
        // Get event trigger or add one if it doesn't exist
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();
            
        // Add pointer enter event
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => {
            button.transform.localScale = Vector3.one * buttonHoverScale;
            if (buttonHoverSound != null && audioSource != null)
                audioSource.PlayOneShot(buttonHoverSound);
        });
        trigger.triggers.Add(enterEntry);
        
        // Add pointer exit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => {
            button.transform.localScale = Vector3.one;
        });
        trigger.triggers.Add(exitEntry);
    }
    
    public void StartGame()
    {
        // Play click sound
        if (buttonClickSound != null && audioSource != null)
            audioSource.PlayOneShot(buttonClickSound);
            
        // Start a coroutine to load the scene after playing the sound
        StartCoroutine(LoadGameScene());
    }
    
    private IEnumerator LoadGameScene()
    {
        // Wait a short time for the click sound to play
        yield return new WaitForSeconds(0.2f);
        
        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void ToggleOptions()
    {
        // Play click sound
        if (buttonClickSound != null && audioSource != null)
            audioSource.PlayOneShot(buttonClickSound);
            
        // Toggle options panel
        if (optionsPanel != null)
            optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
    
    public void ExitGame()
    {
        // Play click sound
        if (buttonClickSound != null && audioSource != null)
            audioSource.PlayOneShot(buttonClickSound);
            
        Debug.Log("Exit game requested");
        
        // Wait a bit before quitting to allow sound to play
        StartCoroutine(QuitAfterDelay());
    }
    
    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
} 