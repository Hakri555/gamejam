
using System.Collections;
using UnityEngine;

public class ExplosiveBullet : BulletBase
{
    
    private float _explosionRadius;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public override void Init(BulletStats bulletConfig) 
    {
        _animator = GetComponent<Animator>();
        if (_animator == null) 
        {
            Debug.Log("ошибка поиск animator");
        }
        _bulletConfig = bulletConfig;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _explosionRadius = _bulletConfig.ExplosionRadius;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExplosionAnimation());
        }
    }
    protected override void OnHit()
    {
        
        // Логика взрыва (нанесение урона по области)
        Collider[] targets = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damagable))
            {
                damagable.TakeDamage(_bulletConfig.BasicDamage);
            }

        }
       
    }

    protected override void DestroyHanlder()
    {
        StopCoroutine(ExplosionAnimation());
        Destroy(gameObject,1.4f);
    }

    private IEnumerator ExplosionAnimation() 
    {
        if (_spriteRenderer == null)
        {
            Debug.Log("меняем цвет");
            yield break;
        }
        PlayExplosion();
        Destroy(gameObject, 1.35f);
        yield return new WaitForSeconds(0.8f);
        _direction = new Vector3(0, 0, 0);

    }

    public void PlayExplosion()
    {
        
        _animator.SetTrigger("ExpBullet"); // запус анимки взрыва
        OnHit();
        
    }
    


}