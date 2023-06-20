using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioInfo[] _clips;
    private AudioSource _sourcePrefab;

    private readonly Dictionary<string, AudioClip> _clipMap = new();
    private readonly Queue<AudioSource> _waitingSources = new();

    private void Awake()
    {
        Instance = this;
        _sourcePrefab = new GameObject("Sound", typeof(AudioSource)).GetComponent<AudioSource>();
        foreach (AudioInfo info in _clips)
        {
            _clipMap.Add(info.name, info.clip);
        }
    }

    public void Play(string name, float volume = 1f, float pitch = 1f)
    {
        var clip = _clipMap[name];
        if (clip == null) return;
        var source = _waitingSources.TryDequeue(out AudioSource result) ? result : Instantiate(_sourcePrefab, transform);
        source.gameObject.SetActive(true);
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
        StartCoroutine(PlayRoutine(clip.length / pitch, source));
    }

    private IEnumerator PlayRoutine(float length, AudioSource source)
    {
        yield return new WaitForSeconds(length);
        source.gameObject.SetActive(false);
        _waitingSources.Enqueue(source);
    }
}

[Serializable]
public struct AudioInfo
{
    [SerializeField] public string name;
    [SerializeField] public AudioClip clip;
}
