using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] public TileRow[] rows;

    public static void FindPath(Tile start, Tile finish, out List<Tile> outputPath)
    {
        start.SetDistance(finish.x, finish.y);

        List<Tile> activeTiles = new List<Tile>();
        List<Tile> visitedTiles = new List<Tile>();

        activeTiles.Add(start);

        while (activeTiles.Count > 0)
        {
            activeTiles.Sort();
            Tile checkTile = activeTiles[0];

            if (checkTile == finish)
            {
                visitedTiles.Add(checkTile);

                outputPath = new List<Tile>(visitedTiles);

                return;
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            List<Tile> walkableTiles = GetWalkableTiles(checkTile, finish);

            foreach (Tile walkableTile in walkableTiles)
            {
                if (visitedTiles.Contains(walkableTile))
                {
                    continue;
                }

                if (activeTiles.Contains(walkableTile))
                {
                    int index = activeTiles.IndexOf(walkableTile);
                    Tile existingTile = activeTiles[index];

                    if (existingTile.costPlusDistance > checkTile.costPlusDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                else
                {
                    activeTiles.Add(walkableTile);
                }
            }
        }

        Debug.Log("No path found!");
        outputPath = null;
    }

    static List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
    {
        List<Tile> possibleTiles = currentTile.GetNeighbors();

        foreach (Tile tile in possibleTiles)
        {
            tile.SetDistance(targetTile.x, targetTile.y);
        }

        return possibleTiles;
    }
}
