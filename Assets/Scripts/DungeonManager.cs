using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Essential, Random, Empty, Chest, Enemy
}

public class DungeonManager : MonoBehaviour
{
    public int minBound = 0;
    public int maxBound;
    public Dictionary<Vector2, TileType> positionGrid = new Dictionary<Vector2, TileType>();

    public Vector2 startPos;
    public Vector2 endPos;

    public void CreateDungeon()
    {
        positionGrid.Clear();
        maxBound = Random.Range(40, 81);
        BuildEssentialPath();
        BuildRandomPath();

    }


    private void BuildEssentialPath()
    {
        int randomY = Random.Range(0, maxBound + 1);
        startPos = new Vector2(0, randomY);
        PathTile path = new PathTile(TileType.Essential, startPos, minBound, maxBound, positionGrid);
        int boundTracker = 0;
        while (boundTracker < maxBound)
        {
            //the first tile is an empty tile(entrance)
            positionGrid.Add(path.position, TileType.Empty);
            int adjacentTileCount = path.adjacentTiles.Count;
            if (adjacentTileCount <= 0)
                break;
            int randomIndex = Random.Range(0, adjacentTileCount);
            PathTile nextPath = new PathTile(TileType.Essential, path.adjacentTiles[randomIndex], minBound, maxBound, positionGrid);
            if(nextPath.position.x > path.position.x || (((int)nextPath.position.x) == maxBound - 1 && Random.Range(0, 2) == 1)){
                ++boundTracker;
            }
            path = nextPath;
        }
        //the last tile is a empty tile(exit)
        if (!positionGrid.ContainsKey(path.position))
        {
            positionGrid.Add(path.position, TileType.Empty);
        }
        endPos = new Vector2(path.position.x, path.position.y);
    }


    private void BuildRandomPath()
    {
        List<PathTile> pathQueue = new List<PathTile>();
        foreach(KeyValuePair<Vector2,TileType> tile in positionGrid)
        {
            Vector2 randomPos = new Vector2(tile.Key.x, tile.Key.y);
            pathQueue.Add(new PathTile(TileType.Random, randomPos, minBound, maxBound, positionGrid));
        }
        //pathQueue.ForEach(delegate (PathTile tile)
        //{
        //    int adjacent = tile.adjacentTiles.Count;
        //    if (adjacent != 0)
        //    {
        //        if (Random.Range(0, 5) == 1)
        //        {
        //            BuildChamber(tile);
        //        } else if (Random.Range(0, 5) == 1 || (tile.type == TileType.Random && adjacent > 1))
        //        {
        //            int randomIndex = Random.Range(0, adjacent);
        //            Vector2 pathPos = tile.adjacentTiles[randomIndex];
        //            positionGrid.Add(pathPos, TileType.Empty);
        //            pathQueue.Add(new PathTile(TileType.Random, pathPos, minBound, maxBound, positionGrid));
        //        }
        //    }
        //});
        for(int i = 0; i < pathQueue.Count; i++)
        {
                PathTile tile = pathQueue[i];
                int adjacent = tile.adjacentTiles.Count;
                if (adjacent != 0)
                {
                    if (Random.Range(0, 5) == 1)
                    {
                        BuildChamber(tile);
                    } else if (Random.Range(0, 5) == 1 || (tile.type == TileType.Random && adjacent > 1))
                    {
                        int randomIndex = Random.Range(0, adjacent);
                        Vector2 pathPos = tile.adjacentTiles[randomIndex];
                    if (!positionGrid.ContainsKey(pathPos))
                    {
                        if (Random.Range(0, 30) == 1)
                            positionGrid.Add(pathPos, TileType.Enemy);
                        else
                            positionGrid.Add(pathPos, TileType.Empty);

                    }
                    pathQueue.Add(new PathTile(TileType.Random, pathPos, minBound, maxBound, positionGrid));
                    }
                }
        }
    }


    private void BuildChamber(PathTile tile)
    {
        int chamberSize = Random.Range(3, 5);
        int adjacentCount = tile.adjacentTiles.Count;
        int randomIndex = Random.Range(0, adjacentCount);
        Vector2 originalPoint = tile.adjacentTiles[randomIndex];

        for(int x = (int)originalPoint.x;x < originalPoint.x + chamberSize; x++)
        {
            for(int y = (int)originalPoint.y; y < originalPoint.y + chamberSize; y++)
            {
                Vector2 chamberTile = new Vector2(x, y);
                if (!positionGrid.ContainsKey(chamberTile) && chamberTile.x < maxBound && chamberTile.x > 0 && chamberTile.y < maxBound && chamberTile.y > 0)
                {
                    TileType type = Random.Range(0, 50) == 1 ? TileType.Chest : TileType.Empty;
                    positionGrid.Add(chamberTile, type);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}