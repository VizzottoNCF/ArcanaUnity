using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    private Coroutine FadeInCoroutine;
    private Coroutine FadeOutCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } 
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name, bool fadeIn = false)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) { Debug.LogWarning($"Sound {name} not found"); return; }

        if (FadeInCoroutine != null && s.fade)
            StopCoroutine(FadeInCoroutine);
        
        
        s.source.Play();
        if (s.fade)
            FadeInCoroutine = StartCoroutine(FadeVolume(s, FadeInCoroutine, 0f, s.volume, .2f));
    }

    public void StopPlaying(string name)
    {
        if (FadeOutCoroutine != null)
            StopCoroutine(FadeOutCoroutine);

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) { Debug.LogWarning($"Sound {name} not found"); return; }
        
        FadeOutCoroutine = StartCoroutine(FadeVolume(s, FadeOutCoroutine, s.volume, 0f, .2f));
    }

    public IEnumerator FadeVolume(Sound s, Coroutine c, float startVol, float endVol, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            s.source.volume = Mathf.Lerp(startVol, endVol, t / duration);
            yield return null;
        }
        s.source.volume = endVol;

        if (endVol == 0f)
            s.source.Stop();
    }
}