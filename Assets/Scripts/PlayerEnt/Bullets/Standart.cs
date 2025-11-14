using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartBullet : BulletBase
{

    public override void Init(BulletStats bulletConfig)
    {
        _bulletConfig = bulletConfig;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnHit();
        }
    }
    protected override void DestroyHanlder()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    protected override void OnHit()
    {
        Debug.Log("Логика standart bullet onhit отработала");
        DestroyHanlder();
    }
}
