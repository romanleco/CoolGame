using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("MusicManager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [SerializeField] private AudioClip[] _loopableMusic;
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _baseMusic;
    [SerializeField] private AudioSource _audioSource;
    public AudioSource fXPlayer { get; private set;}

    void Start()
    {
        DataContainer data = SaveManager.Instance.Load();
        if(data != null)
        {
            AdjustVolume(data.volume);
        }

        fXPlayer = this.GetComponentInChildren<AudioSource>();
    }

    public void PlayLoopable()
    {
        _audioSource.clip = _loopableMusic[Random.Range(0, _loopableMusic.Length)];
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayMainMenu()
    {
        _audioSource.clip = _mainMenuMusic;
        _audioSource.Play();
    }

    public void PlayBase()
    {
        _audioSource.clip = _baseMusic;
        _audioSource.Play();
    }

    public void AdjustVolume(float vol)
    {
        float volFloat = vol;
        _audioSource.volume = volFloat / 100;
        SaveManager.Instance.AdjustVolume(vol, true);
    }
}
