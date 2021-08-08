using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkCardGUI : MonoBehaviour
{
    public Text description;
    public int perkID;


    public void UpdateCard(int _perkID)
    {
        perkID = _perkID;
        description.text = GameManager.Instance.perks[perkID].description;
    }

    public void ActivatePerk()
    {
        PlayerStats.ActivatePerk(perkID);
    }
}
