using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Спрайты двери")]
    public GameObject closedDoor;
    public GameObject openDoor;

    [Header("Привязка к спавнпоинту")]
    public Transform spawnPoint;

    [Header("Настройки обнаружения")]
    public float detectionRadius = 15f;
    public float checkInterval = 0.3f;

    [Header("Звуки двери")]
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public AudioClip doorLoopSound;
    public float doorSoundVolume = 0.7f;

    private Coroutine checkCoroutine;
    private AudioSource audioSource;
    private AudioSource oneShotAudioSource; // Отдельный для коротких звуков
    private bool wasOpen = false;

    private void Start()
    {
        // Основной AudioSource для цикличного звука
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = doorSoundVolume;
        audioSource.spatialBlend = 1f;

        // Второй AudioSource для коротких звуков
        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
        oneShotAudioSource.playOnAwake = false;
        oneShotAudioSource.loop = false;
        oneShotAudioSource.volume = doorSoundVolume;
        oneShotAudioSource.spatialBlend = 1f;

        SetDoorState(false);

        if (spawnPoint != null)
        {
            checkCoroutine = StartCoroutine(DoorStateChecker());
        }
    }

    IEnumerator DoorStateChecker()
    {
        while (true)
        {
            bool enemiesNearby = CheckForEnemiesNearSpawn();
            SetDoorState(enemiesNearby);
            ManageDoorSound(enemiesNearby);
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void ManageDoorSound(bool isOpen)
    {
        if (isOpen != wasOpen)
        {
            if (isOpen)
            {
                // Дверь открылась
                PlayDoorOpenSound();
                StartLoopSound();
            }
            else
            {
                // Дверь закрылась - СНАЧАЛА звук, ПОТОМ остановка
                PlayDoorCloseSound();

                // Ждем окончания звука закрытия перед остановкой цикличного
                if (doorCloseSound != null)
                {
                    float closeSoundDuration = doorCloseSound.length;
                    Invoke("StopLoopSound", closeSoundDuration);
                }
                else
                {
                    StopLoopSound();
                }
            }
            wasOpen = isOpen;
        }
    }

    void PlayDoorOpenSound()
    {
        if (doorOpenSound != null && oneShotAudioSource != null)
        {
            oneShotAudioSource.PlayOneShot(doorOpenSound);
            Debug.Log($"🔊 {gameObject.name}: звук открытия");
        }
    }

    void PlayDoorCloseSound()
    {
        if (doorCloseSound != null && oneShotAudioSource != null)
        {
            oneShotAudioSource.PlayOneShot(doorCloseSound);
            Debug.Log($"🔊 {gameObject.name}: звук закрытия");
        }
    }

    void StartLoopSound()
    {
        if (doorLoopSound != null && audioSource != null)
        {
            audioSource.clip = doorLoopSound;
            audioSource.Play();
            Debug.Log($"🔊 {gameObject.name}: запущен цикличный звук");
        }
    }

    void StopLoopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log($"🔊 {gameObject.name}: цикличный звук остановлен");
        }
    }

    bool CheckForEnemiesNearSpawn()
    {
        if (spawnPoint == null) return false;

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(
            spawnPoint.position,
            detectionRadius
        );

        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    void SetDoorState(bool isOpen)
    {
        if (closedDoor == null || openDoor == null) return;

        closedDoor.SetActive(!isOpen);
        openDoor.SetActive(isOpen);
    }

    public void ForceOpenDoor(bool open, float duration = 0f)
    {
        if (checkCoroutine != null) StopCoroutine(checkCoroutine);

        if (duration > 0)
        {
            StartCoroutine(ForceOpenForDuration(open, duration));
        }
        else
        {
            SetDoorState(open);
            ManageDoorSound(open);
            checkCoroutine = StartCoroutine(DoorStateChecker());
        }
    }

    IEnumerator ForceOpenForDuration(bool open, float duration)
    {
        SetDoorState(open);
        ManageDoorSound(open);
        yield return new WaitForSeconds(duration);
        checkCoroutine = StartCoroutine(DoorStateChecker());
    }
}