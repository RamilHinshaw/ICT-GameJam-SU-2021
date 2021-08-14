using System.Collections;
using System.Collections.Generic;
using Enums;


public static class PlayerStats
{

    public static int playerHealth = 10;
    public static bool swordSizeIncrease = false;
    public static bool regenHealthIfNotHit = false;

    public static int arrowCount = 5;
    public static int arrowPiercing = 0;
    public static bool arrowJumpPerk = false;
    //public static int arrowJumpCount = 0;

    public static bool shieldLogToArrow = false;
    public static float shieldDuration = 0.5f;
    public static bool shieldReduceOtherCD = false;


    
    public static List<int> availableMagePerksID = new List<int>();
    public static List<int> availableRangerPerksID = new List<int>();
    public static List<int> availableKnightPerksID = new List<int>();

    public static void ResetPerks()
    {
      playerHealth = 10;
      swordSizeIncrease = false;
      regenHealthIfNotHit = false;

      arrowCount = 5;
      arrowPiercing = 0;
      arrowJumpPerk = false;

      shieldLogToArrow = false;
      shieldDuration = 0.5f;
      shieldReduceOtherCD = false;
    }

    public static void ActivatePerk(int perkID)
    {
        //UnityEngine.Debug.Log("PERK ACTIVATED!: " + perkID);

        Perk perk = GameManager.Instance.perks[perkID];
        PerkTypes perkType = perk.perkType;

        switch (perkType)
        {
            case PerkTypes.KnightHealthIncrease:
                playerHealth += 6;
                break;

            case PerkTypes.KnightSwordIncrease:
                swordSizeIncrease=true;
                if (perk.removeOnSelection) availableKnightPerksID.Remove(perkID);
                break;

            case PerkTypes.KnightRegenHealth:
                regenHealthIfNotHit = true;
                if (perk.removeOnSelection) availableKnightPerksID.Remove(perkID);
                break;

            case PerkTypes.RangerArrowIncrease:
                arrowCount += 5;
                break;

            case PerkTypes.RangerArrowPiecing:
                arrowPiercing++;
                break;

            case PerkTypes.RangerArrowJumpIncrease:
                arrowJumpPerk = true;
                if (perk.removeOnSelection) availableRangerPerksID.Remove(perkID);
                break;

            case PerkTypes.MageShieldDuration:
                shieldDuration += 1;
                break;

            case PerkTypes.MageShieldLogArrow:
                shieldLogToArrow = true;
                if (perk.removeOnSelection) availableMagePerksID.Remove(perkID);
                break;

            case PerkTypes.MageShieldOtherCD:
                shieldReduceOtherCD = true;
                if (perk.removeOnSelection) availableMagePerksID.Remove(perkID);
                break;

            default:
                break;
        }

        //Telemetry
        UnityEngine.Debug.Log(perkID);
        Telemetry.perkSelected = perk.name;
    }
}
