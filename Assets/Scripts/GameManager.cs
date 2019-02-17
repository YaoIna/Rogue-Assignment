﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public float turnDelay = 0.1f;							//Delay between each Player turn.
	public int healthPoints = 100;							//Starting value for Player health points.
	public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
	[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.

    private BoardManager boardManager;
    private DungeonManager dungeonManger;
	private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
	private bool enemiesMoving;								//Boolean to check if enemies are moving.

	//Awake is always called before any Start functions
	void Awake()
	{
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);	
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		
		//Assign enemies to a new List of Enemy objects.
		enemies = new List<Enemy>();

        //Assign board manager to new board manager
        boardManager = GetComponent<BoardManager>();
        dungeonManger = GetComponent<DungeonManager>();
		
		//Call the InitGame function to initialize the first level 
		InitGame();

        //Add our callback to sceneloaded delegate
        //SceneManager.sceneLoaded += OnSceneLoaded;
        	}


 //   //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
	{
		//Call InitGame to initialize our level.
		InitGame();
	}
    //void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    //{
    //    InitGame();
    //}


    //Initializes the game for each level.
    void InitGame()
	{
		//Clear any Enemy objects in our List to prepare for next level.
		enemies.Clear();

        //setup board manager
        boardManager.GameBoardSetup();
	}
	
	//Update is called every frame.
	void Update()
	{
		//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
		if(playersTurn || enemiesMoving)
			
			//If any of these are true, return and do not start MoveEnemies.
			return;
		
		//Start moving enemies.
		StartCoroutine (MoveEnemies ());
	}

    // update our game board based on player's current position
    public void UpdateGameBoard(int stepX,int stepY,Vector2 playerPosition)
    {
        boardManager.ExtendGameBoard(stepX, stepY, playerPosition);
    }

    public void SwitchToDungeon()
    {
        dungeonManger.CreateDungeon();
        boardManager.SetDungeonBoard(dungeonManger.positionGrid, dungeonManger.maxBound, dungeonManger.endPos);
    }

    public void SwitchToWorld()
    {
        boardManager.SetWorld();
    }

    public Vector2 GetStartPosInDungeon()
    {
        return dungeonManger.startPos;
    }

    //GameOver is called when the player reaches 0 health points
    public void GameOver()
	{
		//Disable this GameManager.
		enabled = false;
	}
	
	//Coroutine to move enemies in sequence.
	IEnumerator MoveEnemies()
	{
		//While enemiesMoving is true player is unable to move.
		enemiesMoving = true;
		
		//Wait for turnDelay seconds, defaults to .1 (100 ms).
		yield return new WaitForSeconds(turnDelay);
		
		//If there are no enemies spawned (IE in first level):
		if (enemies.Count == 0) 
		{
			//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
			yield return new WaitForSeconds(turnDelay);
		}

		playersTurn = true;

		//Enemies are done moving, set enemiesMoving to false.
		enemiesMoving = false;
	}

    private void OnDisable()
    {
        //Remove our callback to sceneloaded delegate
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}