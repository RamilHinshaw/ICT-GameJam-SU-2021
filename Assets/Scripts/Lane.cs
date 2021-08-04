using Enums;
using UnityEngine;

[System.Serializable]
public class Lane
{
    public Lane(int _left, int _middle, int _right)
    {
        left = (ObstacleType) _left;
        middle = (ObstacleType) _middle;
        right = (ObstacleType) _right;
    }

    //[SerializeField] public string name;
    [SerializeField] public ObstacleType left = ObstacleType.NULL;
    [SerializeField] public ObstacleType middle = ObstacleType.NULL;
    [SerializeField] public ObstacleType right = ObstacleType.NULL;
    [SerializeField] public int weight = 1;
    [SerializeField] public bool randomize = false;

    public void TelemetryObstacle()
    {
        TelemetryCounter( (int) left);
        TelemetryCounter( (int) middle);
        TelemetryCounter( (int) right);
    }

    private void TelemetryCounter(int id)
    {
        if (id == (int)ObstacleType.NULL)
            Telemetry.amountGenNull++;
        else if (id == (int)ObstacleType.Web)
            Telemetry.amountGenWebs++;
        else if (id == (int)ObstacleType.Log)
            Telemetry.amountGenLogs++;
        else if (id == (int)ObstacleType.Rock)
            Telemetry.amountGenRocks++;
        else if (id == (int)ObstacleType.Enemies)
            Telemetry.amountGenDragons++;
    }
}
