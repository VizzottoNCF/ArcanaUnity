using UnityEngine;

public class ActivateLava : MonoBehaviour
{
    private bool l = false;
    public LavaSequence lava;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && l == false) { lava.StartSequence(); l = true; }
    }
}