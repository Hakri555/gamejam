using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeRequirements
{
    public int amount;
    public int type;
    public int upgradeBy;
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

    [SerializeField] public GameInfoDummy resourses;
    [SerializeField] Text buttonUpgradeSpeedText;
    [SerializeField] Text buttonUpgradeReachText;
    [SerializeField] Text buttonUpgradeProcessingText;

    public int asteroidSpawnRate = 0;
    public int asteroidOreNum = 0;
    public int clawReach = 3;
    public int clawSpeed = 4;
    public int speedLevel = 0;
    public int reachLevel = 0;
    public int processingLevel = 0;

    
    
    public void UpgradeSpeed()
    {
        if (speedLevel < speedUpgrades.Count)
        {
            if (resourses.reduceResourseByType(speedUpgrades[speedLevel].type, speedUpgrades[speedLevel].amount))
            {
                clawSpeed += speedUpgrades[speedLevel].upgradeBy;
                speedLevel++;
            } else
            {
                Debug.Log("Not enough");
            }
            if (speedLevel >= speedUpgrades.Count)
            {
                buttonUpgradeSpeedText.text = "speed: max";
            }
            else
            {
                buttonUpgradeSpeedText.text = "speed: " + (speedLevel+1);
            }
        }
    }
    public void UpgradeReach()
    {
        if (reachLevel < reachUpgrades.Count)
        {
            if (resourses.reduceResourseByType(reachUpgrades[reachLevel].type, reachUpgrades[reachLevel].amount))
            {
                clawReach += reachUpgrades[reachLevel].upgradeBy;
                reachLevel++;
            }
            else
            {
                Debug.Log("Not enough");
            }
            if (reachLevel >= reachUpgrades.Count)
            {
                buttonUpgradeReachText.text = "reach: max";
            }
            else
            {
                buttonUpgradeReachText.text = "reach: " + (reachLevel + 1);
            }
        }
    }

    public void UpgradeProcessing()
    {
        if (processingLevel < processingUpgrades.Count)
        {
            if (resourses.reduceResourseByType(processingUpgrades[processingLevel].type, processingUpgrades[processingLevel].amount))
            {
                asteroidOreNum += processingUpgrades[processingLevel].upgradeBy;
                processingLevel++;
            }
            else
            {
                Debug.Log("Not enough");
            }
            if (processingLevel >= processingUpgrades.Count)
            {
                buttonUpgradeProcessingText.text = "processing: max";
            }
            else
            {
                buttonUpgradeProcessingText.text = "processing: " + (processingLevel + 1);
            }
        }
    }
}
