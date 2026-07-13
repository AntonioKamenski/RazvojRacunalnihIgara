using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { StartScreen, Playing, PowerupSelect, BossFight, Win, Lose }
    public GameState CurrentState { get; private set; } = GameState.StartScreen;

    public event Action OnGameStart;
    public event Action<int> OnWaveComplete;
    public event Action OnNextWave;
    public event Action OnBossFight;
    public event Action OnWin;
    public event Action OnLose;

    public const int TotalWaves = 3;
    private int currentWave = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartGame()
    {
        currentWave = 0;
        CurrentState = GameState.Playing;
        OnGameStart?.Invoke();
    }

    public void WaveComplete()
    {
        currentWave++;
        CurrentState = GameState.PowerupSelect;
        OnWaveComplete?.Invoke(currentWave);
    }

    // Called by PowerupManager after a powerup is selected
    public void PowerupSelected()
    {
        if (currentWave >= TotalWaves)
        {
            CurrentState = GameState.BossFight;
            OnBossFight?.Invoke();
        }
        else
        {
            CurrentState = GameState.Playing;
            OnNextWave?.Invoke();
        }
    }

    public void BossDefeated()
    {
        if (CurrentState == GameState.BossFight)
        {
            CurrentState = GameState.Win;
            OnWin?.Invoke();
        }
    }

    public void PlayerDied()
    {
        if (CurrentState != GameState.Win && CurrentState != GameState.Lose)
        {
            CurrentState = GameState.Lose;
            OnLose?.Invoke();
        }
    }

    public int GetCurrentWave() => currentWave;
}
