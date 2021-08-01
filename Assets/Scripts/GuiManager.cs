using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GuiManager : MonoBehaviour
{
    public Text health;
    public Text arrowCount;
    public Slider progress_mage, progress_ranger, progress_knight;
    public GameObject screen_death, screen_levelCompleted;

    public void Start()
    {

    }

    public void Update()
    {
        
    }

    public void UpdateArrows(int _arrows)
    {
        arrowCount.text = "x" + _arrows.ToString();
    }

    public void UpdateHealth(int _health)
    {
        health.text = _health.ToString();
    }

    public void SetupSliders(float mage, float ranger, float knight)
    {
        //Max size of sliders
        progress_knight.maxValue = knight;
        progress_mage.maxValue = mage;
        progress_ranger.maxValue = ranger;

        UpdateSliders(mage, ranger, knight);

    }

    public void UpdateSliders(float mage, float ranger, float knight)
    {
        progress_ranger.value = ranger;
        progress_knight.value = knight;
        progress_mage.value = mage;
    }

    public void ShowCompleteScreen()
    {
        if (GameManager.Instance.isCSVLogging)
            WriteToCSV();

        screen_levelCompleted.SetActive(true);

    }

    public void ShowDeathScreen()
    {
        screen_death.SetActive(true);
    }

    private void WriteToCSV()
    {
        string filename = Application.dataPath + "/telemetrics.csv";
        Debug.Log("WRITTEN TO " + filename);
        TextWriter tw = new StreamWriter(filename, true);

        tw.WriteLine(Stats.hitObstacles.ToString() + "," + Stats.arrowsUsed.ToString() + "," + Stats.slashUsed.ToString() +
            "," + Stats.shieldUsed.ToString() + "," + Stats.arrowsHit.ToString() + "," + Stats.slashHit.ToString() + "," + Stats.shieldHit.ToString());

        tw.Close();
    }

}
