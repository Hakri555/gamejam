using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UpgradeRequirements
{
    public int amount;
    public int type;
}

public class UpgradeState : MonoBehaviour
{
    [System.Serializable]
    public class RaritySpread
    {
        public float ironChance;
        public float copperChance;
        public float adamantiumChance;
        public float coalChance;
    }

    [Header("Требования к улучшению скорости")]
    public List<UpgradeRequirements> speedUpgrades;

    [Header("Требования к улучшению дальности")]
    public List<UpgradeRequirements> reachUpgrades;

    [Header("Требования к улучшению эффективности")]
    public List<UpgradeRequirements> processingUpgrades;

    [SerializeReference] public RaritySpread raritySpreadTier1 = new RaritySpread();

    [SerializeReference] public RaritySpread raritySpreadTier2 = new RaritySpread();

    [SerializeReference] public RaritySpread raritySpreadTier3 = new RaritySpread();

    public int asteroidSpawnRate = 0;
    public int asteroidOreNum = 0;
    public int clawReach = 3;
    public int clawSpeed = 4;
    
    
    public void UpgradeSpeed()
    {

    }
    public void UpgradeReach()
    {
        switch (clawReach)
        {
            case 3:
                break;
        }
    }
}
