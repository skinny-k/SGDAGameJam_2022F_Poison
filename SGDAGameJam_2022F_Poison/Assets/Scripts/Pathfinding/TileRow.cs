using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    [SerializeField] public int y = 0;
    
    [SerializeField] Tile[] tiles;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Tile tile = transform.GetChild(i).GetComponent<Tile>();

            tile.x = i;
            tile.y = y;
        }
    }

    public Tile GetTile(int index)
    {
        return transform.GetChild(index).GetComponent<Tile>();
    }
}
