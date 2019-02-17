using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 	


public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum; 			
		public int maximum; 			
		
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

    // orginal matrix tiles
    public int originalRows = 7;
    public int originalColumns = 7;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;

    private Dictionary<Vector2, Vector2> positionGrid = new Dictionary<Vector2, Vector2>();
    //A tranform to hold our floor tiles
    private Transform gameBoardHolder;

    //dungeon parameters
    public GameObject exit;
    public GameObject[] outerWallTiles;

    private Transform dungeonBoardHolder;
    private Dictionary<Vector2, Vector2> dungeonPositionGrids;


    //add tile
    private void AddTile(Vector2 position,bool hasWall)
    {
        // if board has this tile, do nothing
        if (positionGrid.ContainsKey(position))
            return;

        //add normal word tile
        positionGrid.Add(position, position);
        GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
        GameObject tileInstance = Instantiate(tile, new Vector3(position.x, position.y, 0f), Quaternion.identity, gameBoardHolder) as GameObject;

        //add tile of dungeon's entrance
        if (Random.Range(0, 100) == 1) {
            tile = exit;
            tileInstance = Instantiate(tile, new Vector3(position.x, position.y, 0f), Quaternion.identity, gameBoardHolder) as GameObject;
        }

        //add walls tile
        if (hasWall)
        {
            //add 0.2 possibility to generate a wall on tile
            if(Random.Range(0,5) == 1)
            {
                tile = wallTiles[Random.Range(0, wallTiles.Length)];
                tileInstance = Instantiate(tile, new Vector3(position.x, position.y, 0f), Quaternion.identity, gameBoardHolder) as GameObject;
            }
        }

    }

    public void setDungeonBoard(Dictionary<Vector2,TileType> tiles,int bound,Vector2 endPos)
    {
        gameBoardHolder.gameObject.SetActive(false);
    }

    //setup our orignal game board
    public void GameBoardSetup()
    {
        gameBoardHolder = new GameObject("GameBoard").transform;

        for(int x = 0; x < originalColumns; x++)
        {
            for(int y = 0; y < originalRows; y++)
            {
                AddTile(new Vector2(x, y), false);
            }
        }
    }

    //extend the game board based on the player position and moving direction
    public void ExtendGameBoard(int stepX, int stepY, Vector2 playerPosition)
    {
        int currentX = (int)playerPosition.x;
        int currentY = (int)playerPosition.y;

        // player moves along x direction
        if (stepY == 0)
        {
            int endX = currentX + 3 * stepX;
            int endY = currentY + 2;
            if (stepX > 0)
            {
                for (currentX += stepX; currentX < endX; currentX += stepX)
                {
                    currentY = (int)playerPosition.y;
                    for (currentY += -1; currentY < endY; currentY++)
                    {
                        AddTile(new Vector2(currentX, currentY), true);
                    }
                }
            }
            else
            {
                for (currentX += stepX; currentX > endX; currentX += stepX)
                {
                    currentY = (int)playerPosition.y;
                    for (currentY += -1; currentY < endY; currentY++)
                    {
                        AddTile(new Vector2(currentX, currentY), true);
                    }
                }
            }
        }
        else
        {
            int endY = currentY + 3 * stepY;
            int endX = currentX + 2;
            if (stepY > 0)
            {
                for (currentY += stepY; currentY < endY; currentY += stepY)
                {
                    currentX = (int)playerPosition.x;
                    for (currentX += -1; currentX < endX; currentX++)
                    {
                        AddTile(new Vector2(currentX, currentY), true);
                    }
                }
            }
            else
            {
                for (currentY += stepY; currentY > endY; currentY += stepY)
                {
                    currentX = (int)playerPosition.x;
                    for (currentX += -1; currentX < endX; currentX++)
                    {
                        AddTile(new Vector2(currentX, currentY), true);
                    }
                }
            }
        }
    }


}
