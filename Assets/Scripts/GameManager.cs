using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	//  public = accesible from outside the class
	//  static = the variable belongs to the class itself
	// rather than an instance of the class
	public static GameManager instance = null;
	// public variable of the type BoardManager calld boardScript
	public BoardManager boardScript;
	private int level = 3; 
	
	// Use this for initialization
	void Awake() 
    {
		// Destroy's all game objects but this one
		if (instance == null)
			instance = this; 
		else if (instance != this)
		{	
			// the gameObject this script is attached to 
			Destroy(gameObject);
		}

		// Don't Destroy this gameObject on loading a new Scene 
		DontDestroyOnLoad(gameObject);  

        boardScript = GetComponent<BoardManager>(); 
		InitGame();
	}
	

	void InitGame()
	{
		boardScript.SetUpScene(level); 
	}
	

}