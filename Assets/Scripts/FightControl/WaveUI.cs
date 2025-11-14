using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text waveInfoText;
    public Text timerText;
    public Text enemiesCountText;

    private WaveManager waveManager;
    private float waveTimer;

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        if (waveManager != null)
        {
            // Информация о текущей волне
            waveInfoText.text = $"Волна: {waveManager.currentWaveIndex + 1}/{waveManager.waves.Count}";

            // Количество врагов на карте
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemiesCountText.text = $"Врагов: {enemyCount}";

            // Таймер можно добавить если нужно
        }
    }
}