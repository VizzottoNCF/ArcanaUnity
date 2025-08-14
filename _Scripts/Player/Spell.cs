using UnityEngine;

public class Spell : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private int _spellLevel;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private float _activeTime;

    public virtual void rf_Activate(GameObject Parent) { }
    public virtual void rf_BeginCooldown(GameObject Parent) { }

    public float rf_ReadActiveTime() { return _activeTime; }
    public float rf_ReadCooldownTime() { return _cooldownTime; }
}