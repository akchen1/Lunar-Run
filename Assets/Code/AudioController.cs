using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private AudioClip Death;
    [SerializeField] private AudioClip Oxygen;
    [SerializeField] private AudioClip Cheese;
    [SerializeField] private AudioClip Meteor;
    [SerializeField] private AudioClip ButtonClick;

    private Dictionary<string, AudioClip> sfxLib;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        sfxLib = new Dictionary<string, AudioClip>();
        sfxLib.Add("death", Death);
        sfxLib.Add("oxygen", Oxygen);
        sfxLib.Add("cheese", Cheese);
        sfxLib.Add("meteor", Meteor);
        sfxLib.Add("buttonclick", ButtonClick);

        MusicSource.volume = PlayerPrefs.GetFloat("MUSICVOLUME", MusicSource.volume);
        SFXSource.volume = PlayerPrefs.GetFloat("SFXVOLUME", SFXSource.volume);
    }
    public void PlaySFX(string sfx)
    {
        if (!sfxLib.ContainsKey(sfx)) return;
        SFXSource.PlayOneShot(sfxLib[sfx]);
    }
}
