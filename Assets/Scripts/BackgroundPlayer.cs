using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;

    private int clipCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clips[clipCount];
            audioSource.Play();
            increasePlaylistCount();
        }

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    audioSource.Stop();
        //    increasePlaylistCount();
        //    audioSource.clip = clips[clipCount];
        //    audioSource.Play();
        //}
    }

    void increasePlaylistCount()
    {
        clipCount = (clipCount + 1) % clips.Length;
        Debug.Log(clipCount);
    }
}
