using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] lineClips;
    public AudioClip[] circleClips;
    public AudioClip[] flattenClips;
    public AudioClip[] rockClips;
    public AudioClip curveClip;


    AudioSource audioSource;
    public static SoundManager Instance;

    public static bool StopFading = false;

    void Start()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayLineClip()
    {
        float delay = 0f; //float delay = ((int)(Time.timeSinceLevelLoad * 10000f) % 250f) / 1000f;
        StopFading = true;
        Invoke("PlayLine", delay);
    }

    private void PlayLine()
    {
        audioSource.PlayOneShot(lineClips[(int)UnityEngine.Random.Range(0, lineClips.Length)]);
    }

    public void PlayRockClip()
    {
        float delay = 0f; //float delay = ((int)(Time.timeSinceLevelLoad * 10000f) % 250f) / 1000f;
        StopFading = true;
        Invoke("PlayRock", delay);
    }

    private void PlayRock()
    {
        audioSource.PlayOneShot(rockClips[(int)UnityEngine.Random.Range(0, rockClips.Length)]);
    }

    public void PlayFlattenClip()
    {
        float delay = 0f; //float delay = ((int)(Time.timeSinceLevelLoad * 10000f) % 250f) / 1000f;
        StopFading = true;
        Invoke("PlayFlatten", delay);
    }

    private void PlayFlatten()
    {
        audioSource.PlayOneShot(flattenClips[(int)UnityEngine.Random.Range(0, flattenClips.Length)]);
    }

    public void PlayCircleClip()
    {
        float delay = 0f; //float delay = ((int)(Time.timeSinceLevelLoad * 10000f) % 250f) / 1000f;
        StopFading = true;
        Invoke("PlayCircle", delay);
    }

    private void PlayCircle()
    {
        audioSource.PlayOneShot(circleClips[(int)UnityEngine.Random.Range(0, circleClips.Length)]);
    }

    public void PlayCurveClip()
    {
        float delay = 0f; //float delay = ((int)(Time.timeSinceLevelLoad * 10000f) % 250f) / 1000f;
        StopFading = true;
        Invoke("PlayCurve", delay);
    }

    private void PlayCurve()
    {
        audioSource.PlayOneShot(curveClip);
    }

    internal void StopPlaying()
    {
        StartCoroutine(FadeOut());
    }


    public  IEnumerator FadeOut(float FadeTime = 0.75f)
    {
        StopFading = false;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0 && StopFading ==  false)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }


}
