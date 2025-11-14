//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EntityMoveManager : MonoBehaviour, IMovable
//{
//    [SerializeField] bossMovementConfig MovementConfig;
//    private float _basicSpeed;
//    private float _stopDistance = 0.1f;
//    private bool _hasReachedBase;
//    private bool _isInCombat = false;
//    private float _aceleration;
//    private List<GameObject> _objectsTargets;
//    private EntityMeleeCombatManager _entityCombatManager;


//    public void Awake()
//    {
//        _entityCombatManager = gameObject.GetComponent<EntityMeleeCombatManager>();
//        _basicSpeed = MovementConfig.MovementSpeed;
//        _aceleration = MovementConfig.AcelerationSpeed;
//    }
//    public void Update()
//    {
//        if (!_hasReachedBase)
//        {
//            MoveToTheEnemy();
//        }

//    }

//    public void MoveToTheEnemy()
//    {
//        if (_objectsTargets[1] != null)
//        {
//            float distanceToTarget = Vector3.Distance(transform.position, _objectsTargets[1].transform.position);

//            if (distanceToTarget > _stopDistance)
//            {
//                transform.position = Vector3.MoveTowards(
//                    transform.position,
//                    _objectsTargets[1].transform.position,
//                    _basicSpeed * Time.deltaTime
//                );
//            }

//        }

//        else
//        {
//            // Достигли цели - начинаем атаку
//            if (_objectsTargets[1].CompareTag("Base"))
//            {
//                _hasReachedBase = true;
                
//                //StartAttack(_objectsTargets[1]);
//            }
//            else if (_objectsTargets[1].CompareTag("Robot"))
//            {
//                _isInCombat = true;
//                _entityCombatManager.StartAttack(_objectsTargets[1].GetComponent(Collider2D));
                
//            }
//        }

//    }

//}
