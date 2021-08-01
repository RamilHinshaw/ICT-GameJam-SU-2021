using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GameManger is a singleton Monobehavior
public class GameManager : MonoBehaviour
{

    #region Instance

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                print("Instance of GameObject does not exist!");

            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public bool isCSVLogging = false;

    //MANAGERS HERE    
    public GuiManager GuiManager;

    private AudioSource audioSource;//, audioSource_sfx;
    public AudioClip music_fanfare, music_death, sfx_destroyed;

    public void PlayFanfare()
    {
        //audioSource_music.PlayOneShot(music_fanfare);
        audioSource.clip = music_fanfare;
        audioSource.Play();
    }

    public void PlayDeath()
    {
        //audioSource_music.PlayOneShot(music_death);
        audioSource.clip = music_death;
        audioSource.Play();
    }

    public void PlayDestroyedSfx()
    {
        audioSource.PlayOneShot(sfx_destroyed);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Stats.hitObstacles = 0;
        Stats.arrowsUsed = 0;
        Stats.slashUsed = 0;
        Stats.shieldUsed = 0;
        Stats.arrowsHit = 0;
        Stats.slashHit = 0;
        Stats.shieldHit = 0;
    }

    private void Update()
    {
        GuiManager.Update();

    }

}



