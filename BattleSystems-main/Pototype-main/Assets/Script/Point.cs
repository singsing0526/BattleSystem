using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Sprite[] pointImages;
    //Up, Down, Left, Right
    public Point[] connectingPoints = new Point[4];
    public bool isPath = true;
    public SpriteRenderer sr;
    public int index;
}
