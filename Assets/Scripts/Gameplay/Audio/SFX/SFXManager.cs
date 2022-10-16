using System;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private SFXFile[] _SFXFiles;
    private readonly Dictionary<string, SFXFile> _SFX = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (SFXFile s in _SFXFiles)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            _SFX.Add(s.name, s);
        }
    }

    public void PlayOneShot(string name)
    {
        SFXFile s = _SFX[name];
        if (s == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }

        s.source.PlayOneShot(s.clip);
    }
}