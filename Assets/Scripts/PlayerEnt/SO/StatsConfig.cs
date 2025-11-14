using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "entStatsConfig", menuName = "Configs/ent/stats")]
public class entBasicStatsConfig : ScriptableObject
{
    [Header("Max basic stats")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxArmor;
    [SerializeField] private float _regenSpeed;
    [SerializeField] private float _maxDpsToTake;

    public float MaxHealth => _maxHealth;
    public float MaxArmor => _maxArmor;
    public float RegenSpeed => _regenSpeed;
    public float MaxDpsToTake => _maxDpsToTake;
    public void OnValidate()
    {
        _maxHealth = Mathf.Max(0,_maxHealth);

        _maxArmor = Mathf.Clamp(_maxArmor ,0, 100);

        _regenSpeed = Mathf.Clamp(_regenSpeed ,0, _maxHealth / 10);

        _maxDpsToTake = Mathf.Max(0,_maxDpsToTake);

    }
}
