using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour 
{

	public GameObject gameManager; 
	// The Awake function is called on all objects in 
	// the scene before any object's Start function is called.
	void Awake() 
    {
		// using the static variable in the GameManager script
        if (GameManager.instance == null)
		{
			Instantiate(gameManager);
		}
	}
	
}