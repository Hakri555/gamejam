using UnityEngine;
using System.Collections;

public class EntManager : MonoBehaviour
{
    [Header("Характеристики робота")]
    public float Health = 100f;
    public float Damage = 20f;
    public float attackRange = 1.5f;
    public float attackRate = 1f; // раз в секунду
    public string targetTag = "Enemy"; // тег объектов, которым наносится урон

    private Coroutine _attackCoroutine;
    void Start()
    {
        // сразу запускаем цикл атаки
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            // находим все объекты в радиусе
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // вызываем метод TakeDamage на объекте
                    hit.SendMessage("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
                }
            }

            yield return new WaitForSeconds(attackRate);
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

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
