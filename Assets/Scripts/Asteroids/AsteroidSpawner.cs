using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    
    [SerializeField] int spawnerTier;
    [SerializeField] UpgradeState upgradeState;
    [SerializeField] Asteroid asteroid;
    
    float minSpawnTime;
    float maxSpawnTime;
    int minNumOfOre;
    int maxNumOfOre;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        if (spawnerTier == 0)
        {
            minNumOfOre = 2+upgradeState.asteroidOreNum * 5;
            maxNumOfOre = 5+upgradeState.asteroidOreNum * 5;
            minSpawnTime = 12;
            maxSpawnTime = 14;
        }
        else if (spawnerTier == 1)
        {
            minNumOfOre = 5 + upgradeState.asteroidOreNum * 5;
            maxNumOfOre = 8 + upgradeState.asteroidOreNum * 5;
            minSpawnTime = 15;
            maxSpawnTime = 20;
        }
        else if (spawnerTier == 2)
        {
            minNumOfOre = 15 + upgradeState.asteroidOreNum * 2;
            maxNumOfOre = 20 + upgradeState.asteroidOreNum * 2;
            minSpawnTime = 50;
            maxSpawnTime = 100;
        }
        else
        {
            minSpawnTime = 0;
            maxSpawnTime = 0;
        }
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            if (minSpawnTime == 0) break;
            yield return new WaitForSeconds(Random.Range(minSpawnTime-upgradeState.asteroidSpawnRate, maxSpawnTime-upgradeState.asteroidSpawnRate));
            float chance = Random.value;
            Asteroid newAsteroid = Asteroid.Instantiate(asteroid, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
            newAsteroid.asteroidInfo.speed = Random.Range(0.5f, 0.7f);
            UpgradeState.RaritySpread rarity = null;
            switch (spawnerTier){
                case 0:
                    rarity = upgradeState.raritySpreadTier1;
                    newAsteroid.asteroidInfo.size = Asteroid.SMALL;
                    break;
                case 1:
                    newAsteroid.asteroidInfo.size = Asteroid.MEDIUM;
                    rarity = upgradeState.raritySpreadTier2;
                    break;
                case 2:
                    newAsteroid.asteroidInfo.size = Asteroid.LARGE;
                    rarity = upgradeState.raritySpreadTier3;
                    break;
                default:
                    newAsteroid.asteroidInfo.size = Asteroid.SMALL;
                    newAsteroid.asteroidInfo.type = Asteroid.COAL;
                    break;
            }
            if (rarity != null)
            {
                float spawnChance = 0;
                if (chance <= rarity.ironChance)
                {
                    newAsteroid.asteroidInfo.type = Asteroid.IRON;
                }
                spawnChance += rarity.ironChance;
                if (chance >= rarity.ironChance && chance <= (spawnChance + rarity.copperChance))
                {
                    newAsteroid.asteroidInfo.type = Asteroid.COPPER;
                }
                spawnChance += rarity.copperChance;
                if (chance >= spawnChance && chance <= (spawnChance + rarity.adamantiumChance))
                {
                    newAsteroid.asteroidInfo.type = Asteroid.ADAMANTIUM;
                }
                spawnChance += rarity.adamantiumChance;
                if (chance > spawnChance)
                {
                    newAsteroid.asteroidInfo.type = Asteroid.COAL;
                }
            }
            
            newAsteroid.asteroidInfo.oreAmount = Random.Range(minNumOfOre, maxNumOfOre);
        }
    }
}
