using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerResourceStats playerStats;
    [SerializeField] private EnemyConfig enemyStats;
    public event Action<Vector2> OnDamage;
    public event Action<Vector2> OnDeath;
    public int health;
    public int maxHealth;
    public List<int> resistances;

    private void Awake()
    {
        health = maxHealth;
        if (resistances.Count == 0)
        {
            resistances = new List<int>();
            for (int i = 0; i < Enum.GetValues(typeof(re_DamageType)).Length; i++) { resistances.Add(0); }
        }
        if (playerStats != null) { InitializeResistances(); }
        else { InitializeEnemyResistances(); };
    }

        public void ApplyResistance(int damage, re_DamageType damageType, Vector2 sourcePosition = default)
        {
            
            int damageDone = damage - resistances[(int)damageType];

            if (damageDone > 0) { ChangeHealth(-damageDone, sourcePosition); }
            else { damageDone = 0; }

            // TODO: DISPLAY HOW MUCH DAMAGE WAS TAKEN

        }

    public void ChangeHealth(int amount, Vector2 sourcePosition = default)
    {
        health += amount;

        if (health > maxHealth) { health = maxHealth; }
        else if (health <= 0) { OnDeath?.Invoke(sourcePosition); }
        else if (amount < 0) { OnDamage?.Invoke(sourcePosition); }
    }

    private void InitializeResistances()
    {
        maxHealth = playerStats.MaxHealth;
        health = maxHealth;

        // Safety check to ensure list has enough elements
        int maxIndex = (int)re_DamageType.Ice;
        if (resistances.Count <= maxIndex)
        {
            Debug.LogWarning($"Resistances list only has {resistances.Count} elements but needs at least {maxIndex + 1}");
            return;
        }

        resistances[(int)re_DamageType.Physical] = playerStats.PhysicalResistance;
        resistances[(int)re_DamageType.Magical] = playerStats.MagicalResistance;
        resistances[(int)re_DamageType.Fire] = playerStats.FireResistance;
        resistances[(int)re_DamageType.Electric] = playerStats.ElectricResistance;
        resistances[(int)re_DamageType.Ice] = playerStats.IceResistance;
    }

    private void InitializeEnemyResistances()
    {
        maxHealth = enemyStats.maxHealth;
        health = maxHealth;

        // Safety check to ensure list has enough elements
        int maxIndex = (int)re_DamageType.Ice;
        if (resistances.Count <= maxIndex)
        {
            Debug.LogWarning($"Resistances list only has {resistances.Count} elements but needs at least {maxIndex + 1}");
            return;
        }

        resistances[(int)re_DamageType.Physical] = enemyStats.PhysicalResistance;
        resistances[(int)re_DamageType.Magical] = enemyStats.MagicalResistance;
        resistances[(int)re_DamageType.Fire] = enemyStats.FireResistance;
        resistances[(int)re_DamageType.Electric] = enemyStats.ElectricResistance;
        resistances[(int)re_DamageType.Ice] = enemyStats.IceResistance;
    }
}
public enum re_DamageType
{
    None,
    Physical,
    Magical,
    Fire,
    Electric,
    Ice
}