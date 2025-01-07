using UnityEngine;

public class RippleVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    /// <summary>
    /// Function to call a ripple effect alongside an audio cue for deaf accessibility
    /// </summary>
    /// <param name="Position">The transform where the ripple effect will be spawned</param>
    /// <param name="StartSize">The initial size of the ripple effect</param>
    /// <param name="Speed">The speed of the ripple expansion</param>
    /// <param name="Duration">The duration of the ripple effect</param>
    public void rf_SpawnRipple(Transform Position, float StartSize = 15, float Speed = 1, float Duration = 2)
    {
        ParticleSystem instantiatedParticleSystem = Instantiate(_particleSystem, Position.position, Position.rotation);

        ParticleSystem.MainModule mainModule = instantiatedParticleSystem.main;

        mainModule.duration = Duration;
        mainModule.startSize = StartSize;
        mainModule.startSpeed = Speed;

        _particleSystem.transform.position = Position.position;

        _particleSystem.Play();
    }
}

