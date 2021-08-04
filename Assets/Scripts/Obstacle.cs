using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Obstacle : MonoBehaviour
{
    //public enum ObstacleType
    //{
    //    Web,
    //    Enemies,
    //    Rock,
    //    Hole
    //}

    public ObstacleType obstacleType;
    public GameObject particle;

    public void Explode()
    {
        TelemetryDamagedFrom( (int) obstacleType);
        Destroy(gameObject);
        Instantiate(particle, transform.position, transform.rotation);
        GameManager.Instance.PlayDestroyedSfx();
    }

    private void TelemetryDamagedFrom(int id)
    {
        if (id == (int)ObstacleType.Web)
            Telemetry.damagedFromWeb++;
        else if (id == (int)ObstacleType.Log)
            Telemetry.damagedFromLog++;
        else if (id == (int)ObstacleType.Rock)
            Telemetry.damagedFromRock++;
        else if (id == (int)ObstacleType.Enemies)
            Telemetry.damagedFromDragon++;
    }
}
