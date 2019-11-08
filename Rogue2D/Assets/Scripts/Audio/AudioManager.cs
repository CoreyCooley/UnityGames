using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

   public static AudioManager instance;

   public AudioSource levelMusic;
   public AudioSource gameOverMusic;
   public AudioSource victoryMusic;

   public AudioSource[] sfx;

    private void Awake() 
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayNewAudio(AudioSource stopAudio, AudioSource startAudio)
    {
        stopAudio.Stop();

        startAudio.Play();
    }

    public void PlayGameOver()
    {
        PlayNewAudio(levelMusic, gameOverMusic);
    }

    public void PlayVictory()
    {
        PlayNewAudio(levelMusic, victoryMusic);
    }

    public void PlaySFX(int sfxIndex)    
    {
        PlayNewAudio(sfx[sfxIndex], sfx[sfxIndex]);
    }
}
