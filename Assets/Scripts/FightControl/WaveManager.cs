using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnPointConfig
{
    public Transform spawnPoint;
    public List<WaveEnemy> enemies; // Враги для этого вейпоинта
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
    public List<SpawnPointConfig> spawnPointsConfig; // Конфиг для каждого вейпоинта
    public float preWaveDelay = 0f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Контроллеры дверей")]
    public DoorController[] doorControllers;

    [Header("Настройки волн")]
    public List<Wave> waves;
    public float timeBetweenWaves = 120f;

    public int currentWaveIndex = 0;
    private Transform baseTarget;

    void Start()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
        }

        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        yield return new WaitForSeconds(5f);

        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"=== НАЧИНАЕТСЯ ВОЛНА {currentWaveIndex + 1}: {currentWave.waveName} ===");

            if (currentWave.preWaveDelay > 0)
            {
                Debug.Log($"Ожидание перед волной: {currentWave.preWaveDelay} сек");
                yield return new WaitForSeconds(currentWave.preWaveDelay);
            }

            yield return StartCoroutine(SpawnWave(currentWave));

            currentWaveIndex++;

            if (currentWaveIndex < waves.Count)
            {
                Debug.Log($"=== ВОЛНА ЗАВЕРШЕНА. СЛЕДУЮЩАЯ ЧЕРЕЗ {timeBetweenWaves} СЕКУНД ===");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        Debug.Log("=== ВСЕ ВОЛНЫ ЗАВЕРШЕНЫ! ===");
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

        // Запускаем спавн для каждого вейпоинта параллельно
        foreach (SpawnPointConfig spawnConfig in wave.spawnPointsConfig)
        {
            if (spawnConfig.spawnPoint != null)
            {
                Coroutine coroutine = StartCoroutine(SpawnAtPoint(spawnConfig));
                spawnCoroutines.Add(coroutine);
            }
        }

        // Ждем завершения всех корутин спавна
        foreach (Coroutine coroutine in spawnCoroutines)
        {
            yield return coroutine;
        }

        // Ждем пока все враги волны не будут уничтожены
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
        Debug.Log($"Начало спавна в точке: {spawnConfig.spawnPoint.name}");

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
            Debug.Log($"Заспавнено {waveEnemy.count} врагов типа {waveEnemy.enemyPrefab.name} в {spawnConfig.spawnPoint.name}");
        }

        Debug.Log($"Завершен спавн в точке: {spawnConfig.spawnPoint.name}");
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Заспавнен {enemyPrefab.name} в {spawnPoint.name}");
    }

    IEnumerator WaitForWaveCompletion()
    {
        while (AreEnemiesAlive())
        {
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Все враги волны уничтожены!");
    }

    bool AreEnemiesAlive()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length > 0;
    }
}