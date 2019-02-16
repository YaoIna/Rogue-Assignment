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
    public int orignalRows = 7;
    public int orignalColumns = 7;

    public GameObject[] floorTiles;

    private Dictionary<Vector2, Vector2> positionGrid = new Dictionary<Vector2, Vector2>();
    //A tranform to hold our floor tiles
    private Transform gameBoardHolder;

    //setup our orignal game board
    public void GameBoardSetup()
    {
        gameBoardHolder = new GameObject("GameBoard").transform;

        for(int x = 0; x < orignalColumns; x++)
        {
            for(int y = 0; y < orignalRows; y++)
            {
                positionGrid.Add(new Vector2(x, y), new Vector2(x, y));
                GameObject randomTile = floorTiles[Random.Range(0, floorTiles.Length)];
                //put randomtile in the right position and set its parent
                GameObject tileInstance = Instantiate(randomTile, new Vector3(x, y, 0f), Quaternion.identity,gameBoardHolder) as GameObject;
            }
        }
    }


}
