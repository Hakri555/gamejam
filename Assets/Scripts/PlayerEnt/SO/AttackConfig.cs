

using UnityEngine;

[CreateAssetMenu(fileName = "BossAttackConfig", menuName = "Configs/Boss/attaclstats")]
public class bossAttackConfig : ScriptableObject
{
    [Header("Boss basic attack settings")]

    [SerializeField] private float _basicDamage;
    [SerializeField] private float _critDamage;
    public enum AttackTypes
    {
        Physical,
        Explovsive
    };
    public float BasicDamage => _basicDamage;
    public float CritDamage => _critDamage;

    public void OnValidate()
    {
        _basicDamage = Mathf.Clamp(_basicDamage,1, _basicDamage);
        _critDamage = Mathf.Clamp(_critDamage,BasicDamage, _basicDamage * 5);
    }
}
