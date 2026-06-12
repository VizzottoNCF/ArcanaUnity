using Unity.Cinemachine;
using UnityEngine;

public class VolcanoTrigger : MonoBehaviour
{
    public VolcanoExplosion exp;
    private CinemachineImpulseSource cis;

    private void Start() => cis = GetComponent<CinemachineImpulseSource>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("Explosion", 3f);
            cis.GenerateImpulse();
            AudioManager.Instance.Play("rumble");
            //Destroy(gameObject, 3.5f);
        }
    }

    private void Explosion() => exp.Explode();
}
