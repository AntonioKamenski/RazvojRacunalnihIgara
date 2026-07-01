using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("AudioManager [Auto]");
                _instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip[] gameMusicTracks;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 1f;

    [Header("SFX — UI")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip upgradeSelectClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField, Range(0f, 1f)] private float uiVolume = 1f;

    [Header("SFX — Player Combat")]
    [SerializeField] private AudioClip[] playerMeleeClips;
    [SerializeField] private AudioClip[] playerBowShotClips;
    [SerializeField] private AudioClip[] playerSpellClips;
    [SerializeField, Range(0f, 1f)] private float playerCombatVolume = 1f;

    [Header("SFX — Player Feedback")]
    [SerializeField] private AudioClip[] playerHitClips;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField, Range(0f, 1f)] private float playerFeedbackVolume = 1f;

    [Header("SFX — Enemy Combat")]
    [SerializeField] private AudioClip[] enemyMeleeClips;
    [SerializeField] private AudioClip[] enemyRangedClips;
    [SerializeField, Range(0f, 1f)] private float enemyCombatVolume = 1f;

    [Header("SFX — Enemy Feedback")]
    [SerializeField] private AudioClip[] enemyHitClips;
    [SerializeField] private AudioClip[] enemyDeathClips;
    [SerializeField, Range(0f, 1f)] private float enemyFeedbackVolume = 1f;

    [Header("SFX — Boss")]
    [SerializeField] private AudioClip bossRoarClip;
    [SerializeField] private AudioClip bossDeathClip;
    [SerializeField, Range(0f, 1f)] private float bossVolume = 1f;

    [Header("SFX — World")]
    [SerializeField] private AudioClip portalOpenClip;
    [SerializeField] private AudioClip xpPickupClip;
    [SerializeField] private AudioClip bleedTickClip;
    [SerializeField] private AudioClip freezeClip;
    [SerializeField, Range(0f, 1f)] private float worldVolume = 1f;

    [Header("SFX Settings")]
    [SerializeField] private float pitchVariation = 0.08f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    // ── Music ────────────────────────────────────────────────────────────────

    public void PlayMenuMusic() => PlayMusicClip(menuMusic);

    public void PlayGameMusic()
    {
        if (gameMusicTracks == null || gameMusicTracks.Length == 0) return;
        currentTrackIndex = 0;
        PlayMusicClip(gameMusicTracks[currentTrackIndex]);
    }

    public void NextTrack()
    {
        if (gameMusicTracks == null || gameMusicTracks.Length == 0) return;
        currentTrackIndex = (currentTrackIndex + 1) % gameMusicTracks.Length;
        PlayMusicClip(gameMusicTracks[currentTrackIndex]);
    }

    public string GetCurrentTrackName()
    {
        if (gameMusicTracks == null || gameMusicTracks.Length == 0) return "None";
        var clip = gameMusicTracks[currentTrackIndex];
        return clip != null ? clip.name : "None";
    }

    private void PlayMusicClip(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        if (GameManager.Instance != null && GameManager.Instance.IsMusicEnabled)
            musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public float GetMusicVolume() => musicVolume;

    public void SetMusicEnabled(bool enabled)
    {
        if (enabled) { if (!musicSource.isPlaying && musicSource.clip != null) musicSource.Play(); }
        else musicSource.Stop();
    }

    // ── UI SFX ───────────────────────────────────────────────────────────────

    public void PlayButtonClick()   => Play(buttonClickClip,   uiVolume);
    public void PlayUpgradeSelect() => Play(upgradeSelectClip, uiVolume);
    public void PlayLevelUp()       => Play(levelUpClip,       uiVolume);

    // ── Player combat SFX ────────────────────────────────────────────────────

    public void PlayPlayerMelee()   => PlayRandom(playerMeleeClips,   playerCombatVolume);
    public void PlayPlayerBowShot() => PlayRandom(playerBowShotClips, playerCombatVolume);
    public void PlayPlayerSpell()   => PlayRandom(playerSpellClips,   playerCombatVolume);

    // ── Player feedback SFX ──────────────────────────────────────────────────

    public void PlayPlayerHit()   => PlayRandom(playerHitClips,  playerFeedbackVolume);
    public void PlayPlayerDeath() => Play(playerDeathClip,       playerFeedbackVolume);

    // ── Enemy combat SFX ─────────────────────────────────────────────────────

    public void PlayEnemyMelee()  => PlayRandom(enemyMeleeClips,  enemyCombatVolume);
    public void PlayEnemyRanged() => PlayRandom(enemyRangedClips, enemyCombatVolume);

    // ── Enemy feedback SFX ───────────────────────────────────────────────────

    public void PlayEnemyHit()   => PlayRandom(enemyHitClips,   enemyFeedbackVolume);
    public void PlayEnemyDeath() => PlayRandom(enemyDeathClips, enemyFeedbackVolume);

    // ── Boss SFX ─────────────────────────────────────────────────────────────

    public void PlayBossRoar()  => Play(bossRoarClip,  bossVolume);
    public void PlayBossDeath() => Play(bossDeathClip, bossVolume);

    // ── World SFX ────────────────────────────────────────────────────────────

    public void PlayPortalOpen() => Play(portalOpenClip, worldVolume);
    public void PlayXPPickup()   => Play(xpPickupClip,   worldVolume);
    public void PlayBleedTick()  => Play(bleedTickClip,  worldVolume);
    public void PlayFreeze()     => Play(freezeClip,     worldVolume);

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void Play(AudioClip clip, float volume)
    {
        if (clip == null) return;
        sfxSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        sfxSource.PlayOneShot(clip, volume);
    }

    private void PlayRandom(AudioClip[] clips, float volume)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;
        sfxSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        sfxSource.PlayOneShot(clip, volume);
    }
}
