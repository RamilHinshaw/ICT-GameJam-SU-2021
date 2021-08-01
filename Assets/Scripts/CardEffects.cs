using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    public void IncreaseHealth(int num)
    {
        PlayerStats.playerHealth += num;
    }

    public void IncreaseArrowCount(int num)
    {
        PlayerStats.arrowCount += num;
    }

    public void IncreaseShieldDur(int num)
    {
        PlayerStats.shieldDuration += num;
    }


}
