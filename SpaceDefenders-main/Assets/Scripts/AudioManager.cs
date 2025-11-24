using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    public AudioClip buttonClickClip;
    public AudioClip buttonHoverClip;
    public AudioClip pauseClip;
    public AudioClip unpauseClip;
    public AudioClip speedSlowClip;
    public AudioClip speedNormalClip;
    public AudioClip speedFastClip;
    public AudioClip panelToggleClip;
    public AudioClip warningClip;

    public AudioClip towerPlacedClip;
    public AudioClip enemyDestroyedClip;
    public AudioClip missionCompleteClip;
    public AudioClip gameOverClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource.volume = 0.2f;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayTowerPlaced() => PlaySound(towerPlacedClip);
    public void PlayEnemyDestroyed() => PlaySound(enemyDestroyedClip);
    public void PlayButtonClick() => PlaySound(buttonClickClip);
    public void PlayButtonHover() => PlaySound(buttonHoverClip);
    public void PlayMissionComplete() => PlaySound(missionCompleteClip);
    public void PlayGameOver() => PlaySound(gameOverClip);
    public void PlayUnpause() => PlaySound(unpauseClip);
    public void PlayPause() => PlaySound(pauseClip);
    public void PlaySpeedSlow() => PlaySound(speedSlowClip);
    public void PlaySpeedNormal() => PlaySound(speedNormalClip);
    public void PlaySpeedFast() => PlaySound(speedFastClip);
    public void PlayWarning() => PlaySound(warningClip);
    public void PlayPanelToggle() => PlaySound(panelToggleClip);

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
