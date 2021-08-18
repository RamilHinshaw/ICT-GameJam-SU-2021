using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float health = 10;
    private float startHealth;
    public float startSpeed = 3f;
    public float maxSpeed = 12f;
    public float attackDelay = 0.75f;
    public float maxIdleTimer = 5f;

    [Header("The lower it is, the more difficult the dragon is!")]
    [Range(0.01f, 1f)]
    public float difficultyModifier = 0.15f;
   // private float currentDifficulty;

    [Header("Setup")]
    public Animator anim;
    public AudioSource audioSource, gameManagerAudioSource, AS_dragonDeath;
    public GameObject spawner;
    public GameObject projectile, explosion;
    public AudioClip sfx_fireball, sfx_damaged, sfx_death;
    public AudioClip music_fanfare;
    public Slider ui_healthSlider;
    public GameObject ui_completionScreen;

    private float timer = 2.4f;


    private Behavior behavior = Behavior.Idle;

    private const float MIDDLE_LANE = 4.241f;
    public int lanePos = 0; // 0 is middle, -1 is left, 1 is right
    private int targetLane;

    private bool enraged = false;
    private int enragedAttackCounter = 0;
    private int enragedTargetAttackCounter = 0;
    public int minEnragedAttack = 3;
    public int maxEnragedAttack = 20;
    public float enragedCooldown = 6f;
    private float enragedCurrentCooldown = 0f;
    public Renderer rend;
    public Material normalMat, enragedMat;
    public GameObject glowBody, body;

    public enum Behavior
    {
        Idle,
        MoveToLane,
        Attack        
    }

    //public enum Phases
    //{
    //    phase1,
    //    phase2,
    //    phase3
    //}

    // Start is called before the first frame update
    void Start()
    {
        ui_healthSlider.maxValue = health;
        ui_healthSlider.value = health;

        startHealth = health;
        Telemetry.bossHealth = (int)startHealth;

        //for (int i = 0; i < rend.materials.Length; i++)
        //{
        //    print(rend.materials[i].name);
        //}

        print(rend.materials[3].name);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.player.isDisabled) return;

        AI_LOGIC();
    }

    private void AI_LOGIC()
    {
        switch (behavior)
        {
            case Behavior.Idle:
                Idle();
                break;
            case Behavior.MoveToLane:
                MoveToLane();
                break;
            case Behavior.Attack:
                Attack();
                break;
            default:
                break;
        }

        if (enragedCurrentCooldown > 0)
            enragedCurrentCooldown -= Time.deltaTime;
    }

    private void Idle()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {

            //IF HALF HEALTH FOLLOW PLAYER POSITION!
            if (Random.value < 0.70f)
            //if ((health / startHealth) >= 0.5f && Random.value <= 0.85f)
            {
                print("Followed Player!");
                targetLane = GameManager.Instance.player.lanePos * -1;
                behavior = Behavior.MoveToLane;
            }

            //else if ((health / startHealth) <= 0.5f && Random.value <= 0.25f)
            //{
            //    print("Followed Player!");
            //    targetLane = GameManager.Instance.player.lanePos * -1;
            //    behavior = Behavior.MoveToLane;
            //}

            else
            {
                targetLane = Random.Range(-1, 2);
                behavior = Behavior.MoveToLane;
            }

        }
    }

    private void Attack()
    {

        timer -= Time.deltaTime;

        if (timer >= 0) return;
        //WAIT FOR ATTACK DELAY!

        anim.SetTrigger("attack");

        //Spawn Prefab
        if (enraged)
            enragedAttackCounter++;

        Instantiate(projectile, spawner.transform.position, spawner.transform.rotation);
        audioSource.PlayOneShot(sfx_fireball);


        //if (enraged)
        //    return;

        //IF HALF HEALTH ATTACK MULTIPLE TIMES!
        if ( (health/startHealth) <= 0.5f && Random.value < 0.55f && enraged == false && enragedCurrentCooldown <= 0f)
        {
            enraged = true;
            //rend.materials[3] = enragedMat;
            //rend.materials[3].SetColor("_EmissionColor", new Color(.75f, .141f, .188f, 0.1472708f));
            //rend.materials[3].EnableKeyword("Emmision");
            rend.materials[3].EnableKeyword("Emmision");
            glowBody.SetActive(true);
            body.SetActive(false);

            audioSource.PlayOneShot(sfx_death);
            timer = 1.75f;

            enragedAttackCounter = 0;

            enragedTargetAttackCounter = (int) Random.Range(minEnragedAttack + (maxEnragedAttack * (1 - (health / startHealth))) / 3, maxEnragedAttack * (1 - (health / startHealth)));

        }

        if (enraged && enragedAttackCounter < enragedTargetAttackCounter)
        {
            timer -= Time.deltaTime; //Wait to go crazy!

            if (timer > 0) return;


            print("ENRAGED ATTACK!");
            timer = (attackDelay / 2) * CurrentDifficulty();
            targetLane = Random.Range(-1, 2);
            behavior = Behavior.MoveToLane;
            return;
        }


        else if (enraged == true)
        {
            enraged = false;
            //rend.materials[3] = normalMat;
            rend.materials[3].DisableKeyword("Emmision");
            glowBody.SetActive(false);
            body.SetActive(true);

            enragedCurrentCooldown = (enragedCooldown * CurrentDifficulty()) * 2f;

            //Wait time and idle longer
            timer = Random.Range(.15f * CurrentDifficulty(), maxIdleTimer * CurrentDifficulty());
            behavior = Behavior.Idle;
            timer *= 2f;
            return;
        }



        //Switch to IDLE!
        timer = Random.Range(.15f * CurrentDifficulty(), maxIdleTimer * CurrentDifficulty());
        behavior = Behavior.Idle;
    }

    private void MoveToLane()
    {     
        if (lanePos == targetLane)
        {
            behavior = Behavior.Attack;
            return;
        }


        Vector3 newPos = new Vector3(MIDDLE_LANE + targetLane, transform.position.y, transform.position.z);
        float speed = startSpeed + (maxSpeed - startSpeed) *  ( 1 - (health / startHealth) );

        if (enraged)
            speed *= 2f;

        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
                
        //Attack if in position
        if (transform.position == newPos)
        {
            lanePos = targetLane;
            timer = attackDelay * CurrentDifficulty() ;

            if (enraged)
                timer /= 2f;

            behavior = Behavior.Attack;
        }

    }

    private float CurrentDifficulty()
    {
        float difficulty = ((1 - difficultyModifier) * (health / startHealth) + difficultyModifier);

        //print(difficulty);
        return difficulty;
    }

    public void Damage()
    {
        //Lower Health
        health--;
        Telemetry.bossHealth = (int)health;
        ui_healthSlider.value = health;
        audioSource.PlayOneShot(sfx_damaged);
        Instantiate(explosion, spawner.transform.position, transform.rotation);

        if (health <= 0)
            Death();
        //Update UI
    }

    private void Death()
    {
        if (GameManager.Instance.player.isDisabled) return;

        GameManager.Instance.player.isDisabled = true;
        GameManager.Instance.player.currentSpeed = 0;
        GameManager.Instance.player.DisableAnim();

    
        gameManagerAudioSource.Stop();
        gameManagerAudioSource.clip = music_fanfare;
        gameManagerAudioSource.Play();
        ui_completionScreen.SetActive(true);
        gameObject.SetActive(false);

        AS_dragonDeath.enabled = true;

    }


}
