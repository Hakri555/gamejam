using System;
using UnityEngine;

public class EntManager : MonoBehaviour, IDamageable
{
    //[SerializeField] entBasicStatsConfig ScriptableObject;

    private float _health;
    private float _armor;
    private float _regen;
    private float _maxDamageToTake;

    public float Health
    {
        get => _health;
        set
        {
            if (value < 0)
                throw new ArgumentException("Health cannot be negative");
            _health = value;
        }
    }

    public float Armor
    {
        get => _armor;
        set
        {
            if (value < 0)
                throw new ArgumentException("Armor cannot be negative");
            _armor = value;
        }
    }

    public float Regen
    {
        get => _regen;
        set
        {
            if (value < 0)
                throw new ArgumentException("Regeneration cannot be negative");
            _regen = value;
        }
    }

    public float MaxDamageToTake
    {
        get => _maxDamageToTake;
        set
        {
            if (value < 0)
                throw new ArgumentException("Max damage cannot be negative");
            _maxDamageToTake = value;
        }
    }

    /*public void Awake()
    {
        _health = ScriptableObject.MaxHealth;
        _armor = ScriptableObject.MaxArmor;
        _regen = ScriptableObject.RegenSpeed;
        _maxDamageToTake = ScriptableObject.MaxDpsToTake;
    }*/


    public void TakeDamage(float damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }
    }
}