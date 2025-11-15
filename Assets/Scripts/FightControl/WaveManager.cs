using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class SpawnPointConfig
{
    public Transform spawnPoint;
    public List<WaveEnemy> enemies;
}

[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval = 1f;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public List<SpawnPointConfig> spawnPointsConfig;
    public float preWaveDelay = 0f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Контроллеры дверей")]
    public DoorController[] doorControllers;

    [Header("Настройки волн")]
    public List<Wave> waves;
    public float timeBetweenWaves = 120f;
    public float initialDelay = 120f;

    [Header("Настройки таймера - Text Mesh Pro")]
    public TextMeshProUGUI countdownText; 
    public GameObject countdownPanel;
    public bool showMilliseconds = false;

    public int currentWaveIndex = 0;
    private Transform baseTarget;
    private bool gameStarted = false;

    public System.Action<float> OnTimerUpdate;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
        }

        InitializeTimerUI();
        StartCoroutine(InitialCountdown());
    }

    void InitializeTimerUI()
    {
        // Если текстовое поле не назначено, пробуем найти Text Mesh Pro компонент
        if (countdownText == null)
        {
            countdownText = GameObject.Find("CountdownText")?.GetComponent<TextMeshProUGUI>();
        }

        if (countdownText != null)
        {
            countdownText.text = FormatTime(initialDelay);
            countdownText.gameObject.SetActive(true);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }
    }

    IEnumerator InitialCountdown()
    {

        float timer = initialDelay;

        while (timer > 0)
        {
            UpdateTimerDisplay(timer);
            OnTimerUpdate?.Invoke(timer);

            yield return new WaitForSeconds(1f);
            timer -= 1f;

            if (timer <= 10f)
            {
                StartCoroutine(FlashTimer());
            }
        }

        OnTimerComplete();
    }

    void UpdateTimerDisplay(float timeRemaining)
    {
        if (countdownText != null)
        {
            countdownText.text = FormatTime(timeRemaining);
        }
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        if (showMilliseconds)
        {
            int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000) % 1000);
            return string.Format("До нашествия: {0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
        else
        {
            return string.Format("До нашествия: {0:00}:{1:00}", minutes, seconds);
        }
    }

    IEnumerator FlashTimer()
    {
        if (countdownText != null)
        {
            Color originalColor = countdownText.color;
            countdownText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            countdownText.color = originalColor;
        }
    }

    void OnTimerComplete()
    {

        if (countdownText != null)
        {
            countdownText.color = Color.red;

            StartCoroutine(HideTimerAfterDelay(3f));
        }

        gameStarted = true;
        StartCoroutine(WaveSpawner());
    }

    IEnumerator HideTimerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
    }

    // Остальные методы остаются без изменений
    IEnumerator WaveSpawner()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];

            if (currentWave.preWaveDelay > 0)
            {
                yield return new WaitForSeconds(currentWave.preWaveDelay);
            }

            yield return StartCoroutine(SpawnWave(currentWave));

            currentWaveIndex++;

            if (currentWaveIndex < waves.Count)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

    }

    IEnumerator SpawnWave(Wave wave)
    {
        List<Coroutine> spawnCoroutines = new List<Coroutine>();

        foreach (SpawnPointConfig spawnConfig in wave.spawnPointsConfig)
        {
            DoorController door = FindDoorBySpawnPoint(spawnConfig.spawnPoint);
            if (door != null)
            {
                door.ForceOpenDoor(true);
            }
        }

        foreach (SpawnPointConfig spawnConfig in wave.spawnPointsConfig)
        {
            if (spawnConfig.spawnPoint != null)
            {
                Coroutine coroutine = StartCoroutine(SpawnAtPoint(spawnConfig));
                spawnCoroutines.Add(coroutine);
            }
        }

        foreach (Coroutine coroutine in spawnCoroutines)
        {
            yield return coroutine;
        }

        yield return StartCoroutine(WaitForWaveCompletion());
    }

    DoorController FindDoorBySpawnPoint(Transform spawnPoint)
    {
        foreach (DoorController door in doorControllers)
        {
            if (door.spawnPoint == spawnPoint)
            {
                return door;
            }
        }
        return null;
    }

    IEnumerator SpawnAtPoint(SpawnPointConfig spawnConfig)
    {

        foreach (WaveEnemy waveEnemy in spawnConfig.enemies)
        {
            for (int i = 0; i < waveEnemy.count; i++)
            {
                SpawnEnemy(waveEnemy.enemyPrefab, spawnConfig.spawnPoint);

                if (i < waveEnemy.count - 1)
                {
                    yield return new WaitForSeconds(waveEnemy.spawnInterval);
                }
            }
        }

    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    IEnumerator WaitForWaveCompletion()
    {
        while (AreEnemiesAlive())
        {
            yield return new WaitForSeconds(1f);
        }
    }

    bool AreEnemiesAlive()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length > 0;
    }
}