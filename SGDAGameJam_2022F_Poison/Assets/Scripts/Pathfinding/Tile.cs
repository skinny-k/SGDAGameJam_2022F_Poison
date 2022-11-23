using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IComparable, IInteractable
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

    public bool Interact(Player player)
    {
        if (walkable)
        {
            ClickTile(player);
        }
        else
        {
            GetNearestWalkableTile(player).ClickTile(player);
        }
        return true;
    }

    void ClickTile(Player player)
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

    public List<Tile> GetWalkableNeighbors()
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

    /*
    Tile GetNearestWalkableTile(Player player)
    {
        List<Tile> tiles = GetWalkableNeighbors();

        if (tiles.Count > 0)
        {
            Tile nearestTile = null;
            List<Tile> pathToNearestTile = new List<Tile>();
            List<Tile> pathToNewTile = new List<Tile>();

            foreach (Tile tile in tiles)
            {
                Map.FindPath(player.GetComponent<PlayerMovement>().CurrentTile, tile, out pathToNewTile);
                
                if (nearestTile == null || pathToNewTile.Count < pathToNearestTile.Count)
                {
                    nearestTile = tile;
                    pathToNearestTile = new List<Tile>(pathToNewTile);
                    continue;
                }
            }
            
            return nearestTile;
        }
        else
        {
            return null;
        }
    }
    */

    public List<Tile> GetNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();
        
        if (y - 1 > 0 && parentMap.rows[y - 1].GetTile(x) != null)
        {
            neighbors.Add(parentMap.rows[y - 1].GetTile(x));
        }
        if (y + 1 < parentMap.rows.Length && parentMap.rows[y + 1].GetTile(x) != null)
        {
            neighbors.Add(parentMap.rows[y + 1].GetTile(x));
        }
        if (x - 1 > 0 && parentMap.rows[y].GetTile(x - 1) != null)
        {
            neighbors.Add(parentMap.rows[y].GetTile(x - 1));
        }
        if (y + 1 < parentMap.rows[y].transform.childCount && parentMap.rows[y].GetTile(x + 1) != null)
        {
            neighbors.Add(parentMap.rows[y].GetTile(x + 1));
        }

        return neighbors;
    }

    Tile GetNearestWalkableTile(Player player)
    {
        List<Tile> neighbors = GetNeighbors();

        // if the tile has at least one walkable neighbor, find it and return it
        Tile nearestWalkableNeighbor = null;
        List<Tile> pathToNearestTile = new List<Tile>();
        List<Tile> pathToNewTile = new List<Tile>();

        foreach (Tile neighbor in new List<Tile>(neighbors))
        {
            if (neighbor.walkable)
            {
                Map.FindPath(player.GetComponent<PlayerMovement>().CurrentTile, neighbor, out pathToNewTile);
                if (nearestWalkableNeighbor == null || pathToNewTile.Count < pathToNearestTile.Count)
                {
                    nearestWalkableNeighbor = neighbor;
                    pathToNearestTile = new List<Tile>(pathToNewTile);
                }
            }
        }
        if (nearestWalkableNeighbor != null)
        {
            return nearestWalkableNeighbor;
        }
        else
        {
            // if no walkable neighbor exists, find the nearest neighbor to the player and call GetNearestWalkableTile recursively
            Vector3 playerPosWithoutZ = player.transform.position;
            playerPosWithoutZ.z = 0;

            Tile nearestTile = null;
            float distToNearestTile = 0;
            foreach (Tile neighbor in neighbors)
            {
                Vector3 tilePosWithoutZ = neighbor.transform.position;
                tilePosWithoutZ.z = 0;

                float distToNeighbor = Vector3.Distance(tilePosWithoutZ, playerPosWithoutZ);
                if (nearestTile == null || distToNeighbor < distToNearestTile)
                {
                    nearestTile = neighbor;
                    distToNearestTile = distToNeighbor;
                }
            }

            return nearestTile.GetNearestWalkableTile(player);
        }
    }
}
