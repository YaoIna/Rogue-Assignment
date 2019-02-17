using UnityEngine;
using System.Collections;
using UnityEngine.UI;   //Allows us to use UI.
using System.Collections.Generic;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	public int wallDamage = 1;					//How much damage a player does to a wall when chopping it.
	public Text healthText;						//UI Text to display current player health total.
	private Animator animator;					//Used to store a reference to the Player's animator component.
	private int health;
    public bool onWorld;
    public bool dungeonTransition;						//Used to store player health points total during level.

    public int attactNumber;
    public int defenseNumber;
    public Image glove;
    public Image boot;

    private Dictionary<string, Item> inventory;
    private Vector2 currentPosition;
	
	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();
		
		//Get the current health point total stored in GameManager.instance between levels.
		health = GameManager.instance.healthPoints;
		
		//Set the healthText to reflect the current player health total.
		healthText.text = "Health: " + health;

        currentPosition.x = currentPosition.y = 3;

        onWorld = true;
        dungeonTransition = false;

        inventory = new Dictionary<string, Item>();
		
		//Call the Start function of the MovingObject base class.
		base.Start ();
	}
	
	private void Update ()
	{
		//If it's not the player's turn, exit the function.
		if(!GameManager.instance.playersTurn) return;
		
		int horizontal = 0;  	//Used to store the horizontal move direction.
		int vertical = 0;		//Used to store the vertical move direction.

        bool canMove = false;
		
		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		
		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		//Check if moving horizontally, if so set vertical to zero.
		if(horizontal != 0)
		{
			vertical = 0;
		}

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            if (!dungeonTransition)
            {
                //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
                //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                canMove = onWorld ? AttemptMove<Wall>(horizontal, vertical) : AttemptMove<Chest>(horizontal, vertical);

                if (canMove && onWorld)
                {
                    currentPosition.x += horizontal;
                    currentPosition.y += vertical;

                    //update board manager in game manager
                    GameManager.instance.UpdateGameBoard(horizontal, vertical, currentPosition);
                }
            }
        }
	}

    private void SwitchSecene()
    {
        if (onWorld)
        {
            onWorld = false;
            GameManager.instance.SwitchToDungeon();
            dungeonTransition = false;
            transform.position = GameManager.instance.GetStartPosInDungeon();
        }
        else
        {
            onWorld = true;
            GameManager.instance.SwitchToWorld();
            dungeonTransition = false;
            transform.position = currentPosition;
        }
    }

    private void RefillHealth(string itemTag)
    {
        if (health >= 100)
            return;
        switch (itemTag)
        {
            case "Soda":
                health += Random.Range(5, 11);
                break;
            case "Food":
                health += Random.Range(1, 5);
                break;
            default:
                break;
        }
        GameManager.instance.healthPoints = health;
        healthText.text = "Health: " + health;
    }

    private void UpdateInventory(Collider2D item)
    {
        Item itemInstance = item.GetComponent<Item>();
        if (inventory.ContainsKey(itemInstance.itemName))
        {
            inventory[itemInstance.itemName] = itemInstance;
        }
        else
        {
            inventory.Add(itemInstance.itemName, itemInstance);
        }
        switch (itemInstance.itemType)
        {
            case ItemType.Glove:
                glove.color = itemInstance.levelColor;
                break;
            case ItemType.Boot:
                boot.color = itemInstance.levelColor;
                break;
        }

        attactNumber = 0;
        defenseNumber = 0;
        foreach(KeyValuePair<string,Item> gear in inventory)
        {
            attactNumber += gear.Value.attackNum;
            defenseNumber += gear.Value.defenseNum;
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Exit":
                dungeonTransition = true;
                Invoke("SwitchSecene", 0.5f);
                Destroy(collision.gameObject);
                break;
            case "Food":
            case "Soda":
                RefillHealth(collision.tag);
                break;
            case "Item":
                UpdateInventory(collision);
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
        Destroy(collision.gameObject);
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override bool AttemptMove <T> (int xDir, int yDir)
	{	
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		bool hit = base.AttemptMove <T> (xDir, yDir);
		
		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		GameManager.instance.playersTurn = false;

		return hit;
	}
	
	
	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
        if (typeof(T) == typeof(Wall))
        {
            //Set hitWall to equal the component passed in as a parameter.
            Wall hitWall = component as Wall;

            //Call the DamageWall function of the Wall we are hitting.
            hitWall.DamageWall(wallDamage);
        } else if (typeof(T) == typeof(Chest))
        {
            Chest chest = component as Chest;
            chest.Open();
        }
        //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        animator.SetTrigger ("playerChop");
	}
	
	//LoseHealth is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseHealth (int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger ("playerHit");
		
		//Subtract lost health points from the players total.
		health -= loss;
		
		//Update the health display with the new total.
		healthText.text = "-"+ loss + " Health: " + health;
		
		//Check to see if game has ended.
		CheckIfGameOver ();
	}
	
	
	//CheckIfGameOver checks if the player is out of health points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if health point total is less than or equal to zero.
		if (health <= 0) 
		{	
			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();
		}
	}
}