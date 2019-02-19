using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource lowSource;
    public AudioSource midSource;
    public AudioSource highSource;
    public float lowPitchRange = 0.0f;
    public float highPitchRange = 0.0f;
    public float measure = 0.0f;

    public SoundControl baseAudio;
    public SoundControl midAudio;
    public SoundControl highAudio;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        lowPitchRange = 0.25f;
        highPitchRange = 1.75f;

        baseAudio = new SoundControl();
        midAudio = new SoundControl();
        highAudio = new SoundControl();

        FormAudio(false);
    }

    void Update()
    {
        baseAudio.PlaySoundLine(lowSource);
        midAudio.PlaySoundLine(midSource);
        highAudio.PlaySoundLine(highSource);
    }

    public void FormAudio(bool tension)
    {

        if (tension)
        {
            measure = Random.Range(1.0f, 3.0f);
        }
        else
        {
            measure = Random.Range(10.0f, 20.0f);
        }

        baseAudio.CalculateAudio(measure, 3, 7, lowPitchRange, highPitchRange);
        midAudio.CalculateAudio(measure, 2, 6, lowPitchRange, highPitchRange);
        highAudio.CalculateAudio(measure, 5, 10, lowPitchRange, highPitchRange);

    }

  
}
