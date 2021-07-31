using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        Web,
        Enemies,
        Rock,
        Hole
    }

    public ObstacleType obstacleType;
}
