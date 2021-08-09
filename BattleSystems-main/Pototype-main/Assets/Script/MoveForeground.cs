using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForeground : MonoBehaviour
{
    private bool shouldLerp = false;
    public bool isForeground = false;
    public Database database;
    public int mapID = 0;
    private float timeStartedLerping, lerpTime = 1.75f;

    private Vector2 endPosition, startPosition;
    public MoveForeground nextMap, firstMap;

    public void StartLerping()
    {
        enabled = true;
        timeStartedLerping = Time.time;

        if (database.mapLerpingNumber < 3)
        {
            database.mapLerpingNumber++;
            shouldLerp = true;
            nextMap.StartLerping();
        }
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }

    private void setMapPosition(int mapID)
    {
        startPosition = new Vector2(mapID * 23.75f, 0);
        endPosition = new Vector2(startPosition.x - 23.75f, 0);
        transform.position = startPosition;
        transform.name = "Foreground" + mapID;
    }

    private void Start()
    {
        if (isForeground)
        {
            setMapPosition(mapID);
            if (mapID == 0)
            {
                firstMap = GetComponent<MoveForeground>();
            }
            if (mapID != 2)
            {
                nextMap = Instantiate(gameObject).GetComponent<MoveForeground>();
                nextMap.mapID = mapID + 1;
                nextMap.firstMap = firstMap;
            }
            else
            {
                nextMap = firstMap;
            }
        }
        enabled = false;
    }

    private void FixedUpdate()
    {
        if (shouldLerp)
        {
           transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
            if ((Vector2)transform.position == endPosition)
            {
                enabled = false;
                mapID--;
                if (mapID < 0)
                {
                    mapID = 2;
                }
                setMapPosition(mapID);

                if (mapID == 0)
                {
                    database.currentWave++;
                    database.CreateEnemy();
                    database.mapLerpingNumber = 0;
                }
            }
        }
    }
}
