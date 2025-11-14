using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Характеристики врага")]
    public float Health = 100f;
    public float Damage = 10f;
    public float MovementSpeed = 2f;
    public float stopDistance = 1f;
    public float enemyDetectionRadius = 1f;

    private bool _hasReachedBase = false;
    private bool _isInCombat = false;
    private bool _isStoppedByEnemy = false;
    private Transform _baseTarget;
    private Coroutine _attackCoroutine;
    private GameObject _currentTarget;
    private Bounds _baseBounds;

    void Start()
    {
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
                    StartAttack(_currentTarget);
                }
                else if (_currentTarget.CompareTag("Robot"))
                {
                    _isInCombat = true;
                    StartAttack(_currentTarget);
                }
            }
        }
    }

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

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    void Die()
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        Destroy(gameObject);
    }
}