using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [HideInInspector] public float timerForLevel = 0f;


    //Super Hard Coded!
    [HideInInspector]  public PlayerController player;
    public GameObject endTrigger;
    [HideInInspector]  public Vector3 playerStartPos;
    public float trackProgress = 0f; //100% if at goal

    public void UpdatePlayerProgress()
    {
        float fullTrack = Vector3.Distance(playerStartPos, endTrigger.transform.position);
        float completedTrack = Vector3.Distance(player.transform.position, endTrigger.transform.position);

        trackProgress =  (1 - (completedTrack / fullTrack)) * 100;
        GuiManager.UpdateTrackProgress(trackProgress);
    }

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

        Telemetry.Reset();

        //Hardcoded if first level
        if (SceneManager.GetActiveScene().name == "Level 1")
            Telemetry.GenerateUserID();
    }

    private void Update()
    {
        GuiManager.Update();

        timerForLevel += Time.deltaTime;
        UpdatePlayerProgress();


        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(0);
        }
    }

}



