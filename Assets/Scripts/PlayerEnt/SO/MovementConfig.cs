
using UnityEngine;

[CreateAssetMenu(fileName = "BossMovementConfig", menuName = "Configs/Boss/movement")]
public class bossMovementConfig : ScriptableObject
{
    [Header("Max basic stats")]

    [SerializeField]private float _basicMovementSpeed;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private int _acelerationSpeed;
    
    public int AcelerationSpeed  => _acelerationSpeed;
    public float MovementSpeed => _maxMovementSpeed;
    public float BasicMovementSpeed => _basicMovementSpeed;

    public void OnValidate()
    {
        _basicMovementSpeed =  _basicMovementSpeed > 0 ? _basicMovementSpeed : 1;

        _maxMovementSpeed = _maxMovementSpeed > 0 && _maxMovementSpeed >= _basicMovementSpeed ? MovementSpeed : _basicMovementSpeed;

        _acelerationSpeed = Mathf.Max(0, _acelerationSpeed);
        
    }
}
