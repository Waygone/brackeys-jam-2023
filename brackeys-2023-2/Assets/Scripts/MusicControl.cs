using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    private AudioSource musicSource;

    [SerializeField] private AudioClip endgameMusic;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();

        if (GlobalData.State == GlobalData.GameState.LEVEL_1_END_GAME)
        {
            musicSource.clip = endgameMusic; 
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    private void Update()
    {
        musicSource.volume = GlobalData.MusicVolume / 100f;
    }
}
