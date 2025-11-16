using UnityEngine;
using System.Collections;

public class EntManager : MonoBehaviour
{
    [Header("Характеристики робота")]
    public float Health = 100f;
    public float Damage = 20f;
    public float attackRange = 1f;
    public float attackRate = 1f;
    public string targetTag = "Enemy";

    [Header("Настройки анимаций")]
    public float attackAnimationDuration = 0.5f;

    [Header("Отладка")]
    public bool debugMode = true;

    private Animator _animator;
    private Coroutine _attackCoroutine;
    private bool _isAttacking = false;
    private bool _hasTargetInRange = false;

    void Awake()
    {
        // Автоматически получаем аниматор
        _animator = GetComponent<Animator>();
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator не найден! Добавь компонент Animator на объект.");
            return;
        }

        // Отзеркаливание
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x); 
        transform.localScale = scale;

        _attackCoroutine = StartCoroutine(AttackRoutine());

        if (debugMode)
            Debug.Log("EntManager запущен. Аниматор: " + _animator.name);
    }

    void Update()
    {
        CheckForTargetsInRange();
        HandleAnimations();

        // Отладка
        if (debugMode && Time.frameCount % 60 == 0)
        {
            Debug.Log($"Состояние: Атака={_isAttacking}, Цель в радиусе={_hasTargetInRange}");
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_hasTargetInRange && !_isAttacking)
            {
                yield return StartCoroutine(PerformAttack());
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator PerformAttack()
    {
        _isAttacking = true;

        if (debugMode) Debug.Log("Начинаем атаку!");

        // Запускаем анимацию атаки через триггер
        _animator.SetTrigger("Attack");

        // Ждем перед нанесением урона (чтобы совпало с анимацией)
        yield return new WaitForSeconds(attackAnimationDuration * 0.3f);

        // Наносим урон
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                hit.SendMessage("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
                if (debugMode) Debug.Log("Нанесен урон: " + hit.name);
            }
        }

        // Ждем завершения анимации
        yield return new WaitForSeconds(attackAnimationDuration * 0.7f);

        _isAttacking = false;

        if (debugMode) Debug.Log("Атака завершена");

        // Ждем перед следующей атакой
        yield return new WaitForSeconds(attackRate - attackAnimationDuration);
    }

    void CheckForTargetsInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        _hasTargetInRange = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                _hasTargetInRange = true;
                break;
            }
        }
    }

    void HandleAnimations()
    {
        if (_isAttacking) return;

        // Управляем анимацией ожидания/готовности
        if (_hasTargetInRange)
        {
            _animator.SetBool("HasTarget", true);
        }
        else
        {
            _animator.SetBool("HasTarget", false);
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        if (Health <= 0) Die();
    }

    void Die()
    {
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        if (_animator != null)
            _animator.SetTrigger("Die");

        Destroy(gameObject, 1f); // Даем время на анимацию смерти
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}