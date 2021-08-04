using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class TrackGenerator : MonoBehaviour
{
    const float CENTER_SPAWN = 4.257f;
    const float LEFT_SPAWN = CENTER_SPAWN - 1;
    const float RIGHT_SPAWN = CENTER_SPAWN + 1;
    const float START_SPAWN_Z = -4f;

    [SerializeField] public List<Segment> segments = new List<Segment>(); //SEGMENT SETTINGS

    public List<Segment> generatedTrack = new List<Segment>();

    public List<GameObject> obstaclePrefabs = new List<GameObject>();

    public GameObject trackObj;
    public Renderer trackRend;
    public GameObject endZone;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrack();

        SpawnTrack(generatedTrack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTrack()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Segment segment = new Segment(segments[i].GenerateSegment(), segments[i].spaceBetweenLanes);
            generatedTrack.Add(segment);
        }
    }

    private void SpawnTrack(List<Segment> track)
    {
        float zSpawnOffset = 0; //Keep track of the distance of each object to spawn

        for (int i = 0; i < track.Count; i++)
        {
      
            for (int j = 0; j < track[i].lanes.Count; j++)
            {


                //Spawn for this lane

                //LEFT
                if (track[i].lanes[j].left != ObstacleType.NULL)
                    Instantiate(obstaclePrefabs[(int)track[i].lanes[j].left], new Vector3(LEFT_SPAWN, 4.23f, START_SPAWN_Z - zSpawnOffset), Quaternion.identity);

                //MIDDLE
                if (track[i].lanes[j].middle != ObstacleType.NULL)
                    Instantiate(obstaclePrefabs[(int)track[i].lanes[j].middle], new Vector3(CENTER_SPAWN, 4.23f, START_SPAWN_Z - zSpawnOffset), Quaternion.identity);

                //RIGHT
                if (track[i].lanes[j].right != ObstacleType.NULL)
                    Instantiate(obstaclePrefabs[(int)track[i].lanes[j].right], new Vector3(RIGHT_SPAWN, 4.23f, START_SPAWN_Z - zSpawnOffset), Quaternion.identity);

                zSpawnOffset += track[i].spaceBetweenLanes;

            }

        }


        //Stretch Track
        zSpawnOffset += 15f;
        trackObj.transform.localScale = new Vector3(trackObj.transform.localScale.x, trackObj.transform.localScale.y, zSpawnOffset);

        //END ZONE
        var endObj = Instantiate(endZone, new Vector3(CENTER_SPAWN, 4.23f, START_SPAWN_Z - zSpawnOffset + 5), Quaternion.identity);
        GameManager.Instance.endTrigger = endObj;

        //Adjust Track Texture
        Material mat = trackRend.material;
        mat.SetTextureScale("_MainTex", new Vector2(5, zSpawnOffset + zSpawnOffset * 0.15f) );
        print(zSpawnOffset);
    }
}
