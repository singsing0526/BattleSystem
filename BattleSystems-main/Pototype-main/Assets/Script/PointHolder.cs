using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHolder : MonoBehaviour
{
    // 0 to 12 is the main path, 13 to 14 is the cage path, 15 to 17 is the tower path, 18 is the shop
    public Vector2[] pointPositions;
    public List<Point> allPoints;
    public GameObject pointPrefab, characterIconPrefab;
    private int[] mainLevel = { 0, 1, 2, 4, 6, 7, 10, 11, 12, 14, 17, 18};
    // Add avaliable levels in the list below. Eg. 0, 1, 2, 4, 6, 7, 10, 11, 12, 14, 17
    public List<int> avaliableLevel, beatedLevel;
    private GameObject characterIconHolder;
    private int currentPoint = 0;
    public enum KeyPressed { Up, Down, Left, Right};
    private KeyPressed keyPressed = KeyPressed.Right;
    private bool isTraveling = false, isMoving = false;
    private TMPro.TextMeshProUGUI textHolder;

    public void SetUp()
    {
        for (int i = 0; i < pointPositions.Length; i++)
        {
            allPoints.Add(Instantiate(pointPrefab, pointPositions[i], Quaternion.identity).GetComponent<Point>());
            if (IsLevelPoint(i))
            {
                allPoints[i].isPath = false;
            }
            allPoints[i].index = i;
            allPoints[i].name = "Point" + " " + i;
            allPoints[i].transform.SetParent(transform);
            allPoints[i].sr = allPoints[i].GetComponent<SpriteRenderer>();
            allPoints[i].transform.localScale = new Vector2(0.5f, 0.5f);
        }

        if (avaliableLevel.Contains(0) == false)
        {
            avaliableLevel.Add(0);
        }

        for (int i = 0; i < pointPositions.Length; i++)
        {
            switch (i)
            {
                case 0:
                    allPoints[i].connectingPoints[1] = allPoints[i + 1];
                    allPoints[i].connectingPoints[3] = allPoints[i + 1];
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (i == 1)
                    {
                        allPoints[i].connectingPoints[0] = allPoints[i - 1];
                        allPoints[i].connectingPoints[1] = allPoints[18];
                    }
                    if (i == 4 || i == 5)
                    {
                        allPoints[i].connectingPoints[0] = allPoints[i + 1];
                    }
                    if (i == 6)
                    {
                        allPoints[i].connectingPoints[1] = allPoints[i + 1];
                    }
                    if (i == 7)
                    {
                        allPoints[i].connectingPoints[0] = allPoints[i - 1];
                        allPoints[i].connectingPoints[1] = allPoints[13];
                    }
                    if (i == 10)
                    {
                        allPoints[i].connectingPoints[1] = allPoints[15];
                    }
                    if (i == 11)
                    {
                        allPoints[i].connectingPoints[0] = allPoints[i + 1];
                    }
                    allPoints[i].connectingPoints[2] = allPoints[i - 1];
                    allPoints[i].connectingPoints[3] = allPoints[i + 1];
                    break;
                case 12:
                    allPoints[i].connectingPoints[1] = allPoints[i - 1];
                    allPoints[i].connectingPoints[2] = allPoints[i - 1];
                    break;
                case 13:
                    allPoints[i].connectingPoints[0] = allPoints[7];
                    allPoints[i].connectingPoints[1] = allPoints[i + 1];
                    break;
                case 14:
                    allPoints[i].connectingPoints[0] = allPoints[i - 1];
                    break;
                case 15:
                    allPoints[i].connectingPoints[0] = allPoints[10];
                    allPoints[i].connectingPoints[1] = allPoints[i + 1];
                    allPoints[i].connectingPoints[3] = allPoints[10];
                    break;
                case 16:
                    allPoints[i].connectingPoints[0] = allPoints[i - 1];
                    allPoints[i].connectingPoints[1] = allPoints[i + 1];
                    allPoints[i].connectingPoints[3] = allPoints[i - 1];
                    break;
                case 17:
                    allPoints[i].connectingPoints[0] = allPoints[i - 1];
                    allPoints[i].connectingPoints[3] = allPoints[i - 1];
                    break;
                case 18:
                    allPoints[i].connectingPoints[0] = allPoints[1];
                    allPoints[i].connectingPoints[2] = allPoints[1];
                    break;

            }
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < pointPositions.Length; i++)
        {
            if (avaliableLevel.Contains(i))
            {
                if (beatedLevel.Contains(i) == false)
                {
                    allPoints[i].sr.sprite = allPoints[i].pointImages[0];
                }
                else
                {
                    allPoints[i].transform.localScale = new Vector2(1.5f, 1.5f);
                    allPoints[i].sr.sprite = allPoints[i].pointImages[1];
                }
            }
        }

        characterIconHolder = Instantiate(characterIconPrefab);
        characterIconHolder.transform.position = GetCurrentPosition(currentPoint);

        textHolder = Instantiate(transform.parent.GetComponent<CrossSceneManagement>().database.i).GetComponent<TMPro.TextMeshProUGUI>();
        textHolder.transform.SetParent(GameObject.Find("Canvas").transform);
        SetText();
    }

    public Vector2 GetCurrentPosition(int currentPoint)
    {
        return allPoints[currentPoint].transform.position;
    }

    private void Update()
    {
        if (isTraveling == false)
        {
            if (Input.GetKeyDown(KeyCode.A) && allPoints[currentPoint].connectingPoints[2] != null)
            {
                keyPressed = KeyPressed.Left;
                isTraveling = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && allPoints[currentPoint].connectingPoints[3] != null)
            {
                keyPressed = KeyPressed.Right;
                isTraveling = true;
            }
            if (Input.GetKeyDown(KeyCode.W) && allPoints[currentPoint].connectingPoints[0] != null)
            {
                keyPressed = KeyPressed.Up;
                isTraveling = true;
            }
            if (Input.GetKeyDown(KeyCode.S) && allPoints[currentPoint].connectingPoints[1] != null)
            {
                keyPressed = KeyPressed.Down;
                isTraveling = true;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                CrossSceneManagement tempCrossSceneManager = transform.parent.GetComponent<CrossSceneManagement>();
                if (currentPoint != 18)
                {
                    tempCrossSceneManager.LoadScene("BattleScene");
                    tempCrossSceneManager.database.level = currentPoint;
                    tempCrossSceneManager.previousBattleSceneLevel = currentPoint;
                }
                else
                {
                    tempCrossSceneManager.LoadScene("Shop");
                }
            }
        }
        else
        {
            if (isMoving == false)
            {
                if (isLevelAvaliable(allPoints[currentPoint].connectingPoints[GetDirectionalIndex(keyPressed)].index) == true){
                    isMoving = true;
                    StartCoroutine("MovePosition");
                }
                else
                {
                    isTraveling = false;
                }
            }
        }
    }

    public void ExpandLevel()
    {
        if (beatedLevel.Contains(currentPoint) == false)
        {
            beatedLevel.Add(currentPoint);
            allPoints[currentPoint].sr.sprite = allPoints[currentPoint].pointImages[1];
            allPoints[currentPoint].transform.localScale = new Vector2(1.5f, 1.5f);
        }
        int[] tempPoints = GetSurroundingPossiblePoints(currentPoint);
        for (int i = 0; i < 4; i++)
        {
            if (tempPoints[i] != -1)
            {
                if (avaliableLevel.Contains(tempPoints[i]) == false)
                {
                    avaliableLevel.Add(tempPoints[i]);
                    allPoints[tempPoints[i]].sr.sprite = allPoints[tempPoints[i]].pointImages[0];
                }
            }
        }
    }

    IEnumerator MovePosition()
    {
        textHolder.text = null;
        do
        {
            characterIconHolder.transform.position = allPoints[currentPoint].connectingPoints[GetDirectionalIndex(keyPressed)].transform.position;
            currentPoint = allPoints[currentPoint].connectingPoints[GetDirectionalIndex(keyPressed)].index;
            yield return new WaitForSeconds(0.1f);
        } while (allPoints[currentPoint].isPath == true && allPoints[currentPoint].connectingPoints[GetDirectionalIndex(keyPressed)] != null);
        isMoving = false;
        isTraveling = false;
        SetText();
    }

    public int GetDirectionalIndex(KeyPressed keypressed)
    {
        switch (keypressed)
        {
            case KeyPressed.Up:
                return 0;
            case KeyPressed.Down:
                return 1;
            case KeyPressed.Left:
                return 2;
            case KeyPressed.Right:
                return 3;
        }
        return 0;
    }

    public bool IsLevelPoint(int value)
    {
        for(int i = 0; i < mainLevel.Length; i++)
        {
            if (mainLevel[i] == value)
            {
                return true;
            }
        }
        return false;
    }

    public bool isLevelAvaliable(int value)
    {
        for (int i = 0; i < avaliableLevel.Count; i++)
        {
            if (avaliableLevel[i] == value)
            {
                return true;
            }
        }
        if (allPoints[value].isPath == true && allPoints[value].connectingPoints[GetDirectionalIndex(keyPressed)] != null)
        {
            return isLevelAvaliable(allPoints[value].connectingPoints[GetDirectionalIndex(keyPressed)].index);
        }
        return false;
    }

    public void SetText()
    {
        string tempString = "[Z] - Enter ";

        for (int i = 0; i < 4; i++)
        {
            if (allPoints[currentPoint].connectingPoints[i] != null)
            {
                bool tempBool = isLevelAvaliable(allPoints[currentPoint].connectingPoints[i].index);
                if (tempBool == true)
                {
                    switch (i)
                    {
                        case 0:
                            tempString += "[W] - Up ";
                            break;
                        case 1:
                            tempString += "[S] - Down ";
                            break;
                        case 2:
                            tempString += "[A] - Left ";
                            break;
                        case 3:
                            tempString += "[D] - Right ";
                            break;
                    }
                }
            }
        }
        textHolder.text = tempString;
    }

    public int[] GetSurroundingPossiblePoints(int value)
    {
        int[] tempPoints = { -1, -1, -1, -1};
        for (int i = 0; i < 4; i++)
        {
            if (allPoints[value].connectingPoints[i] != null)
            {
                tempPoints[i] = allPoints[GetNearestPointIndex(value, i)].index;
            }
        }
        return tempPoints;
    }

    public int GetNearestPointIndex(int pointIndex, int pointDirection)
    {
        if (allPoints[pointIndex].connectingPoints[pointDirection].isPath == false)
        {
            return allPoints[pointIndex].connectingPoints[pointDirection].index;
        }
        return GetNearestPointIndex(allPoints[pointIndex].connectingPoints[pointDirection].index, pointDirection);
    }
}
