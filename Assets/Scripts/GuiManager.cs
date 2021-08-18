using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GuiManager : MonoBehaviour
{
    public Text health;
    public Text arrowCount;
    public Slider progress_mage, progress_ranger, progress_knight;
    public GameObject screen_death, screen_levelCompleted;
    public Slider progress_track;
    public Text levelName;

    public PerkCardGUI cardMage, cardRanger, cardKnight;

    public RawImage iconArrow, iconShield, iconSword;

    public void Start()
    {
        levelName.text = SceneManager.GetActiveScene().name;
    }

    public void UpdateTrackProgress(float val)
    {
        progress_track.value = val;
    }

    public void Update()
    {
        progress_track.value = GameManager.Instance.trackProgress;
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

    public void SetupPerkCards(int magePerkID, int rangerPerkID, int knightPerkID)
    {
        cardMage.UpdateCard(magePerkID);
        cardRanger.UpdateCard(rangerPerkID);
        cardKnight.UpdateCard(knightPerkID);
    }

    public void ShowCompleteScreen()
    {
        //RecordTelemetryData();

        screen_levelCompleted.SetActive(true);

    }

    public void ShowDeathScreen()
    {
        //RecordTelemetryData();
        screen_death.SetActive(true);
    }

    public void UpdatePerkIcons(float shieldCurrCD, float rangerCurrCD, float knightCurrCD)
    {
        iconShield.color = (shieldCurrCD<=0)? Color.white : new Color(0.33f, 0.33f, 0.33f);
        iconSword.color = (knightCurrCD <= 0) ? Color.white : new Color(0.33f, 0.33f, 0.33f);
        iconArrow.color = (rangerCurrCD <= 0) ? Color.white : new Color(0.33f, 0.33f, 0.33f);
    }

}
