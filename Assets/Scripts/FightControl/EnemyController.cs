using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Характеристики врага")]
    public float Health = 100f;
    public float Damage = 10f;
    public float Speed = 2f;

    [Range(0, 100)]
    public float DefencePrecent = 0f; // Процент защиты
    public float AttackSpeed = 1f; // Атак в секунду

    private bool canAttack = true;
    private Transform baseTarget;

    private void Start()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
        }
        else
        {
            Debug.LogError("Нет объекта с тегом Base");
        }
    }

    private void Update()
    {
        if (baseTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, baseTarget.position, Speed * Time.deltaTime);
        }

    }

    // Метод атаки при столкновении коллайдеров
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {
            StartCoroutine(AttackRoutine(collision.gameObject));
        }
        else if (collision.CompareTag("Robot"))
        {
            StartCoroutine(AttackRoutine(collision.gameObject));
        }
    }

    private IEnumerator AttackRoutine(GameObject target)
    {
        while (target != null && Health > 0)
        {
            if (canAttack)
            {
                Attack(target);
                canAttack = false;
                yield return new WaitForSeconds(1f / AttackSpeed);
                canAttack = true;
            }
            yield return null;
        }
    }

    private void Attack(GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(Damage);
            Debug.Log("Нанесен урон " + Damage);
        }
        else
        {
            Debug.LogWarning("У объекта " + target.name + " нет IDamageable.");
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        float damageMultiplier = 1f - (DefencePrecent / 100f);
        float actualDamage = incomingDamage * damageMultiplier;

        Health -= actualDamage;
        Debug.Log("Получено урона " + actualDamage);

        if (Health < 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


}
