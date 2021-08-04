
using System.IO;
public static class Telemetry
{
    public static int userID = -1;
    public static string level;


    public static int hitObstacles = 0;
    public static int arrowsUsed = 0;
    public static int slashUsed = 0;
    public static int shieldUsed = 0;

    public static int arrowsHit = 0;
    public static int slashHit = 0;
    public static int shieldHit = 0;

    public static float timeInStage = 0;

    public static bool playerDied = false;

    public static int amountGenRocks, amountGenDragons, amountGenLogs, amountGenWebs, amountGenNull;
    public static int jumpsUsed = 0;


    public static int remainingHealth;
    public static int arrowsLeft;

    public static int damagedFromRock, damagedFromDragon, damagedFromWeb, damagedFromLog;

    public static float trackProgress = 0f;

    public static void Reset()
    {
        //userID = -1;
        level = string.Empty;

        hitObstacles = 0;
        arrowsUsed = 0;
        slashUsed = 0;
        shieldUsed = 0;
        arrowsHit = 0;
        slashHit = 0;
        shieldHit = 0;
        timeInStage = 0;
        playerDied = false;
        amountGenRocks = amountGenDragons = amountGenLogs = amountGenWebs = amountGenNull = 0;
        jumpsUsed = 0;
        remainingHealth = 0;
        arrowsLeft = 0;
        damagedFromRock = damagedFromDragon = damagedFromLog = 0;
        trackProgress = 0f;

    }

    public static void GenerateUserID()
    {
        userID = UnityEngine.Random.Range(10000, 99999);
    }

    public static void WriteToFile(string path)
    {
        TextWriter tw = new StreamWriter(path, true);

        tw.WriteLine(

            userID.ToString() + "," +
            level.ToString() + "," +
            hitObstacles.ToString() + "," +
            arrowsUsed.ToString() + "," +
            slashUsed.ToString() + "," +
            shieldUsed.ToString() + "," +
            arrowsHit.ToString() + "," +
            slashHit.ToString() + "," +
            shieldHit.ToString() + "," +
            timeInStage.ToString() + "," +
            playerDied.ToString() + "," +
            amountGenRocks.ToString() + "," +
            amountGenDragons.ToString() + "," +
            amountGenLogs.ToString() + "," +
            amountGenNull.ToString() + "," +
            amountGenWebs.ToString() + "," +
            jumpsUsed.ToString() + "," +
            remainingHealth.ToString() + "," +
            arrowsLeft.ToString() + "," +
            damagedFromRock.ToString() + "," +
            damagedFromDragon.ToString() + "," +
            damagedFromLog.ToString() + "," +
            UnityEngine.Mathf.Ceil(trackProgress).ToString() + "%,"

            );

        tw.Close();
    }
}
