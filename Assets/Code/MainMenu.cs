using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private bool controlsActive;
    private bool creditsActive;
    private bool optionsActive;

    [SerializeField] private Animator controlsAnim;
    [SerializeField] private Animator creditsAnim;
    [SerializeField] private Animator optionsAnim;

    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider SensSlider;

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;

    public Slider.SliderEvent AdjustMusicVolume { get; private set; }

    private void Start()
    {
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MUSICVOLUME", MusicSource.volume);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVOLUME", SFXSource.volume);
        SensSlider.value = PlayerPrefs.GetFloat("SENS", 0.5f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        
    }

    private void OnEnable()
    {
        MusicVolumeSlider.onValueChanged.AddListener(delegate {
            MusicSource.volume = MusicVolumeSlider.value;
            PlayerPrefs.SetFloat("MUSICVOLUME", MusicVolumeSlider.value);

	    });
        SFXVolumeSlider.onValueChanged.AddListener(delegate {
            SFXSource.volume = SFXVolumeSlider.value;
            PlayerPrefs.SetFloat("SFXVOLUME", SFXVolumeSlider.value);

        });
        SensSlider.onValueChanged.AddListener(delegate {
            PlayerPrefs.SetFloat("SENS", SensSlider.value);
        });
    }

    private void OnDisable()
    {
        MusicVolumeSlider.onValueChanged.RemoveAllListeners();
        SFXVolumeSlider.onValueChanged.RemoveAllListeners();
        SensSlider.onValueChanged.RemoveAllListeners();
    }

    public void Controls()
    {
        if (controlsActive)
        {
            controlsAnim.Play("SlideUp");
        } else
        {
            if (creditsActive)
            {
                creditsAnim.GetComponent<Canvas>().sortingOrder = 1;
                controlsAnim.GetComponent<Canvas>().sortingOrder = 2;
                creditsAnim.Play("SlideUp");
            } else if (optionsActive)
            {
                optionsAnim.GetComponent<Canvas>().sortingOrder = 1;
                controlsAnim.GetComponent<Canvas>().sortingOrder = 2;
                optionsAnim.Play("SlideUp");
            }
            controlsAnim.Play("SlideDown");
            creditsActive = false;
            optionsActive = false;
        }
        controlsActive = !controlsActive;
    }

    public void Credits()
    {
        if (creditsActive)
        {
            creditsAnim.Play("SlideUp");
        }
        else
        {
            if(controlsActive)
            {
                creditsAnim.GetComponent<Canvas>().sortingOrder = 2;
                controlsAnim.GetComponent<Canvas>().sortingOrder = 1;
                controlsAnim.Play("SlideUp");
            } else if (optionsActive)
            {
                optionsAnim.GetComponent<Canvas>().sortingOrder = 1;
                creditsAnim.GetComponent<Canvas>().sortingOrder = 2;
                optionsAnim.Play("SlideUp");
            }
            creditsAnim.Play("SlideDown");
            controlsActive = false;
            optionsActive = false;
        }
        creditsActive = !creditsActive;
    }

    public void Options()
    {
        if (optionsActive)
        {
            optionsAnim.Play("SlideUp");
        }
        else
        {
            if (controlsActive)
            {
                optionsAnim.GetComponent<Canvas>().sortingOrder = 2;
                controlsAnim.GetComponent<Canvas>().sortingOrder = 1;
                controlsAnim.Play("SlideUp");
            }
            else if (creditsActive)
            {
                creditsAnim.GetComponent<Canvas>().sortingOrder = 1;
                optionsAnim.GetComponent<Canvas>().sortingOrder = 2;
                creditsAnim.Play("SlideUp");
            }
            optionsAnim.Play("SlideDown");
            creditsActive = false;
            controlsActive = false;
        }
        optionsActive = !optionsActive;
    }
}
