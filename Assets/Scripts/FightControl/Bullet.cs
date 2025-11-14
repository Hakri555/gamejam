using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Настройки снаряда")]
    public float speed = 10f;
    public float damage = 25f;
    public float lifeTime = 3f;

    private Transform _target;

    void Start()
    {
        Debug.Log($"🎯 Снаряд создан! Цель: {_target?.name ?? "нет"}");

        // Делаем снаряд видимым - красный цвет
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            Debug.Log("🔴 Снаряд окрашен в красный");
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (_target != null)
        {
            // Простое движение к цели
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);

            // Поворачиваем к цели
            Vector2 direction = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            Debug.Log($"➡️ Снаряд движется к {_target.name}, позиция: {transform.position}");
        }
        else
        {
            // Летим прямо если цель потеряна
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        Debug.Log($"🎯 Снаряд получил цель: {target.name}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"💥 Снаряд столкнулся с: {collision.gameObject.name} (тег: {collision.tag})");

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"🎯 Попадание во врага: {collision.gameObject.name}");

            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"💀 Нанесен урон: {damage}");
            }
            else
            {
                Debug.LogError("❌ У врага нет EnemyController!");
            }

            Destroy(gameObject);
        }
    }
}