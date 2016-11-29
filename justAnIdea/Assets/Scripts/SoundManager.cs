using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] soundList;

    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        soundList = GetComponents<AudioSource>();
        soundList[0].Play();
    }

    /****************************************
        Sound Indices:
        0: background track
        1: boss death
        2: death sound
        3: pickup sound
        4: hit sound
        5: jump sound 1
        6: jump sound 2
        7: player shot sound
        8: enemy shot sound
    ****************************************/
    public void PlaySound(int index)
    {
        if (index == 0 && soundList[0].isPlaying)
        {
            Debug.Log("Not playing the sound ha!");
        }
        else
        {
            soundList[index].Play();
        }
    }

}