using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GuiManager : MonoBehaviour
{

    public void Start()
    {

    }

    public void Update()
    {
        
    }

    public void StartEndScreen()
    {
        
    }

    private void WriteToCSV()
    {
        string filename = Application.dataPath + "/test.csv";
        Debug.Log("WRITTEN TO " + filename);
        TextWriter tw = new StreamWriter(filename);

        tw.WriteLine(Stats.hitObstacles.ToString() + "," + Stats.arrowsUsed.ToString() + "," + Stats.slashUsed.ToString() +
            "," + Stats.shieldUsed.ToString() + "," + Stats.arrowsHit.ToString() + "," + Stats.slashHit.ToString() + "," + Stats.shieldHit.ToString());

        tw.Close();
    }

}
