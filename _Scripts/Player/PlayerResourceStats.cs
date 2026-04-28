using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerStats")]
public class PlayerResourceStats : ScriptableObject
{
    [Header("Resources")]
    public int MaxHealth = 10;
    public int MaxMana = 20;
    public float ManaRegenRate = 5f;

    [Header("Combat Variables")]
    public int PhysicalResistance = 0;
    public int MagicalResistance = 0;
    public int FireResistance = 0;
    public int ElectricResistance = 0;
    public int IceResistance = 0;

}
