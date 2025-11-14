using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCombatManager : MonoBehaviour
{
    [SerializeField] private BulletStats _standartBullet;
    [SerializeField] private BulletStats _exploBullet;
    
    [SerializeField] private List<GameObject> _shootingPoints;
    
    private GameObject CreateBullet(BulletStats.BulletDamageType type, Transform ShootFromWhere)
    {

        GameObject prefab = type == BulletStats.BulletDamageType.Common
            ? _standartBullet.BulletPrefab
            : _exploBullet.BulletPrefab;
        
        return Instantiate(prefab, ShootFromWhere.position, ShootFromWhere.rotation);
    }

    public void Shoot()
    {
        StopCoroutine(ShootCor());
        StartCoroutine(ShootCor());
    }
    private IEnumerator ShootCor()
    {
        float randomValue;
        sbyte counter = 0;
        GameObject bullet;

        while (counter < _shootingPoints.Count)
        {
            randomValue = Random.Range(0f, 100f);
            if (randomValue <= _exploBullet.BasicChanceToSpawn)
            {
                bullet = CreateBullet(BulletStats.BulletDamageType.Explovsive, _shootingPoints[counter].transform);
                //Debug.Log($"{_exploBullet.BasicChanceToSpawn}% сработали.");

            }
            else
            {
                bullet = CreateBullet(BulletStats.BulletDamageType.Common, _shootingPoints[counter].transform);

            }

            counter++;
            yield return null;
        }

    }
    public void ShootRows(float delayBetweenShots, float delayBetweenRows, int shotsPerRow, float timeToWait)
    {
        StartCoroutine(_ShootRows(delayBetweenShots, delayBetweenRows, shotsPerRow, timeToWait));
    }

    public void StopShootingRows()
    {
        StopCoroutine(_ShootRows(0, 0, 0, 0));
    }
    private IEnumerator _ShootRows(float delayBetweenShots, float delayBetweenRows, int shotsPerRow, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        while (true)
        {
            for (int i = 0; i < shotsPerRow; i++)
            {
                Shoot();

                if (i < shotsPerRow)
                {
                    float timer = 0;
                    while (timer < delayBetweenShots)
                    {
                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
            }
            float rowTimer = 0;
            while (rowTimer < delayBetweenRows)
            {
                rowTimer += Time.deltaTime;
                yield return null;
            }
        }





    }
}
