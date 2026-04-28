using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerSpells")]
public class PlayerKnownSpells : ScriptableObject
{
    public event EventHandler OnSpellUnlocked;
    public class OnSpellUnlockedEventArgs : EventArgs { public Spell unlockedSpell; }
    public List<Spell> knownSpells = new List<Spell>();

    public void LearnSpell(Spell newSpell)
    {
        if (!knownSpells.Contains(newSpell))
        {
            knownSpells.Add(newSpell);
            OnSpellUnlocked?.Invoke(this, new OnSpellUnlockedEventArgs { unlockedSpell = newSpell });
        }
    }

    public bool IsSpellUnlocked(Spell spell) { return knownSpells.Contains(spell); }
}