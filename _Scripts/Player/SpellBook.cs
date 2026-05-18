using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [Header("Resources")]
    public PlayerResourceStats playerStats;
    public float maxMana;
    public float currentMana;
    [SerializeField] private float manaRegenRate = 5f;
    [SerializeField] private float regenDelay = 1f;
    private Coroutine regenCoroutine;

    [Header("Changing Spells")]
    public bool _isChangingSpell = false;
    [SerializeField] private int _currentChangingSpellSlot = 0;
    [SerializeField] private bool _isCastingSpell = false;
    [SerializeField] private GameObject _spellChangeGrid;
    [SerializeField] private GameObject _equippedSpellLeft;
    [SerializeField] private GameObject _equippedSpellRight;
    [SerializeField] private Image _LeftSpellIcon;
    [SerializeField] private Image _RightSpellIcon;
    [SerializeField] private GameObject _spellButtonPrefab;
    public PlayerKnownSpells _playerKnownSpells;

    [Header("Equipped Spells")]
    public List<Spell> _spell;
    [SerializeField] private List<float> _cooldownTime;
    [SerializeField] private List<float> _activeTime;


    public enum re_SpellState
    {
        READY,
        ACTIVE,
        COOLDOWN,
        INACTIVE
    }

    [Header("Spell State / Bindings")]
    [SerializeField] private List<re_SpellState> _state;
    [SerializeField] private List<bool> _spellInputPress;
    [SerializeField] private List<bool> _spellInputHeld;
    [SerializeField] private List<bool> _spellInputRelease;

    private void Awake() => ServiceLocator.Register<SpellBook>(this);
    private void Start()
    {

        if (_spellInputPress.Count == 0) { _spellInputPress.Add(false); _spellInputPress.Add(false); }
        if (_spellInputHeld.Count == 0) { _spellInputHeld.Add(false); _spellInputHeld.Add(false); }
        if (_spellInputRelease.Count == 0) { _spellInputRelease.Add(false); _spellInputRelease.Add(false); }
        if (_cooldownTime.Count == 0) { _cooldownTime.Add(0f); _cooldownTime.Add(0f); }
        if (_activeTime.Count == 0) { _activeTime.Add(0f); _activeTime.Add(0f); }
        if (_spell.Count == 0) { _spell.Add(null); _spell.Add(null); }

        rf_ChangeSpell(_spell[0], 0);
        rf_ChangeSpell(_spell[1], 1);

        manaRegenRate = playerStats.ManaRegenRate;
        maxMana = playerStats.MaxMana;
        currentMana = maxMana;
    }

    void Update()
    {
        if (GameController.Instance.IsDead) { return; }
        if (currentMana > maxMana) { currentMana = maxMana; }


        // load spell input
        _spellInputPress[0] = InputManager.spellLeftWasPressed;
        _spellInputRelease[0] = InputManager.spellLeftWasPressed;
        _spellInputHeld[0] = InputManager.spellLeftWasPressed;

        _spellInputPress[1] = InputManager.spellRightWasPressed;
        _spellInputRelease[1] = InputManager.spellRightWasPressed;
        _spellInputHeld[1] = InputManager.spellRightWasPressed;

        // run spell state machine for both spell slots if there isn't any spell on cooldown
        if (_spell[0] != null) { SpellStateMachine(0); }
        if (_spell[1] != null) { SpellStateMachine(1); }

        //if (Input.GetKeyDown(KeyCode.K)) { ToggleSpellChangeMenu(); }
    }

    public void UseMana(float amount)
    {
        if (currentMana > 0)
        {
            currentMana = Mathf.Max(currentMana - amount, 0);

            if (regenCoroutine != null) { StopCoroutine(regenCoroutine); }
            regenCoroutine = StartCoroutine(RegenMana());
        }
    }

    private IEnumerator RegenMana()
    {
        yield return new WaitForSeconds(regenDelay);
        while (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            yield return null;
        }

        currentMana = maxMana;
    }

    public void rf_ChangeSpell(Spell NewSpell, int slot)
    {
        if (NewSpell == null)
        {
            _spell[slot] = null;
            _state[slot] = re_SpellState.INACTIVE;
            return;
        }

        _spell[slot] = NewSpell;
        _activeTime[slot] = NewSpell.rf_ReadActiveTime();
        _cooldownTime[slot] = NewSpell.rf_ReadCooldownTime();
        _state[slot] = re_SpellState.READY;

        
        if (slot == 0) { _LeftSpellIcon.sprite = NewSpell._icon; }
        else           { _RightSpellIcon.sprite = NewSpell._icon; }
    }

    private void SpellStateMachine(int slot)
    {
        if (_isChangingSpell) { return; }

        CooldownFade_VFX(slot == 0 ? _LeftSpellIcon.gameObject : _RightSpellIcon.gameObject, slot, _state[slot]);

        // state for spell availability
        switch (_state[slot])
        {
            case re_SpellState.READY:
                if (_spellInputRelease[slot] && (!_isCastingSpell || _spell[0] == _spell[1]) && _spell[slot].manaCost <= currentMana)
                {
                    _spell[slot].rf_Activate(gameObject);
                    _state[slot] = re_SpellState.ACTIVE;
                    _activeTime[slot] = _spell[slot].rf_ReadActiveTime();
                    UseMana(_spell[slot].manaCost);
                }
                break;


            case re_SpellState.ACTIVE:
                if (_activeTime[slot] > 0)
                {
                    _isCastingSpell = true;
                    _activeTime[slot] -= Time.deltaTime;
                }
                else
                {
                    _state[slot] = re_SpellState.COOLDOWN;
                    _cooldownTime[slot] = _spell[slot].rf_ReadCooldownTime();
                }
                break;


            case re_SpellState.COOLDOWN:
                if (_cooldownTime[slot] > 0)
                {
                    _isCastingSpell = true;
                    _spell[slot].rf_BeginCooldown(gameObject);
                    _cooldownTime[slot] -= Time.deltaTime;
                }
                else { _state[slot] = re_SpellState.READY; _isCastingSpell = false; }
                break;

            case re_SpellState.INACTIVE:
                break;

            default:
                Debug.LogWarning("SPELL STATE DEFAULTED: SOMETHING WENT WRONG.");
                break;
        }
    }

    public void CooldownFade_VFX(GameObject spellIcon, int slot, re_SpellState state)
    {
        Transform cooldownVFX = spellIcon.transform.GetChild(0);
        if (state == re_SpellState.INACTIVE || state == re_SpellState.READY) { cooldownVFX.gameObject.GetComponent<Image>().fillAmount = 0f; return; }

        float p = (_cooldownTime[slot] / _spell[slot].rf_ReadCooldownTime());
        cooldownVFX.gameObject.GetComponent<Image>().fillAmount = p;
    }

    public void OpenSpellChangeMenu()
    {
        _equippedSpellLeft.GetComponent<Button>().interactable = true;
        _equippedSpellRight.GetComponent<Button>().interactable = true;

        // TODO: replace with sprites later
        if (_currentChangingSpellSlot == 0) { _equippedSpellLeft.GetComponent<Image>().color = Color.yellow; }
        else { _equippedSpellRight.GetComponent<Image>().color = Color.yellow; }

        foreach (Spell spell in _playerKnownSpells.knownSpells)
        {
            GameObject s = Instantiate(_spellButtonPrefab, _spellChangeGrid.transform);

            // error prevention: Image and Icon exists
            Image imageComponent = s.GetComponent<Image>();
            Button buttonComponent = s.GetComponent<Button>();
            if (imageComponent == null) { Debug.LogError("Prefab " + _spellButtonPrefab.name + " does not have an Image component!"); continue; }
            if (spell._icon == null) { Debug.LogWarning("Spell " + spell.name + " has no icon!"); continue; }

            // swap
            imageComponent.sprite = spell._icon;
            buttonComponent.onClick.AddListener(() => { rf_ChangeSpell(spell, _currentChangingSpellSlot); });

            // Hover EventTrigger
            EventTrigger trigger = s.GetComponent<EventTrigger>();
            if (trigger == null) { trigger = s.AddComponent<EventTrigger>(); }

            // Pointer Enter
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => { ShowSpellDetails(spell); });
            trigger.triggers.Add(entryEnter);

            // Pointer Exit
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => { HideSpellDetails(spell); });
            trigger.triggers.Add(entryExit);
        }
    }

    public void CloseSpellChangeMenu()
    {
        Color b = new Color(0.5188679f, 0.2928829f, 0.1933517f, 1f);
        _equippedSpellLeft.GetComponent<Button>().interactable = false;
        _equippedSpellLeft.GetComponent<Image>().color = b;
        _equippedSpellRight.GetComponent<Button>().interactable = false;
        _equippedSpellRight.GetComponent<Image>().color = b;

        foreach (Transform child in _spellChangeGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ToggleSpellChangeMenu()
    {
        if (_spellChangeGrid.activeSelf) 
        { 
            _isChangingSpell = false;
            _spellChangeGrid.SetActive(false); 
            CloseSpellChangeMenu(); 
        }
        else 
        {
            _isChangingSpell = true;
            _spellChangeGrid.SetActive(true); 
            OpenSpellChangeMenu(); 
        }
    }

    public void ShowSpellDetails(Spell s)
    {
        Debug.Log("Showing details for spell: " + s.name);
    }

    public void HideSpellDetails(Spell s)
    {
        Debug.Log("Hiding details for spell: " + s.name);
        return;
    }

    public void SetCurrentChangingSpellSlot(GameObject button)
    {
        if (!_isChangingSpell) { return; }
        
        //defaults to slot 0 and changes if its slot 1
        int slot = 0;
        if (button == _equippedSpellRight) { slot = 1; }

        _currentChangingSpellSlot = slot;

        //TODO: SPRITE LATER
        Color b = new Color(0.5188679f, 0.2928829f, 0.1933517f, 1f);
        _equippedSpellLeft.GetComponent<Image>().color = b;
        _equippedSpellRight.GetComponent<Image>().color = b;

        button.GetComponent<Image>().color = Color.yellow;


    }
}