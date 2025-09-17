using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            Destroy(gameObject);
        }
        
    }
}
