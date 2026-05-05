using System.Reflection;
using UnityEngine;

public class DestroyOnFlag : MonoBehaviour
{
    public string flagName;
    public PlayerResourceStats playerStats;

    private void Update()
    {
        if (playerStats == null) { playerStats = ServiceLocator.Get<SpellBook>().playerStats; return; }
    
        if (playerStats.GetFlag(flagName)) { Destroy(gameObject); }
    }
}
