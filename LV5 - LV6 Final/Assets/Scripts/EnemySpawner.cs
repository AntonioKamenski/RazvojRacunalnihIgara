using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Vector3 bossSpawnPosition = new Vector3(0f, 5f, 0f);

    private int currentWaveIndex = -1;
    private int activeEnemyCount = 0;
    private bool waveSpawnDone = false;

    void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnNextWave  += OnNextWave;
        GameManager.Instance.OnBossFight += SpawnBoss;
        Enemy.OnEnemyDied                += OnEnemyDied;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= OnGameStart;
            GameManager.Instance.OnNextWave  -= OnNextWave;
            GameManager.Instance.OnBossFight -= SpawnBoss;
        }
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    private void OnGameStart() => BeginNextWave();
    private void OnNextWave()  => BeginNextWave();

    private void BeginNextWave()
    {
        ClearRemainingEnemies();
        currentWaveIndex++;
        if (currentWaveIndex < waveConfigs.Count)
            StartCoroutine(SpawnWave(waveConfigs[currentWaveIndex]));
    }

    private void ClearRemainingEnemies()
    {
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            Destroy(enemy.gameObject);
    }

    private IEnumerator SpawnWave(WaveConfig wave)
    {
        activeEnemyCount = 0;
        waveSpawnDone    = false;

        for (int i = 0; i < wave.GetNumberOfEnemies(); i++)
        {
            var enemy = Instantiate(
                wave.GetEnemyPrefab(),
                wave.GetWaypoints()[0].transform.position,
                Quaternion.identity);

            enemy.GetComponent<EnemyPath>().SetWaveConfig(wave);
            activeEnemyCount++;
            yield return new WaitForSeconds(wave.GetTimeBetweenSpawns());
        }

        waveSpawnDone = true;
        CheckWaveComplete();
    }

    private void OnEnemyDied()
    {
        activeEnemyCount--;
        CheckWaveComplete();
    }

    private void CheckWaveComplete()
    {
        if (!waveSpawnDone || activeEnemyCount > 0) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        GameManager.Instance.WaveComplete();
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null)
            Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
    }
}
