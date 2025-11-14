
using UnityEngine;

[CreateAssetMenu(fileName = "BossBasicBulletConfig", menuName = "Configs/Boss/Bullets")]
public class BulletStats : ScriptableObject
{
    [Header("Bullet stat configs")]

    [SerializeField] private float _baseDamage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxBulletSpeedMultiplier;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private bool _isBulletExplovsive;
    private BulletDamageType _bulletType;
    [Header("Expo bullet stats(only active if _isBulletExplovsive is true)")]
    [SerializeField] private sbyte _explosionRadius;
    [SerializeField] private float _basicChanceToSpawn;

    [Header("Expo bullet stats(only active if _isBulletExplovsive is true)")]
    [SerializeField] private bool _isShootingFromHive;
    public enum BulletDamageType 
    {
        Common,
        Explovsive
    }
    public float BasicDamage => _baseDamage;
    public float LifeTime => _lifeTime;
    public float BulletSpeed => _bulletSpeed;
    public GameObject BulletPrefab => _bullet;
    public sbyte ExplosionRadius => _explosionRadius;
    public BulletDamageType BulletType => _bulletType;
    public float Acceleration => _acceleration;
    public float MaxBulletSpeedMultiplier => _maxBulletSpeedMultiplier;
    public float BasicChanceToSpawn => _basicChanceToSpawn;
    public bool IsShootingFromHive => _isShootingFromHive;

    private void OnValidate()
    {
        _baseDamage = Mathf.Max(0,_baseDamage);

        _lifeTime = Mathf.Max(0, _lifeTime);

        _bulletSpeed = Mathf.Max(0, _bulletSpeed);

        if (_bullet == null) 
        {
            Debug.LogError($"Please Assign bullet prefab in {this}");
        }
        if (_isBulletExplovsive == true)
        {
            _bulletType = BulletDamageType.Explovsive;

        }
        else
        {
            _bulletType = BulletDamageType.Common;
            _explosionRadius = -1;
        }
    }
}
