
using System;
using System.Collections;
using System.Collections.Generic; // allows us to use Lists
using UnityEngine;
using Random = UnityEngine.Random; // specify in both system and unity name spaces

public class BoardManager : MonoBehaviour 
{

	// Lays out the level based on the current level
	// Use this for initialization
	
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		// assignment constructor
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max; 
		}
	}

	public int columns = 8; 
	public int rows = 8;

	public Count wallCount = new Count(5,9); 
	public Count foodCount = new Count(1,5);
	
	// only one exit
	public GameObject exit; 

	// spawn arrays of game objects for:
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	
	// will child all gameobjects to boardHolder to keep things tidy
	private Transform boardHolder;

	// use to track all different positions and see if object was spawned there
	// Lists are like dynamic arrays
	private List <Vector3> gridPositions = new List<Vector3>();


	//  creates the list of vectore positions for the game space
	void InitialiseList()
	{
		gridPositions.Clear();
		// -1 so that the's a ring around game board
		for (int x = 1; x < columns -1; x++)
		{
			for( int y = 1; y < rows -1; y++)
			{
				gridPositions.Add(new Vector3(x,y,0f)); 
			}
		}
	}

	// sets the boarder around the map
	void BoardSetup()
	{
		boardHolder = new GameObject("Board").transform;

		for (int x = -1; x < columns + 1; x++)
		{
			for (int y = -1; y < rows +1; y++)
			{
				// defult is floorTiles but overidden at border 
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; 
				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				// Quaternion.identity means no rotation
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject; 
				
				// set the parrent
				instance.transform.SetParent(boardHolder); 
			}
		}
	}

	Vector3 RandomPosition()
	{
		// .Count same as Length for arrays, but Count works on Lists
		int randomIndex = Random.Range(0,gridPositions.Count);
		// chooses a random gridPositions
		Vector3 randomPosition = gridPositions[randomIndex];
		// removes the chosen one so we don't place two tiles per grid
		gridPositions.RemoveAt(randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum )
	{
		// controls how many objects we'll spawn
		int objectCount = Random.Range(minimum, maximum + 1); 

		for(int i = 0; i < objectCount; i++)
		{
			Vector3 randomPosition = RandomPosition(); 
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}

	}

	
	
	public void SetUpScene(int level)
	{
		// lays floor and outer walls the board
		BoardSetup();
		// creates grid positions
		InitialiseList();
		// create random wall positions
		LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum); 

		// generate enemies based on log difficulty
		int enemyCount = (int) Mathf.Log(level, 2f); 

		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount); 
		
		// create the exit 
		Instantiate(exit, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
	}
}