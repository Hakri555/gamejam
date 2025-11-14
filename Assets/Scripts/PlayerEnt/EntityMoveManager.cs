using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveManager : MonoBehaviour, IMovable
{
    [SerializeField] bossMovementConfig MovementConfig;
    private float _basicSpeed;
    private float _aceleration;
    private List<GameObject> objects;
    public void Awake()
    {
        _basicSpeed = MovementConfig.MovementSpeed;
        _aceleration = MovementConfig.AcelerationSpeed;
    }
    public void Update()
    {

    }


    public void MoveToTheEnemy()
    {
        //if (_currentTarget != null)
        //{
        //    float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.transform.position);

        //    if (distanceToTarget > stopDistance)
        //    {
        //        transform.position = Vector3.MoveTowards(
        //            transform.position,
        //            _currentTarget.transform.position,
        //            MovementSpeed * Time.deltaTime
        //        );
        //    }

        //}

        //else
        //{
        //    // Достигли цели - начинаем атаку
        //    if (_currentTarget.CompareTag("Base"))
        //    {
        //        _hasReachedBase = true;
        //        StartAttack(_currentTarget);
        //    }
        //    else if (_currentTarget.CompareTag("Robot"))
        //    {
        //        _isInCombat = true;
        //        StartAttack(_currentTarget);
        //    }
        //}


    }
}
