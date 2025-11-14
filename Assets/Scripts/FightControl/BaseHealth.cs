using UnityEngine;
using UnityEngine.Events;

public class BaseHealth : MonoBehaviour, IDamageable
{
    [Header ("Настрйоки здоровья базы")]
    public float MaxHealth = 500f;
    public float CurrentHealth;

    [Header ("События")]
    public UnityEvent OnBaseDamaged; // Башню атакуют
    public UnityEvent OnBaseDestroyed; // Башню сломали

    [Header("Визуальные эффекты")]
    public Color damageColor = Color.red; // Красный цвет на короткое время после получения урона
    public float flashDuration = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if ( _spriteRenderer != null )
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log("База получила урон: " + damage);

        if (_spriteRenderer != null)
        {
            StartCoroutine(DamageFlash());
        }

        OnBaseDamaged?.Invoke();
        
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Реализация мигания красным, когда идет урон
    private System.Collections.IEnumerator DamageFlash()
    {
        _spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.color = _originalColor;
    }

    private void Die()
    {
        OnBaseDestroyed?.Invoke();
        // Временное решение (пока не решили концовку)
        gameObject.SetActive(false);
    }
}
