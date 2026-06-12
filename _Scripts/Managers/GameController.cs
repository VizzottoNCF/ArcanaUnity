using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Unlockables")]


    [Header("Stats")]
    public static GameController Instance;
    public bool CanPlayerMove = true;
    public bool InKnockback = false;
    public bool IsPlayerGrounded = true;
    public bool IsDead = false;
    public bool isTimeSlowed = false;
    public bool CanTakeDamage = true;

    [Header("References")]
    [SerializeField] private GameObject _HudBar;
    [SerializeField] private GameObject _HudSpells;
    [SerializeField] private GameObject _HudRespawn;
    [SerializeField] private GameObject _HudWin;
    [SerializeField] private GameObject _HudPowerUp;
    [SerializeField] private GameObject _SpriteReference;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _startPos;
    private Animator anim;

    
    private void Awake()
    {
        // singleton instance
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        anim = GetComponent<Animator>();

        // grabs player spawn point in level and rigidbody component
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    // call this function on health script when necessary
    [ContextMenu("Cause Player Death")]
    public void rf_PlayerDeath()
    {
        IsDead = true;
        anim.SetBool("isDead", true);

        // turn off hud
        _HudBar.SetActive(false);
        _HudSpells.SetActive(false);
        _HudRespawn.SetActive(true);
    }

    public void New_PowerUp(string msg)
    {
        _HudPowerUp.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{msg} desbloqueado!";
        _HudPowerUp.SetActive(true);

        Invoke(nameof(Disable_PowerUp), 3f); 
    }
    private void Disable_PowerUp() => _HudPowerUp.SetActive(false);

    public void rf_Respawn()
    {
        bool save = false;
        if (IsDead)
        {
            AudioManager.Instance.StopAllSongs();

            AudioManager.Instance.Play("standardBGM");
            // reset vars just in case
            CanPlayerMove = true;
            InKnockback = false;
            isTimeSlowed = false;
            Time.timeScale = 1f;
            IsDead = false;
            anim.SetBool("isDead", false);

            // restore hud
            _HudBar.SetActive(true);
            _HudSpells.SetActive(true);
            _HudRespawn.SetActive(false);

            // restore health
            Health h = GetComponent<Health>();
            h.health = h.maxHealth;

            // reset enemy date
            EnemySaveSystem.ResetSave();
            if (SceneManager.GetActiveScene().name == "WL_Vulcan") { save = true; }
        }

        // send player to last spawn point
        //Debug.LogWarning("Respawn load");
        RoomTransitionManager srv = ServiceLocator.Get<RoomTransitionManager>();
        
        srv.EnterRoom(srv.getCurrentRoom(), srv.currSpawn, save);
    }
    public void rf_RespawnNoLoad()
    {
        // send player to last spawn point
        //Debug.LogWarning("Respawn no load");
        RoomTransitionManager srv = ServiceLocator.Get<RoomTransitionManager>();
        srv.TeleportToSpawnPoint();
    }

    public void rf_WinGame()
    {
        CanPlayerMove = false;
        CanTakeDamage = false;

        _HudWin.SetActive(true);
    }
}
