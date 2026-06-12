using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMTrigger : MonoBehaviour
{
    public string bgmName;
    public const string defaultBGM = "standardBGM";
    public bool hasDelay = false;
    public float delayTime = 3f;
    private float timer = 0f;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDelay) return;

        if (collision.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(bgmName))
                bgmName = defaultBGM;

            if (!AudioManager.Instance.IsPlaying(bgmName))
            {
                AudioManager.Instance.StopAllSongs();
                AudioManager.Instance.Play(bgmName);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasDelay) return;
        if (collision.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer >= delayTime && !hasPlayed)
            {
                if (!AudioManager.Instance.IsPlaying(bgmName))
                {
                    AudioManager.Instance.StopAllSongs();
                    AudioManager.Instance.Play(bgmName);
                    hasPlayed = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        { 
            if (defaultBGM == bgmName) return;
            if (SceneManager.GetActiveScene().name == "WL_Vulcan") return;

            if (!AudioManager.Instance.IsPlaying(defaultBGM))
            {
                AudioManager.Instance.StopAllSongs();
                AudioManager.Instance.Play(defaultBGM);
            }
        }        
    }
}
