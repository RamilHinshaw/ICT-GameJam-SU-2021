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

    [SerializeField] public ObstacleType left = ObstacleType.NULL;
    [SerializeField] public ObstacleType middle = ObstacleType.NULL;
    [SerializeField] public ObstacleType right = ObstacleType.NULL;
    [SerializeField] public int weight = 1;
    [SerializeField] public bool randomize = false;
}
