using UnityEngine;
using UnityEngine.UI;

public class UI_HealthMana : MonoBehaviour
{
    [Header("Health")]
    public Image healthFillImage; 
    public Health health;
    public float maxHealth;
    public float currentHealth;
    public float DisplayedHealth;
    public float BaseMaxHealth = 5f;

    [Header("Mana")]
    public Image manaFillImage; 
    public float maxMana;
    public float currentMana;
    public float DisplayedMana;
    public float BaseMaxMana = 20f;

    [Header("Configuration")]
    public PlayerResourceStats playerStats;
    public SpellBook spellbook;
    public float BaseWidth = 256f;
    public float BaseHeight = 50f;
    public float smoothSpeed = 5f;

    void Start()
    {
        maxMana = playerStats.MaxMana;
        maxHealth = playerStats.MaxHealth;
        SetMaxHealth(maxHealth);
        SetMaxMana(maxMana);


        currentHealth = maxHealth;
        DisplayedHealth = maxHealth;
        currentMana = maxMana;
        DisplayedMana = maxMana;

        UpdateHealthBarUI();
    }

    void Update()
    {
        DisplayedHealth = Mathf.Lerp(DisplayedHealth, currentHealth, Time.deltaTime * smoothSpeed);
        DisplayedMana = Mathf.Lerp(DisplayedMana, currentMana, Time.deltaTime * smoothSpeed);

        currentHealth = health.health;
        currentMana = spellbook.currentMana;

        UpdateHealthBarUI();
    }
    private void UpdateHealthBarUI()
    {
        healthFillImage.fillAmount = DisplayedHealth / maxHealth;
        manaFillImage.fillAmount = DisplayedMana / maxMana;
    }

    public void SetHealth(float newHealth) { currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth); }
    public void SetMana(float newHealth) { currentMana = Mathf.Clamp(newHealth, 0f, maxHealth); }
    public void SetMaxHealth(float newMax) 
    {
        maxHealth = newMax;
        healthFillImage.gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((maxHealth / BaseMaxHealth) * BaseWidth, BaseHeight);
        healthFillImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((maxHealth / BaseMaxHealth) * BaseWidth, BaseHeight);
    }
    public void SetMaxMana(float newMax) 
    {
        maxMana= newMax;
        manaFillImage.gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((maxMana / BaseMaxMana) * BaseWidth, BaseHeight / 2);
        manaFillImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((maxMana / BaseMaxMana) * BaseWidth, BaseHeight / 2);
    }
}
