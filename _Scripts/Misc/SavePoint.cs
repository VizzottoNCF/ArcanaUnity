using TMPro;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public SpellBook spellBook;
    private Collider2D _col;
    public bool playerInRange = false;
    public TMP_Text interactText;

    private void Start() { spellBook = ServiceLocator.Get<SpellBook>(); _col = GetComponent<Collider2D>(); }

    private void Update()
    {
        if (interactText != null) { interactText.enabled = playerInRange; }
        if (spellBook == null) { spellBook = ServiceLocator.Get<SpellBook>(); }
        if (!playerInRange) { return; }

        if (Input.GetKeyDown(KeyCode.E)) 
        { 
            // full heal and spawn point
            spellBook.gameObject.GetComponent<Health>().ChangeHealth(spellBook.gameObject.GetComponent<Health>().maxHealth);

            // reset dead enemies
            EnemySaveSystem.ResetSave();

            // spell change
            spellBook.ToggleSpellChangeMenu(); 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) => playerInRange = true;
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
        if (spellBook._isChangingSpell) { spellBook.ToggleSpellChangeMenu(); }
    }
}
