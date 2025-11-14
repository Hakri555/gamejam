using UnityEngine;
using System.Collections;
using TMPro;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected BulletStats _bulletConfig;
    protected float _dynamicBulletSpeed;
    protected Rigidbody2D _bulletRigidbody;
    protected GameObject _player;
    protected Vector2 _direction;

    public abstract void Init(BulletStats bulletConfig);

    private void Awake()
    {
        _bulletRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _dynamicBulletSpeed = _bulletConfig.BulletSpeed;
        Init(_bulletConfig);
        FindPlayer();
        gameObject.transform.rotation *= Quaternion.Euler(0, 0, 90);
    }

    private void Start()
    {
        if (!_bulletConfig.IsShootingFromHive) 
        {
            _direction =  CalculateDirection(_player); 
            ApplyRotation();
        }
        else
        {
            
            _direction = gameObject.transform.rotation * Vector2.right;
        }
        StartCoroutine(DestroyAfterLifetime());
        
    }
    private void Update()
    {
        
        FlyInDirection();
        
    }


    private Vector2 CalculateDirection(GameObject Destianation)
    {
        if (_player == null) 
        { 
            Debug.LogError("Ошибка получения _player в CalculateDrirection в BulletBase");
        };
        return  (Destianation.transform.position - transform.position).normalized;
    }

    private void ApplyRotation()
    {
        if (_player == null) return;
        float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, -angle + 90);
    }

    public virtual void FlyInDirection()
    {
        if (_dynamicBulletSpeed < _bulletConfig.BulletSpeed * _bulletConfig.MaxBulletSpeedMultiplier)
            _dynamicBulletSpeed += Time.fixedDeltaTime * _bulletConfig.Acceleration;
        gameObject.transform.position += (Vector3)_direction * _dynamicBulletSpeed * Time.deltaTime;
    }
    
    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(_bulletConfig.LifeTime);
        DestroyHanlder();
    }

    protected virtual void DestroyHanlder()
    {
        Destroy(gameObject);
    }
    private void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Enemy");
        if (_player == null)
        {
            Debug.LogError("Enemy not found!");
            Destroy(gameObject);
        }
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);
    
    protected abstract void OnHit();
}