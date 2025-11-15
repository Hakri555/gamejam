using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : MonoBehaviour
{
    [Serializable]
    public class RaritySpread
    {
        public float ironChance;
        public float copperChance;
        public float adamantiumChance;
        public float coalChance;
    }
    public int asteroidSpawnRate = 0;
    public int asteroidProgression = 0;
    public int asteroidOreNum = 0;
    public int clawReach = 3;
    public int clawSpeed = 4;
    [SerializeReference] public RaritySpread raritySpreadTier1 = new RaritySpread();
    [SerializeReference] public RaritySpread raritySpreadTier2 = new RaritySpread();
    [SerializeReference] public RaritySpread raritySpreadTier3 = new RaritySpread();
}
