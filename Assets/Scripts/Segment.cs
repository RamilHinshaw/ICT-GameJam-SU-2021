using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment
{
    [SerializeField] public string name;
    [SerializeField] public List<Lane> lanes = new List<Lane>();
    [SerializeField] public int numberOfLanes = 5;
    [SerializeField] public float spaceBetweenLanes = 4f;


    public Segment( List<Lane> _lanes, float _spaceBetweenLanes)
    {
        lanes = _lanes;
        spaceBetweenLanes = _spaceBetweenLanes;
    }

    public Lane RandomLane(System.Random random)
    {
        int sumOfWeights = 0;

        for (int i = 0; i < lanes.Count; i++)
        {
            sumOfWeights += lanes[i].weight;
        }

        int rdm = Random.Range(1, sumOfWeights);

        int tempSum = 0; //Used to check the ranges of each weight and see what rdm falls under
        for (int i = 0; i < lanes.Count; i++)
        {
            if (rdm <= lanes[i].weight + tempSum)
            {
                if (lanes[i].randomize == false)
                    return lanes[i]; //Return LANE

                else
                {
                    List<int> intList = new List<int>();
                    intList.Add( (int) lanes[i].left);
                    intList.Add( (int) lanes[i].middle);
                    intList.Add( (int) lanes[i].right);
                    intList = RandomizeIntList(random, intList);

                    Lane rdmLane = new Lane(intList[0], intList[1], intList[2]);
                    return rdmLane;
                }

            }

            tempSum += lanes[i].weight;
        }

        //If lane is randomized


        //IF GETS HERE ERROR!
        Debug.LogError("WEIGHT RDM FAILED!");
        return lanes[0];
    }

    public List<Lane> GenerateSegment()
    {
        System.Random random = new System.Random();

        List<Lane> rdmLanes = new List<Lane>(); //Empty List

        for (int i = 0; i < numberOfLanes; i++)
        {
            Lane rdmLane = RandomLane(random);
            rdmLane.TelemetryObstacle();
            rdmLanes.Add(rdmLane);
        }

        return rdmLanes;
    }

    public List<int> RandomizeIntList(System.Random random, List<int> list)
    {
  
        List<int> newList = new List<int>();
        newList.AddRange(list);


        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            var value = newList[k];
            newList[k] = newList[n];
            newList[n] = value;
        }

        //Debug.Log("RANDOMIZED!");
        return newList;

  
    }
}
