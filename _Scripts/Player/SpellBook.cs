using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellBook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Spell _spell;
    private float _cooldownTime;
    private float _activeTime;

    enum re_SpellState
    {
        READY,
        ACTIVE,
        COOLDOWN,
        INACTIVE
    }

    [Header("Spell State / Bindings")]
    [SerializeField] private re_SpellState _state = re_SpellState.READY;
    [SerializeField] private InputAction _spellInput;

    void Update()
    {
        switch (_state)
        {
            case re_SpellState.READY:
                if (_spellInput.WasPressedThisFrame())
                {
                    _spell.rf_Activate(gameObject);
                    _state = re_SpellState.ACTIVE;
                    _activeTime = _spell.rf_ReadActiveTime();

                }
                break;


            case re_SpellState.ACTIVE:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    _state = re_SpellState.COOLDOWN;
                    _cooldownTime = _spell.rf_ReadCooldownTime();
                }
                break;


            case re_SpellState.COOLDOWN:
                if (_cooldownTime > 0)
                {
                    _spell.rf_BeginCooldown(gameObject);
                    _cooldownTime -= Time.deltaTime;
                }
                else { _state = re_SpellState.READY; }
                break;

            case re_SpellState.INACTIVE:
                break;

            default:
                Debug.LogWarning("SPELL STATE DEFAULTED: SOMETHING WENT WRONG.");
                break;
        }
    }

    public void rf_ChangeSpell(Spell NewSpell)
    {
        _spell = NewSpell;
        _activeTime = NewSpell.rf_ReadActiveTime();
        _cooldownTime = NewSpell.rf_ReadCooldownTime();
    }

}
