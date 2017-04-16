using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// abstract keyword means must be defined in the class that inherits this
public abstract class MovingObject : MonoBehaviour 
{

	public float moveTime = 0.1f;
	public LayerMask blockingLayer; 

	private BoxCollider2D boxCollider;
	// Use this to hold a reference to body we want to move around
	private Rigidbody2D rb2D;
	// make movement calculations faster 
	private float inverseMoveTime;

	//  can be overidden by inheriting clases
	protected virtual void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime; 
	}
	

	// out let's us return more than one thing. We have the out from the bool, 
	// but also we have the out from the RaycastHit2D
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position; 
		Vector2 end = start + new Vector2(xDir, yDir); 

		// disable own boxcollider so that we don't view hits with self. 
		boxCollider.enabled = false; 
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true; 

		if (hit.transform == null)
		{
			StartCoroutine(SmoothMovement(end)); 
			return true;
		}
		
		return false; 
	}

	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon)
		{
			Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime); 

			//  Moves our rigidbody to the new position 
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude; 
			
			//  this condition waits a frame  
			yield return null;
		}
	}

	protected abstract void OnCantMove <T> (T component)
		where T : Component;

}