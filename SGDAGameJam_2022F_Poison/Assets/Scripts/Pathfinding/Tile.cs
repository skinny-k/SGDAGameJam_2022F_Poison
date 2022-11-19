using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IComparable
{
    Map parentMap;
    TileRow parentRow;

    public static event Action<Tile> OnClick;
    public static event Action<Tile> OnEnter;
    public static event Action<Tile> OnExit;
    
    public int x;
    public int y;
    public bool walkable = true;
    public int cost;
    public int distance
    {
        get;
        private set;
    }
    public int costPlusDistance
    {
        get => cost + distance;
    }
    public Tile parent;

    void Awake()
    {
        parentRow = transform.parent.GetComponent<TileRow>();
        parentMap = parentRow.transform.parent.GetComponent<Map>();
    }

    void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            OnEnter?.Invoke(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            OnExit?.Invoke(this);
        }
    }
    
    public void SetDistance(int targetX, int targetY)
    {
        this.distance = Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }
        
        Tile compareTile = obj as Tile;
        
        if (compareTile == null || this.costPlusDistance > compareTile.costPlusDistance)
        {
            return 1;
        }
        else if (this.costPlusDistance < compareTile.costPlusDistance)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public List<Tile> GetNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();
        
        if (y - 1 > 0 && parentMap.rows[y - 1].GetTile(x) != null && parentMap.rows[y - 1].GetTile(x).walkable)
        {
            neighbors.Add(parentMap.rows[y - 1].GetTile(x));
        }
        if (y + 1 < parentMap.rows.Length && parentMap.rows[y + 1].GetTile(x) != null && parentMap.rows[y + 1].GetTile(x).walkable)
        {
            neighbors.Add(parentMap.rows[y + 1].GetTile(x));
        }
        if (x - 1 > 0 && parentMap.rows[y].GetTile(x - 1) != null && parentMap.rows[y].GetTile(x - 1).walkable)
        {
            neighbors.Add(parentMap.rows[y].GetTile(x - 1));
        }
        if (y + 1 < parentMap.rows[y].transform.childCount && parentMap.rows[y].GetTile(x + 1) != null && parentMap.rows[y].GetTile(x + 1).walkable)
        {
            neighbors.Add(parentMap.rows[y].GetTile(x + 1));
        }

        return neighbors;
    }
}
