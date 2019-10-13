using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	[SerializeField]
	private PipeBounds topPipe;
	[SerializeField]
	private PipeBounds botPipe;

	private bool birdPassedPipes = false;
	public bool IsCollidingWith(Transform bird)
	{
		var sr = bird.GetComponent<SpriteRenderer>();
		if(sr == null)
		{
			Debug.LogError("bird sprite renderer was null");
			return false;
		}

		var col = topPipe.IsCollidingWith(sr.bounds) || botPipe.IsCollidingWith(sr.bounds);
		return col;
	}

	public bool DidBirdPass(Vector3 birdPos)
	{
		if(birdPassedPipes)
		{
			return false;
		}

		if(birdPos.x > transform.position.x)
		{
			birdPassedPipes = true;
			return true;
		}
		return false;
	}

	public void ResetPipe()
	{
		birdPassedPipes = false;
	}
}
