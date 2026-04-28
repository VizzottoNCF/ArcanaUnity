using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private List<SkillButton> spellButtons;
    [SerializeField] private PlayerKnownSpells playerKnownSpells;

    private void Awake()
    {
        Refresh_Buttons();
    }

    public void Refresh_Buttons()
    {
        foreach (SkillButton skillButton in spellButtons)
        {
            skillButton.button.interactable = true;

            // if the spell is already unlocked, disable the button
            // if the spell has a prerequisite that isn't unlocked, disable the button
            if (playerKnownSpells.IsSpellUnlocked(skillButton.linkedSpell) || (!playerKnownSpells.IsSpellUnlocked(skillButton.prerequisiteSpell) && skillButton.prerequisiteSpell != null))
            {
                skillButton.button.interactable = false;
            }

            // add listener to learn spell
            skillButton.button.onClick.AddListener(() =>
            {
                playerKnownSpells.LearnSpell(skillButton.linkedSpell);
                skillButton.button.interactable = false;
                Refresh_Buttons();
            });
        }
    }
}

[Serializable]
public class SkillButton
{
    public Button button;
    public string displayName;
    public string description;
    public Spell linkedSpell;
    public Spell prerequisiteSpell;
}