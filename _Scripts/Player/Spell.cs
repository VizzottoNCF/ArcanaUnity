using UnityEngine;

public class Spell : ScriptableObject
{
    [Header("Base Spell Config")]
    public new string name;
    public string description;
    public Sprite _icon;
    public int manaCost;
    [SerializeField] private SpellType _spellType;
    [SerializeField] private int _spellLevel;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _activeTime;

    public virtual void rf_Activate(GameObject Parent) { }
    public virtual void rf_BeginCooldown(GameObject Parent) { }

    public float rf_ReadActiveTime() { return _activeTime; }
    public float rf_ReadCooldownTime() { return _cooldownTime; }

    public enum SpellType
    {
        MagicSword,
        MagicMissile,
        Fireball,
        IceSpike,
        LightningBolt,

    }

    public string ReadSpellTypeName(SpellType st)
    {
        switch (st)
        {
            case SpellType.MagicSword:
                return "Sword Sorcery";
            case SpellType.MagicMissile:
                return "Magical Sorcery";
            case SpellType.Fireball:
                return "Fire Sorcery";
            case SpellType.IceSpike:
                return "Ice Sorcery";
            case SpellType.LightningBolt:
                return "Lightning Sorcery";
        }
        return "ERROR: INVALID SPELLTYPE";
    }
}