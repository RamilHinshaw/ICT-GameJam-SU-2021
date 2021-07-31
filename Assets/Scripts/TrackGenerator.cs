using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField] public List<Segment> segments = new List<Segment>(); //SEGMENT SETTINGS

    public List<Segment> generatedTrack = new List<Segment>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTrack()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Segment segment = new Segment(segments[i].GenerateSegment());
            generatedTrack.Add(segment);
        }
    }
}
