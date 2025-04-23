using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components

public class BackgroundMusicController : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    private const float BGM_DURATION = 22.5f;
    private bool hasPlayedInitial = false;
    private bool isMusicPaused = false;

    [Header("UI")]
    public Image musicIcon; // Reference to the music icon image
    public Sprite musicOnSprite; // Sprite for when music is playing
    public Sprite musicOffSprite; // Sprite for when music is paused

    void Start()
    {
        // Add AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 0.03f;

        // Start playing background music
        if (backgroundMusic != null)
        {
            StartCoroutine(PlayBackgroundMusic());
        }

        // Set initial icon state
        UpdateMusicIcon();
    }

    void Update()
    {
        // Toggle music with M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }

    public void ToggleMusic()
    {
        if (audioSource != null)
        {
            if (isMusicPaused)
            {
                audioSource.UnPause();
                isMusicPaused = false;
            }
            else
            {
                audioSource.Pause();
                isMusicPaused = true;
            }
            UpdateMusicIcon();
        }
    }

    void UpdateMusicIcon()
    {
        if (musicIcon != null)
        {
            musicIcon.sprite = isMusicPaused ? musicOffSprite : musicOnSprite;
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        // Play the initial 22.5 seconds
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        yield return new WaitForSeconds(BGM_DURATION);
        
        // After initial playthrough, set up looping
        hasPlayedInitial = true;
        audioSource.loop = true;
        audioSource.time = 0f; // Reset to start
        audioSource.Play();
    }
} 