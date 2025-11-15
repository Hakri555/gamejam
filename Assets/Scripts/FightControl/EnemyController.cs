using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Характеристики врага")]
    public float Health = 100f;
    public float Damage = 10f;
    public float MovementSpeed = 0.8f;
    public float stopDistance = 1.5f;
    public float enemyDetectionRadius = 1.5f;

    [Header("Взрыв при достижении базы")]
    public float explosionDamage = 50f;
    public float explosionRadius = 3f;
    public GameObject explosionEffect;

    private Animator _animator;
    private bool _hasReachedBase = false;
    private bool _isInCombat = false;
    private bool _isStoppedByEnemy = false;
    private Transform _baseTarget;
    private Coroutine _attackCoroutine;
    private GameObject _currentTarget;
    private Bounds _baseBounds;
    private Vector3 _lastPosition;
    private float _currentSpeed;
    private bool _wantsToMove = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _lastPosition = transform.position;

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            _baseTarget = baseObject.transform;
            _currentTarget = baseObject;

            Collider2D baseCollider = baseObject.GetComponent<Collider2D>();
            if (baseCollider != null)
            {
                _baseBounds = baseCollider.bounds;
            }
        }
    }

    void Update()
    {
        CalculateMovementSpeed();
        UpdateAnimations();

        if (_hasReachedBase) return;

        CheckForObstacles();

        if (!_isInCombat && !_isStoppedByEnemy && _currentTarget != null)
        {
            Vector3 targetPosition = GetTargetPosition(_currentTarget);
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget > stopDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    MovementSpeed * Time.deltaTime
                );
            }
            else
            {
                if (_currentTarget.CompareTag("Base"))
                {
                    _hasReachedBase = true;
                    ExplodeAtBase();
                }
                else if (_currentTarget.CompareTag("Robot"))
                {
                    _isInCombat = true;
                    StartAttack(_currentTarget);
                }
            }
        }
    }

    void CalculateMovementSpeed()
    {
        Vector3 currentPosition = transform.position;
        _currentSpeed = (currentPosition - _lastPosition).magnitude / Time.deltaTime;
        _lastPosition = currentPosition;
    }

    void UpdateAnimations()
    {
        if (_animator == null) return;

        int baseLayerIndex = 0; // Базовый слой (движение/покой)
        int attackLayerIndex = 1; // Слой атаки

        // НОВАЯ ЛОГИКА СО СЛОЯМИ:
        if (_isInCombat)
        {
            // В БОЮ - включаем слой атаки, выключаем движение
            _animator.SetLayerWeight(attackLayerIndex, 1f);
            _animator.SetLayerWeight(baseLayerIndex, 0f);
            _animator.SetBool("IsAttacking", true);
        }
        else if (_isStoppedByEnemy)
        {
            // ОСТАНОВЛЕН ВРАГОМ - анимация покоя на базовом слое
            _animator.SetLayerWeight(attackLayerIndex, 0f);
            _animator.SetLayerWeight(baseLayerIndex, 1f);
            _animator.SetFloat("Speed", 0f);
            _animator.SetBool("IsAttacking", false);
        }
        else
        {
            // ДВИЖЕНИЕ - анимация ходьбы на базовом слое
            _animator.SetLayerWeight(attackLayerIndex, 0f);
            _animator.SetLayerWeight(baseLayerIndex, 1f);
            _animator.SetFloat("Speed", 1f);
            _animator.SetBool("IsAttacking", false);
        }

        // Направление для разворота спрайта
        if (_currentTarget != null)
        {
            UpdateSpriteDirection();
        }
    }

    void UpdateSpriteDirection()
    {
        Vector3 direction = _currentTarget.transform.position - transform.position;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Остальные методы без изменений...
    Vector3 GetTargetPosition(GameObject target)
    {
        if (target.CompareTag("Base") && _baseBounds.size != Vector3.zero)
        {
            return GetClosestPointOnBounds(_baseBounds, transform.position);
        }
        else
        {
            return target.transform.position;
        }
    }

    private Vector3 GetClosestPointOnBounds(Bounds bounds, Vector3 point)
    {
        Vector3 closestInside = bounds.ClosestPoint(point);

        if (bounds.Contains(point))
        {
            float distToLeft = Mathf.Abs(point.x - bounds.min.x);
            float distToRight = Mathf.Abs(point.x - bounds.max.x);
            float distToBottom = Mathf.Abs(point.y - bounds.min.y);
            float distToTop = Mathf.Abs(point.y - bounds.max.y);

            float minDist = Mathf.Min(distToLeft, distToRight, distToBottom, distToTop);

            if (minDist == distToLeft) return new Vector3(bounds.min.x, point.y, point.z);
            if (minDist == distToRight) return new Vector3(bounds.max.x, point.y, point.z);
            if (minDist == distToBottom) return new Vector3(point.x, bounds.min.y, point.z);
            return new Vector3(point.x, bounds.max.y, point.z);
        }

        return closestInside;
    }

    void CheckForObstacles()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius);

        GameObject closestRobot = null;
        GameObject closestEnemy = null;
        float closestRobotDistance = Mathf.Infinity;
        float closestEnemyDistance = Mathf.Infinity;

        foreach (Collider2D collider in nearbyObjects)
        {
            if (collider.gameObject == gameObject) continue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (collider.CompareTag("Robot") && distance < closestRobotDistance)
            {
                closestRobotDistance = distance;
                closestRobot = collider.gameObject;
            }

            if (collider.CompareTag("Enemy") && distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                closestEnemy = collider.gameObject;
            }
        }

        if (closestRobot != null)
        {
            _currentTarget = closestRobot;
            _isStoppedByEnemy = false;
        }
        else if (closestEnemy != null && IsEnemyInFront(closestEnemy))
        {
            _isStoppedByEnemy = true;
            _currentTarget = _baseTarget != null ? _baseTarget.gameObject : null;
        }
        else
        {
            _isStoppedByEnemy = false;
            if (_baseTarget != null)
            {
                _currentTarget = _baseTarget.gameObject;
            }
        }
    }

    bool IsEnemyInFront(GameObject enemy)
    {
        if (_baseTarget == null) return false;

        Vector3 toEnemy = enemy.transform.position - transform.position;
        Vector3 toBase = _baseTarget.position - transform.position;

        float dot = Vector3.Dot(toEnemy.normalized, toBase.normalized);

        return dot > 0.7f && Vector3.Distance(transform.position, enemy.transform.position) < enemyDetectionRadius;
    }

    void StartAttack(GameObject target)
    {
        if (_attackCoroutine == null && target != null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine(target));
        }
    }

    IEnumerator AttackRoutine(GameObject target)
    {
        while (target != null && Health > 0)
        {
            Attack(target);
            yield return new WaitForSeconds(1f);
        }

        _attackCoroutine = null;

        if (target == null || target.CompareTag("Robot"))
        {
            _isInCombat = false;
            _currentTarget = _baseTarget != null ? _baseTarget.gameObject : null;
        }
    }

    void Attack(GameObject target)
    {
        if (target.CompareTag("Base"))
        {
            BaseHealth baseHealth = target.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(Damage);
            }
        }
        else if (target.CompareTag("Robot"))
        {
            EntManager robotHealth = target.GetComponent<EntManager>();
            if (robotHealth != null)
            {
                robotHealth.TakeDamage(Damage);
            }
        }
    }

    void ExplodeAtBase()
    {
        Debug.Log($"💥 {gameObject.name} взрывается у базы! Урон: {explosionDamage}");

        if (_animator != null)
        {
            _animator.SetTrigger("Explode");
        }

        if (_baseTarget != null)
        {
            BaseHealth baseHealth = _baseTarget.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(explosionDamage);
            }
        }

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in hitObjects)
        {
            if (collider.CompareTag("Robot"))
            {
                EntManager robot = collider.GetComponent<EntManager>();
                if (robot != null)
                {
                    robot.TakeDamage(explosionDamage * 0.5f);
                }
            }
            else if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(explosionDamage * 0.3f);
                }
            }
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Invoke("Die", 0.5f);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (_animator != null)
        {
            _animator.SetTrigger("TakeDamage");
        }

        if (Health <= 0) Die();
    }

    void Die()
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        Destroy(gameObject);
    }
}