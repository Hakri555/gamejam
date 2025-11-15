using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Настройки турели")]
    public float attackRange = 5f;
    public float damage = 25f;
    public float attackSpeed = 1f;
    public GameObject bulletPrefab;

    [Header("Визуальные настройки")]
    public Transform rotationPart;
    public Transform firePoint;

    private Transform _currentTarget;
    private bool _canAttack = true;
    private Coroutine _attackCoroutine;

    void Update()
    {
        FindTarget();

        if (_currentTarget != null)
        {
            RotateTowardsTarget();

            if (_canAttack && _attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
                _canAttack = true;
            }
        }
    }

    void FindTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
        }

        _currentTarget = closestEnemy;
    }

    void RotateTowardsTarget()
    {
        if (rotationPart != null && _currentTarget != null)
        {
            Vector2 direction = _currentTarget.position - rotationPart.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotationPart.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    IEnumerator AttackRoutine()
    {
        while (_currentTarget != null && _canAttack)
        {
            Attack();
            _canAttack = false;
            yield return new WaitForSeconds(1f / attackSpeed);
            _canAttack = true;
        }

        _attackCoroutine = null;
    }

    void Attack()
    {
        Debug.Log($"💥 Турель {gameObject.name} АТАКУЕТ!");

        if (bulletPrefab == null)
        {
            Debug.LogError("❌ bulletPrefab не назначен!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("❌ firePoint не назначен!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log($"🚀 Создан снаряд в позиции: {firePoint.position}");

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(_currentTarget);
            bulletScript.damage = damage;
            Debug.Log($"✅ Снаряд настроен на цель: {_currentTarget.name}");
        }
        else
        {
            Debug.LogError("❌ У снаряда нет скрипта Bullet!");
        }

        // Проверяем видимость снаряда
        SpriteRenderer bulletSprite = bullet.GetComponent<SpriteRenderer>();
        if (bulletSprite != null)
        {
            Debug.Log($"👀 Снаряд имеет SpriteRenderer: {bulletSprite.sprite != null}");
        }
        else
        {
            Debug.LogError("❌ У снаряда нет SpriteRenderer!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}