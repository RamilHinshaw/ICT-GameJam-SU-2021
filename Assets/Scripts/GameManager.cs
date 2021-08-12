using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Enums;

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
    public bool disableTrackProgress = false;

    [Header("Perks")]
    public List<Perk> perks = new List<Perk>();
    public List<int> magePerksID = new List<int>();
    public List<int> rangerPerksID = new List<int>();
    public List<int> knightPerksID = new List<int>();

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        audioSource = GetComponent<AudioSource>();

        Telemetry.Reset();

        //Hardcoded if first level
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            Telemetry.GenerateUserID();

            
        }

        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Telemetry.userID = -1;
        }

        InitPerks();
        InitPlayer();
    }

    private void InitPerks()
    {
        //Reset Available Perks if LVL 1
        if (SceneManager.GetActiveScene().name == "Level 1" || PlayerStats.availableMagePerksID.Count == 0)
        {
            PlayerStats.availableMagePerksID = magePerksID;
            PlayerStats.availableRangerPerksID = rangerPerksID;
            PlayerStats.availableKnightPerksID = knightPerksID;

            PlayerStats.ResetPerks();

            PlayerStats.shieldDuration = player.shieldMaxDuration;
            PlayerStats.arrowCount = player.arrowCount;
            PlayerStats.playerHealth = player.health;
        }

        int rdmMagePerk = PlayerStats.availableMagePerksID[Random.Range(0, PlayerStats.availableMagePerksID.Count)];
        int rdmRangerPerk = PlayerStats.availableRangerPerksID[Random.Range(0, PlayerStats.availableRangerPerksID.Count)];
        int rdmKnightPerk = PlayerStats.availableKnightPerksID[Random.Range(0, PlayerStats.availableKnightPerksID.Count)];

        GuiManager.SetupPerkCards(rdmMagePerk, rdmRangerPerk, rdmKnightPerk);
    }

    public void InitPlayer()
    {
        //CALLED BY PLAYER!
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            player.arrowCount = player.arrowMaxCount;
        }

        else
        {
            player.arrowCount = PlayerStats.arrowCount;
            player.health = PlayerStats.playerHealth;
            player.shieldMaxDuration = PlayerStats.shieldDuration;
        }

        player.shieldCurrentCD = player.shieldMaxDuration;

        GuiManager.SetupSliders(player.shieldCD, player.arrowCD, player.slashCD);
        GuiManager.UpdateHealth(player.health);
        GuiManager.UpdateArrows(player.arrowCount);

        playerStartPos = player.transform.position;
    }

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



    private void Update()
    {
        GuiManager.Update();

        timerForLevel += Time.deltaTime;

        if (!disableTrackProgress)
            UpdatePlayerProgress();


        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RecordTelemetryData()
    {
        if (GameManager.Instance.isCSVLogging)
        {
            Telemetry.level = SceneManager.GetActiveScene().name;
            Telemetry.timeInStage = GameManager.Instance.timerForLevel;
            Telemetry.trackProgress = GameManager.Instance.trackProgress;
            WriteToCSV();
        }
    }

    private void WriteToCSV()
    {
        string filename = Application.dataPath + "/telemetrics.csv";
        Debug.Log("WRITTEN TO " + filename);
        Telemetry.WriteToFile(filename);
    }

}



