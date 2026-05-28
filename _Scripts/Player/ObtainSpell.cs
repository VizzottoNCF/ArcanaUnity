using System.Reflection;
using UnityEngine;

public class ObtainSpell : MonoBehaviour
{
    public Spell spellToObtain;
    public string flagName;
    public SpellBook spellBook;
    public PlayerResourceStats playerStats;
    public string soundName = "obtain";
    private void Start()
    {
        spellBook = ServiceLocator.Get<SpellBook>();
        playerStats = spellBook.playerStats;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (spellToObtain != null) { spellBook._playerKnownSpells.LearnSpell(spellToObtain); }

            playerStats.SetFlag(flagName, true);
            
            AudioManager.Instance.Play(soundName);
        }
    }
}
