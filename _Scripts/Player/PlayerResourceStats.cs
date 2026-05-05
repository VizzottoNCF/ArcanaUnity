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

    [Header("Upgrade Flags")]
    public bool hasSwordSpell1 = false;
    public bool hasMissileSpell1 = false;
    public bool hasDoubleJump = false;

    [Header("Progression Flags")]
    public bool FirstBossDefeated = false;
    public bool SecondBossDefeated = false;

    [Header("Map Flags")]
    public bool Grasslands1DoorB = false;
    public bool Grasslands1DoorC = false;

    public bool GetFlag(string flagName)
    {
        var field = GetType().GetField(flagName);
        if (field != null && field.FieldType == typeof(bool))
        {
            return (bool)field.GetValue(this);
        }
        Debug.LogError($"Flag '{flagName}' not found or is not a boolean.");
        return false;
    }

    public void SetFlag(string flagName, bool value)
    {
        var field = GetType().GetField(flagName);
        if (field != null && field.FieldType == typeof(bool))
        {
            field.SetValue(this, value);
        }
        else
        {
            Debug.LogError($"Flag '{flagName}' not found or is not a boolean.");
        }
    }
}
