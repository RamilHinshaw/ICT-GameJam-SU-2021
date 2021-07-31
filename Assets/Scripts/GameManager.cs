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


    private void Start()
    {
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



