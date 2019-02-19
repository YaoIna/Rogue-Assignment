using UnityEngine;
using System.Collections;

public class SoundControl 
{
    public float[] pitchRanges;
    public float[] playTimes;
    public float playTimer;
    public float interval;
    public float intervalTimer;
    public int cordCount;
    public int rangeCount;

    public SoundControl()
    {
        playTimer = 0.0f;
        interval = 0.0f;
        intervalTimer = 0.0f;
        cordCount = 0;
        rangeCount = 0;
    }

    public void CalculateAudio(float measure, int minFreq, int maxFreq, float low, float high)
    {
        float playTotal = 0.0f;
        float lastPitch = Random.Range(low, high);
        int switchPitchCount = Random.Range(3, maxFreq);
        int switchPitch = 0;
        int pitchDir = Random.Range(0, 2);

        cordCount = Random.Range(minFreq, maxFreq);
        playTimes = new float[cordCount];
        pitchRanges = new float[cordCount];
        for (int i = 0; i < cordCount; i++)
        {
            playTimes[i] = Random.Range(minFreq / cordCount, measure / cordCount);
            playTotal += playTimes[i];
            if (pitchDir == 0)
            {
                lastPitch = pitchRanges[i] = Random.Range(low, lastPitch);
            }
            else if (pitchDir == 1)
            {
                lastPitch = pitchRanges[i] = Random.Range(lastPitch, high);
            }
            switchPitch++;
            if (switchPitch == switchPitchCount)
            {
                if (pitchDir == 0)
                    pitchDir = 1;
                else
                    pitchDir = 0;
            }
        }
        playTimer = playTimes[0];

        interval = (measure - playTotal) / cordCount;
        intervalTimer = interval;
    }

    public void PlaySoundLine(AudioSource source)
    {

        if (rangeCount >= cordCount)
        {
            rangeCount = 0;
        }

        if (playTimer > 0)
        {
            playTimer -= Time.deltaTime;
            if (!source.isPlaying)
            {
                source.pitch = pitchRanges[rangeCount];
                source.Play();
                rangeCount++;
            }
        }
        else if (playTimer <= 0)
        {
            source.Stop();

            if (intervalTimer > 0)
            {
                intervalTimer -= Time.deltaTime;
            }
            else if (intervalTimer <= 0)
            {
                playTimer = playTimes[rangeCount];
                intervalTimer = interval;
            }
        }
    }
}
