using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x
    {
        get;
        private set;
    }
    public int y
    {
        get;
        private set;
    }
    public int cost;
    public int distance;
    public Tile parent;

    public void SetDistance(int targetX, int targetY)
    {
        this.distance = Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y);
    }
}
