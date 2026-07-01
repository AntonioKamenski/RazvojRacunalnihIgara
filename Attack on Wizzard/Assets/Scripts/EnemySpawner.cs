using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject prefab;
        [Range(1, 10)] public int weight = 1;
    }

    [Header("Enemy Pool")]
    [SerializeField] private List<EnemyEntry> enemies;

    [Header("Wave Settings")]
    [SerializeField] private float waveDuration = 30f;
    [SerializeField] private int baseEnemiesPerWave = 5;
    [SerializeField] private int enemiesPerWaveIncrease = 2;

    [Header("Spawn Rate")]
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float intervalDecreasePerWave = 0.1f;
    [SerializeField] private float minimumSpawnInterval = 0.4f;

    [Header("Spawn Position")]
    [SerializeField] private float spawnMargin = 2f;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;
    private int spawnedThisWave = 0;
    private Camera mainCam;

    public int CurrentWave => currentWave;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        waveTimer += Time.deltaTime;

        if (waveTimer >= waveDuration)
        {
            currentWave++;
            waveTimer = 0f;
            spawnedThisWave = 0;
            spawnTimer = 0f;
        }

        var diff = GameManager.Instance?.ActiveDifficulty;
        float countMult    = diff != null ? diff.enemyCountMultiplier    : 1f;
        float intervalMult = diff != null ? diff.spawnIntervalMultiplier : 1f;

        int cap = Mathf.RoundToInt((baseEnemiesPerWave + currentWave * enemiesPerWaveIncrease) * countMult);
        if (spawnedThisWave >= cap) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Spawn();
            float interval = Mathf.Max(minimumSpawnInterval, baseSpawnInterval - currentWave * intervalDecreasePerWave);
            spawnTimer = interval * intervalMult;
        }
    }

    private void Spawn()
    {
        var prefab = PickWeightedRandom();
        if (prefab == null) return;

        Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        spawnedThisWave++;
    }

    private Vector2 GetSpawnPosition()
    {
        if (MapBorders.Instance != null)
        {
            float minX = MapBorders.Instance.minX + spawnMargin;
            float maxX = MapBorders.Instance.maxX - spawnMargin;
            float minY = MapBorders.Instance.minY + spawnMargin;
            float maxY = MapBorders.Instance.maxY - spawnMargin;

            if (minX > maxX)
            {
                float centerX = (MapBorders.Instance.minX + MapBorders.Instance.maxX) * 0.5f;
                minX = maxX = centerX;
            }

            if (minY > maxY)
            {
                float centerY = (MapBorders.Instance.minY + MapBorders.Instance.maxY) * 0.5f;
                minY = maxY = centerY;
            }

            int edge = Random.Range(0, 4);
            float x, y;

            switch (edge)
            {
                case 0:
                    x = Random.Range(minX, maxX);
                    y = maxY;
                    break;
                case 1:
                    x = Random.Range(minX, maxX);
                    y = minY;
                    break;
                case 2:
                    x = minX;
                    y = Random.Range(minY, maxY);
                    break;
                default:
                    x = maxX;
                    y = Random.Range(minY, maxY);
                    break;
            }

            return new Vector2(x, y);
        }

        if (mainCam == null)
            return Vector2.zero;

        float halfH = mainCam.orthographicSize + spawnMargin;
        float halfW = halfH * mainCam.aspect + spawnMargin;
        Vector2 center = mainCam.transform.position;

        int fallbackEdge = Random.Range(0, 4);
        float fallbackX, fallbackY;

        switch (fallbackEdge)
        {
            case 0: fallbackX = Random.Range(center.x - halfW, center.x + halfW); fallbackY = center.y + halfH; break;
            case 1: fallbackX = Random.Range(center.x - halfW, center.x + halfW); fallbackY = center.y - halfH; break;
            case 2: fallbackX = center.x - halfW; fallbackY = Random.Range(center.y - halfH, center.y + halfH); break;
            default: fallbackX = center.x + halfW; fallbackY = Random.Range(center.y - halfH, center.y + halfH); break;
        }

        return new Vector2(fallbackX, fallbackY);
    }

    private GameObject PickWeightedRandom()
    {
        if (enemies == null || enemies.Count == 0) return null;

        int total = 0;
        foreach (var e in enemies) total += e.weight;

        int roll = Random.Range(0, total);
        int cumulative = 0;
        foreach (var e in enemies)
        {
            cumulative += e.weight;
            if (roll < cumulative) return e.prefab;
        }

        return enemies[0].prefab;
    }
}
