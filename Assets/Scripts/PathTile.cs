using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathTile 
{
    public Vector2 position;
    public TileType type;
    public List<Vector2> adjacentTiles;

    public PathTile(TileType type, Vector2 position, int minBound, int maxBound, Dictionary<Vector2,TileType> tiles)
    {
        this.position = position;
        this.type = type;
        adjacentTiles = GetAdjacentPath(minBound, maxBound, tiles);
    }

    private List<Vector2> GetAdjacentPath(int minBound, int maxBound, Dictionary<Vector2,TileType> tiles)
    {
        List<Vector2> pathTiles = new List<Vector2>();
        if(position.y + 1 < maxBound && !tiles.ContainsKey(new Vector2(position.x, position.y + 1))){
            pathTiles.Add(new Vector2(position.x, position.y + 1));
        }
        if (position.x + 1 < maxBound && !tiles.ContainsKey(new Vector2(position.x + 1, position.y)))
        {
            pathTiles.Add(new Vector2(position.x + 1, position.y));
        }
        if (position.y - 1 > minBound && !tiles.ContainsKey(new Vector2(position.x, position.y - 1)))
        {
            pathTiles.Add(new Vector2(position.x, position.y - 1));
        }

        if (position.x - 1 >= minBound && !tiles.ContainsKey(new Vector2(position.x - 1, position.y)) && type != TileType.Essential)
        { 
            pathTiles.Add(new Vector2(position.x - 1, position.y)); 
        }
        return pathTiles;
    }
}